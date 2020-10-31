using Microsoft.Rest;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using UiPath.PowerShell.Models;

namespace UiPath.PowerShell.Util
{
    public abstract class UiPathCmdlet : PSCmdlet
    {
        private bool ProtocolIgnored = SecurityProtocolFix.Ignored;

        private static PSCmdlet _cmdlet;

        internal static RestVerboseTracer VerboseTracer { get; set; }

        static UiPathCmdlet()
        {
            VerboseTracer = new RestVerboseTracer();
            ServiceClientTracing.AddTracingInterceptor(VerboseTracer);
            ServiceClientTracing.IsEnabled = true;
        }

        internal static void DebugMessage(string message)
        {
            _cmdlet.WriteDebug(message);
        }

        internal bool VerboseEnabled
        {
            get
            {
                var verbosePreference = GetVariableValue("global:VerbosePreference") as ActionPreference?;
                if (verbosePreference.HasValue && verbosePreference.Value != ActionPreference.SilentlyContinue)
                    return true;
                if (MyInvocation.BoundParameters.TryGetValue("Verbose", out var value))
                {
                    return  value is bool && (bool) value;
                }
                return false;
            }
        }

        internal bool DebugEnabled
        {
            get
            {
                var debugPreference = GetVariableValue("global:DebugPreference") as ActionPreference?;
                if (debugPreference.HasValue && debugPreference.Value != ActionPreference.SilentlyContinue)
                    return true;
                if (MyInvocation.BoundParameters.TryGetValue("Debug", out var value))
                {
                    return value is bool && (bool)value;
                }
                return false;
            }
        }

        protected override void BeginProcessing()
        {
            _cmdlet = this;

            RestVerboseTracer.ETraceFlags traceFlags = 0;

            if (VerboseEnabled)
            {
                traceFlags |= RestVerboseTracer.ETraceFlags.Enabled;
            }

            if (DebugEnabled)
            {
                traceFlags |= RestVerboseTracer.ETraceFlags.Headers;
                traceFlags |= RestVerboseTracer.ETraceFlags.Content;
            }

            VerboseTracer.StartTracing(traceFlags);
            base.BeginProcessing();
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            VerboseTracer.Flush(this);
            VerboseTracer.StopTracing();
            _cmdlet = null;
        }

        protected override void StopProcessing()
        {
            base.StopProcessing();
            _cmdlet = null;
        }

        /// <summary>
        /// Handle the swagger generated idiocy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        internal T HandleHttpResponseException<T>(Func<Task<HttpOperationResponse<T>>> action) => HandleHttpOperationException(() =>
            {
                var task = action();
                var response = task.Result;
                if (!response.Response.IsSuccessStatusCode)
                {
                    var ex = new HttpOperationException(string.Format("Operation returned an invalid status code '{0}'", response.Response.StatusCode));
                    ex.Request = new HttpRequestMessageWrapper(response.Request, string.Empty); //Can't access disposed request content
                    ex.Response = new HttpResponseMessageWrapper(response.Response, response.Response?.Content?.AsString() ?? string.Empty);
                    throw ex;
                }
                return response.Body;
            });

        internal void HandleHttpResponseException(Func<Task<HttpOperationResponse>> action) => HandleHttpOperationException(() =>
        {
            var task = action();
            var response = task.Result;
            if (!response.Response.IsSuccessStatusCode)
            {
                var ex = new HttpOperationException(string.Format("Operation returned an invalid status code '{0}'", response.Response.StatusCode));
                ex.Request = new HttpRequestMessageWrapper(response.Request, response.Request?.Content?.AsString() ?? string.Empty);
                ex.Response = new HttpResponseMessageWrapper(response.Response, response.Response?.Content?.AsString() ?? string.Empty);
                throw ex;
            }
        });

        protected T HandleHttpOperationException<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (HttpOperationException ex)
            {
                if (TryExtractErrorMessage(ex.Response?.Content, out var message, out var error))
                {
                    WriteError(error, message);
                }
                throw;
            }
            finally
            {
                VerboseTracer.Flush(this);
            }
        }

        protected void HandleHttpOperationException(Action action)
        {
            try
            {
                action();
            }
            catch (HttpOperationException ex)
            {
                if (TryExtractErrorMessage(ex.Response?.Content, out var message, out var error))
                {
                    WriteError(error, message);
                }
                throw;
            }
        }

        protected bool TryExtractErrorMessage(string content, out string message, out int? error)
        {
            bool success = false;
            message = null;
            error = null;
            try
            {
                var jo = JObject.Parse(content);
                message = jo["message"]?.Value<string>();
                error = jo["errorCode"]?.Value<int>();

                message = message ?? jo["error"]?["message"]?.Value<string>();
                error = error ?? jo["error"]?["code"]?.Value<int>();

                if (!error.HasValue && String.IsNullOrWhiteSpace(message))
                {
                    throw new RuntimeException("Cannot parse response");
                }
                success = true;
            }
            catch(Exception e)
            {
                WriteVerbose($"Error: ${e.GetType().Name}:${e.Message} extracting error message from {content}");
            }
            return success;
        }

        protected void WriteError(string message, ErrorCategory errorCategory = ErrorCategory.InvalidResult)
        {
            var ex = new RuntimeException(message);
            WriteError(new ErrorRecord(ex, null, errorCategory, null));
        }

        protected void WriteError(int? errorCode, string errorMessage)
        {
            WriteError($"The operation returned an error: {errorCode}: {errorMessage}");
        }

        internal static void ApplyEnumMember<TEnum>(string stringValue, Action<TEnum> action) where TEnum : struct
        {
            if (Enum.TryParse<TEnum>(stringValue, out var enumValue))
            {
                action(enumValue);
            }
        }

        internal void SetCurrentFolder(AuthToken authToken, string folderPath, TimeSpan timeout)
        {
            var oldFolderId = authToken.OrganizationUnitId;
            authToken.OrganizationUnitId = default;
            try
            {
                using (var api = AuthenticatedCmdlet.MakeApi_19_10(authToken, timeout))
                {
                    var folders = HandleHttpResponseException(() => api.Folders.GetFoldersWithHttpMessagesAsync(filter: $"FullyQualifiedName eq '{Uri.EscapeDataString(folderPath)}'")).Value;
                    if (folders.Count != 1)
                    {
                        throw new Exception($"The folder path '{folderPath}' does not select exactly one Folder");
                    }

                    var folder = folders.Single();
                    authToken.CurrentFolder = Folder.FromDto(folder);
                    authToken.OrganizationUnit = default;
                    authToken.OrganizationUnitId = folder.Id;
                }
            }
            catch
            {
                authToken.OrganizationUnitId = oldFolderId;
                throw;
            }
        }
    }
}
