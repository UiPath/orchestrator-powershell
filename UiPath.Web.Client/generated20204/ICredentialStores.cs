// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace UiPath.Web.Client20204
{
    using Microsoft.Rest;
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// CredentialStores operations.
    /// </summary>
    public partial interface ICredentialStores
    {
        /// <summary>
        /// Gets all Credential Stores.
        /// </summary>
        /// <remarks>
        /// Client Credentials Flow required permissions: Settings or
        /// Settings.Read.
        ///
        /// Required permissions: Settings.View or Assets.Create or Assets.Edit
        /// or Assets.View or Robots.Create or Robots.Edit or Robots.View.
        /// </remarks>
        /// <param name='expand'>
        /// Expands related entities inline.
        /// </param>
        /// <param name='filter'>
        /// Filters the results, based on a Boolean condition.
        /// </param>
        /// <param name='select'>
        /// Selects which properties to include in the response.
        /// </param>
        /// <param name='orderby'>
        /// Sorts the results.
        /// </param>
        /// <param name='top'>
        /// Returns only the first n results.
        /// </param>
        /// <param name='skip'>
        /// Skips the first n results.
        /// </param>
        /// <param name='count'>
        /// Includes a count of the matching results in the response.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        Task<HttpOperationResponse<ODataValueIEnumerableCredentialStoreDto>> GetWithHttpMessagesAsync(string expand = default(string), string filter = default(string), string select = default(string), string orderby = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), Dictionary<string, List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Creates a new Credential Store.
        /// </summary>
        /// <remarks>
        /// Client Credentials Flow required permissions: Settings or
        /// Settings.Write.
        ///
        /// Required permissions: Settings.Create.
        /// </remarks>
        /// <param name='credentialStoreDto'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<HttpOperationResponse<CredentialStoreDto>> PostWithHttpMessagesAsync(CredentialStoreDto credentialStoreDto, Dictionary<string, List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Gets a single Credential Store by its key.
        /// </summary>
        /// <remarks>
        /// Client Credentials Flow required permissions: Settings or
        /// Settings.Read.
        ///
        /// Required permissions: Settings.View.
        /// </remarks>
        /// <param name='id'>
        /// key: Id
        /// </param>
        /// <param name='expand'>
        /// Expands related entities inline.
        /// </param>
        /// <param name='select'>
        /// Selects which properties to include in the response.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        Task<HttpOperationResponse<CredentialStoreDto>> GetByIdWithHttpMessagesAsync(long id, string expand = default(string), string select = default(string), Dictionary<string, List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Updates a Credential Store.
        /// </summary>
        /// <remarks>
        /// Client Credentials Flow required permissions: Settings or
        /// Settings.Write.
        ///
        /// Required permissions: Settings.Edit.
        /// </remarks>
        /// <param name='id'>
        /// key: Id
        /// </param>
        /// <param name='credentialStoreDto'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<HttpOperationResponse> PutByIdWithHttpMessagesAsync(long id, CredentialStoreDto credentialStoreDto, Dictionary<string, List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Deletes a Credential Store.
        /// </summary>
        /// <remarks>
        /// Client Credentials Flow required permissions: Settings or
        /// Settings.Write.
        ///
        /// Required permissions: Settings.Delete.
        /// </remarks>
        /// <param name='id'>
        /// key: Id
        /// </param>
        /// <param name='forceDelete'>
        /// </param>
        /// <param name='ifMatch'>
        /// If-Match header
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        Task<HttpOperationResponse> DeleteByIdWithHttpMessagesAsync(long id, bool? forceDelete = default(bool?), bool? ifMatch = default(bool?), Dictionary<string, List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Sets a credential store as the default for the given credential
        /// type.
        /// </summary>
        /// <remarks>
        /// Client Credentials Flow required permissions: Settings or
        /// Settings.Write.
        ///
        /// Required permissions: Settings.Edit.
        /// </remarks>
        /// <param name='id'>
        /// key: Id
        /// </param>
        /// <param name='defaultStoreActionParameters'>
        /// Provides the resourceType that indicates
        /// the resource type for which the stores becomes default.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<HttpOperationResponse> SetDefaultStoreForResourceTypeByIdWithHttpMessagesAsync(long id, DefaultStoreActionParameters defaultStoreActionParameters, Dictionary<string, List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Get the default credential store for the given resource type.
        /// </summary>
        /// <remarks>
        /// Client Credentials Flow required permissions: Settings or
        /// Settings.Read.
        ///
        /// Required permissions: Settings.View or Assets.Create or Assets.Edit
        /// or Assets.View or Robots.Create or Robots.Edit or Robots.View.
        /// </remarks>
        /// <param name='resourceType'>
        /// Provides the resource type for which to retrieve the default.
        /// Possible values include: 'AssetCredential', 'RobotCredential',
        /// 'BucketCredential'
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<HttpOperationResponse<ODataValueInt64>> GetDefaultStoreForResourceTypeByResourcetypeWithHttpMessagesAsync(string resourceType, Dictionary<string, List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Gets available Credential Store types.
        /// </summary>
        /// <remarks>
        /// Client Credentials Flow required permissions: Settings or
        /// Settings.Read.
        ///
        /// Required permissions: Settings.View.
        /// </remarks>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        Task<HttpOperationResponse<ODataValueIEnumerableString>> GetAvailableCredentialStoreTypesWithHttpMessagesAsync(Dictionary<string, List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Gets available resources robots (and later assets) for a credential
        /// store.
        /// </summary>
        /// <remarks>
        /// Client Credentials Flow required permissions: Settings or
        /// Settings.Read.
        ///
        /// Required permissions: Settings.View.
        /// </remarks>
        /// <param name='key'>
        /// Provides the ID of the credential store for which to retrieve
        /// resources.
        /// </param>
        /// <param name='resourceType'>
        /// Possible values include: 'AssetCredential', 'RobotCredential',
        /// 'BucketCredential'
        /// </param>
        /// <param name='expand'>
        /// Expands related entities inline.
        /// </param>
        /// <param name='filter'>
        /// Filters the results, based on a Boolean condition.
        /// </param>
        /// <param name='select'>
        /// Selects which properties to include in the response.
        /// </param>
        /// <param name='orderby'>
        /// Sorts the results.
        /// </param>
        /// <param name='top'>
        /// Returns only the first n results.
        /// </param>
        /// <param name='skip'>
        /// Skips the first n results.
        /// </param>
        /// <param name='count'>
        /// Includes a count of the matching results in the odata-count header.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<HttpOperationResponse<ODataValueIEnumerableCredentialStoreResourceDto>> GetResourcesForCredentialStoreTypesByKeyAndResourcetypeWithHttpMessagesAsync(long key, string resourceType, string expand = default(string), string filter = default(string), string select = default(string), string orderby = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), Dictionary<string, List<string>> customHeaders = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    }
}