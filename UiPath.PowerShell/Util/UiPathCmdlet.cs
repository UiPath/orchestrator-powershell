using Microsoft.Rest;
using Newtonsoft.Json.Linq;
using System;
using System.Management.Automation;

namespace UiPath.PowerShell.Util
{
    public abstract class UiPathCmdlet : PSCmdlet
    {
        private bool Ignored = BindingResolver.Ignored;
        private bool ProtocolIgnored = SecurityProtocolFix.Ignored;

        private static RestVerboseTracer VerboseTracer { get; set; }

        static UiPathCmdlet()
        {
            VerboseTracer = new RestVerboseTracer();
            ServiceClientTracing.AddTracingInterceptor(VerboseTracer);
            ServiceClientTracing.IsEnabled = true;
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
        }

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
    }
}
