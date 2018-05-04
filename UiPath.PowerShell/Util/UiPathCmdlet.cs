using Microsoft.Rest;
using Newtonsoft.Json.Linq;
using System;
using System.Management.Automation;
using System.Net;

namespace UiPath.PowerShell.Util
{
    public abstract class UiPathCmdlet: PSCmdlet
    {
        private bool Ignored = BindingResolver.Ignored;


        protected T HandleHttpOperationException<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch(HttpOperationException ex)
            {
                if (TryExtractErrorMessage(ex.Response?.Content, out var message, out var error))
                {
                    WriteError(new ErrorRecord(ex, message, MapHttpResponseToErrorCategory(ex.Response.StatusCode), null));
                }
                throw;
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
                    WriteError(new ErrorRecord(ex, message, MapHttpResponseToErrorCategory(ex.Response.StatusCode), null));
                }
                throw;
            }
        }

        protected static ErrorCategory MapHttpResponseToErrorCategory(HttpStatusCode code)
        {
            if ((int) code >= 300 && (int) code < 400)
            {
                return ErrorCategory.AuthenticationError;
            }
            else 
            {
                return ErrorCategory.NotSpecified;
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
                message = jo["message"].Value<string>();
                error = jo["errorCode"].Value<int>();
                success = true;
            }
            catch(Exception e)
            {
                WriteVerbose($"Error: ${e.GetType().Name}:${e.Message} extracting error message from {content}");
            }
            return success;
        }
    }
}
