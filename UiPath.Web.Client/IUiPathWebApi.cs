using Microsoft.Rest;
using Newtonsoft.Json;

namespace UiPath.Web.Client.Generic
{
    public interface IUiPathWebApi
    {
        JsonSerializerSettings SerializationSettings { get; }

        JsonSerializerSettings DeserializationSettings { get; }

        System.Uri BaseUri { get; set; }

        ServiceClientCredentials Credentials { get; }
    }
}
