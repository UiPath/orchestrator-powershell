using Microsoft.Rest;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace UiPath.PowerShell.Util
{
    internal class NetworkAuthenticationCredentials: ServiceClientCredentials
    {
        public ICredentials Credentials { get; set; }

        public override void InitializeServiceClient<T>(ServiceClient<T> client)
        {

            var handler = client.HttpMessageHandlers.First();
            
            //handler.Credentials = Credentials;
        }
    }
}
