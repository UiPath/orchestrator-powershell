using Microsoft.Rest;
using Microsoft.Rest.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using UiPath.Web.Client20181;

namespace UiPath.PowerShell.Util
{
    internal static class UiPathWebApiExtenssions
    {
        public static void MakeHttpRequest(this UiPathWebApi api, HttpMethod method, string url, object dto, out string responseContent, out HttpHeaders headers)
        {
            responseContent = null;
            headers = null;
            using (var httpRequest = new HttpRequestMessage())
            {
                httpRequest.Method = new HttpMethod(method.Method);
                httpRequest.RequestUri = new Uri(api.BaseUri, url);

                string requestContent = null;
                if (dto != null)
                {
                    requestContent = SafeJsonConvert.SerializeObject(dto, api.SerializationSettings);
                    httpRequest.Content = new StringContent(requestContent, System.Text.Encoding.UTF8);
                    httpRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
                }

                var shouldTrace = ServiceClientTracing.IsEnabled;
                string invocationId = null;

                if (shouldTrace)
                {
                    invocationId = ServiceClientTracing.NextInvocationId.ToString();
                    ServiceClientTracing.SendRequest(invocationId, httpRequest);
                }

                api.Credentials?.ProcessHttpRequestAsync(httpRequest, CancellationToken.None).Wait();

                using (var httpResponse = api.HttpClient.SendAsync(httpRequest).Result)
                {
                    if (shouldTrace)
                    {
                        ServiceClientTracing.ReceiveResponse(invocationId, httpResponse);
                    }

                    if (httpResponse.Content != null)
                    {
                        responseContent = httpResponse.Content.ReadAsStringAsync().Result;
                    }
                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        var ex = new HttpOperationException(string.Format("Operation returned an invalid status code '{0}'", httpResponse.StatusCode));
                        ex.Request = new HttpRequestMessageWrapper(httpRequest, requestContent);
                        ex.Response = new HttpResponseMessageWrapper(httpResponse, responseContent);
                        throw ex;
                    }
                    headers = httpResponse.Headers;
                }
            }
        }
    }
}
