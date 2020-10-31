using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net.Http;
using System.Text;

namespace UiPath.PowerShell.Util
{
    internal class RestVerboseTracer : IServiceClientTracingInterceptor
    {
        [Flags]
        public enum ETraceFlags
        {
            Enabled = 0x01,
            Headers = 0x02,
            Content = 0x04,
        };

        private volatile StringBuilder _traces;
        private ETraceFlags _traceFlags = 0;

        public bool IsEnabled => _traceFlags.HasFlag(ETraceFlags.Enabled);

        public void Configuration(string source, string name, string value)
        {
        }

        public void EnterMethod(string invocationId, object instance, string method, IDictionary<string, object> parameters)
        {
        }

        public void ExitMethod(string invocationId, object returnValue)
        {
        }

        internal void StartTracing(ETraceFlags traceFlags)
        {
            _traceFlags = traceFlags;
            if (IsEnabled)
            {
                _traces = new StringBuilder();
            }
        }

        internal void Flush(PSCmdlet cmdlet)
        {
            if (IsEnabled)
            {
                var oldTraces = _traces;
                _traces = new StringBuilder();
                if (oldTraces.Length > 0)
                {
                    cmdlet.WriteVerbose(oldTraces.ToString());
                }
            }
        }

        internal void StopTracing()
        {
            _traceFlags = 0;
            _traces = null; ;
        }

        public void Information(string message)
        {
            if (IsEnabled)
            {
                _traces.AppendLine(message);
            }
        }

        public void ReceiveResponse(string invocationId, HttpResponseMessage response)
        {
            if (IsEnabled)
            {
                _traces.AppendLine($"HTTP response: {response.StatusCode}");
                if (_traceFlags.HasFlag(ETraceFlags.Headers))
                {
                    _traces.AppendLine(response.Headers.ToString());
                }
                if (_traceFlags.HasFlag(ETraceFlags.Content) && null != response.Content)
                {
                    _traces.AppendLine(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        public void SendRequest(string invocationId, HttpRequestMessage request)
        {
            if (IsEnabled)
            {
                _traces.AppendLine($"HTTP request: {request.Method} {request.RequestUri}");
                if (_traceFlags.HasFlag(ETraceFlags.Headers))
                {
                    _traces.AppendLine(request.Headers.ToString());
                }
                if (_traceFlags.HasFlag(ETraceFlags.Content) && null != request.Content)
                {
                    _traces.AppendLine(request.Content.ReadAsStringAsync().Result);
                }
            }

        }

        public void TraceError(string invocationId, Exception exception)
        {
            if (IsEnabled)
            {
                _traces.AppendLine($"HTTP exception: {exception.GetType().Name}:{exception.Message}");
            }
        }
    }
}
