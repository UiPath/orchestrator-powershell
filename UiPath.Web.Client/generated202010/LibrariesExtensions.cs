// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace UiPath.Web.Client202010
{
    using Models;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for Libraries.
    /// </summary>
    public static partial class LibrariesExtensions
    {
            /// <summary>
            /// Gets the library packages.
            /// </summary>
            /// <remarks>
            /// Client Credentials Flow required permissions: Execution or Execution.Read.
            ///
            /// Required permissions: Libraries.View.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='searchTerm'>
            /// </param>
            /// <param name='expand'>
            /// Indicates the related entities to be represented inline. The maximum depth
            /// is 2.
            /// </param>
            /// <param name='filter'>
            /// Restricts the set of items returned. The maximum number of expressions is
            /// 100.
            /// </param>
            /// <param name='select'>
            /// Limits the properties returned in the result.
            /// </param>
            /// <param name='orderby'>
            /// Specifies the order in which items are returned. The maximum number of
            /// expressions is 5.
            /// </param>
            /// <param name='top'>
            /// Limits the number of items returned from a collection. The maximum value is
            /// 1000.
            /// </param>
            /// <param name='skip'>
            /// Excludes the specified number of items of the queried collection from the
            /// result.
            /// </param>
            /// <param name='count'>
            /// Indicates whether the total count of items within a collection are returned
            /// in the result.
            /// </param>
            public static ODataValueOfIEnumerableOfLibraryDto Get(this ILibraries operations, string searchTerm = "", string expand = default(string), string filter = default(string), string select = default(string), string orderby = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?))
            {
                return operations.GetAsync(searchTerm, expand, filter, select, orderby, top, skip, count).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets the library packages.
            /// </summary>
            /// <remarks>
            /// Client Credentials Flow required permissions: Execution or Execution.Read.
            ///
            /// Required permissions: Libraries.View.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='searchTerm'>
            /// </param>
            /// <param name='expand'>
            /// Indicates the related entities to be represented inline. The maximum depth
            /// is 2.
            /// </param>
            /// <param name='filter'>
            /// Restricts the set of items returned. The maximum number of expressions is
            /// 100.
            /// </param>
            /// <param name='select'>
            /// Limits the properties returned in the result.
            /// </param>
            /// <param name='orderby'>
            /// Specifies the order in which items are returned. The maximum number of
            /// expressions is 5.
            /// </param>
            /// <param name='top'>
            /// Limits the number of items returned from a collection. The maximum value is
            /// 1000.
            /// </param>
            /// <param name='skip'>
            /// Excludes the specified number of items of the queried collection from the
            /// result.
            /// </param>
            /// <param name='count'>
            /// Indicates whether the total count of items within a collection are returned
            /// in the result.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ODataValueOfIEnumerableOfLibraryDto> GetAsync(this ILibraries operations, string searchTerm = "", string expand = default(string), string filter = default(string), string select = default(string), string orderby = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetWithHttpMessagesAsync(searchTerm, expand, filter, select, orderby, top, skip, count, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Deletes a package.
            /// </summary>
            /// <remarks>
            /// Client Credentials Flow required permissions: Execution or Execution.Write.
            ///
            /// Required permissions: Libraries.Delete.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='key'>
            /// </param>
            public static void DeleteById(this ILibraries operations, string key)
            {
                operations.DeleteByIdAsync(key).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Deletes a package.
            /// </summary>
            /// <remarks>
            /// Client Credentials Flow required permissions: Execution or Execution.Write.
            ///
            /// Required permissions: Libraries.Delete.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='key'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task DeleteByIdAsync(this ILibraries operations, string key, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.DeleteByIdWithHttpMessagesAsync(key, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Downloads the .nupkg file of a Package.
            /// </summary>
            /// <remarks>
            /// Client Credentials Flow required permissions: Execution or Execution.Read.
            ///
            /// Required permissions: Libraries.View.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='key'>
            /// </param>
            public static Stream DownloadPackageByKey(this ILibraries operations, string key)
            {
                return operations.DownloadPackageByKeyAsync(key).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Downloads the .nupkg file of a Package.
            /// </summary>
            /// <remarks>
            /// Client Credentials Flow required permissions: Execution or Execution.Read.
            ///
            /// Required permissions: Libraries.View.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='key'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<Stream> DownloadPackageByKeyAsync(this ILibraries operations, string key, CancellationToken cancellationToken = default(CancellationToken))
            {
                var _result = await operations.DownloadPackageByKeyWithHttpMessagesAsync(key, null, cancellationToken).ConfigureAwait(false);
                _result.Request.Dispose();
                return _result.Body;
            }

            /// <summary>
            /// Returns a collection of all available versions of a given package. Allows
            /// odata query options.
            /// </summary>
            /// <remarks>
            /// Client Credentials Flow required permissions: Execution or Execution.Read.
            ///
            /// Required permissions: Libraries.View.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='packageId'>
            /// The Id of the package for which the versions are fetched.
            /// </param>
            /// <param name='expand'>
            /// Indicates the related entities to be represented inline. The maximum depth
            /// is 2.
            /// </param>
            /// <param name='filter'>
            /// Restricts the set of items returned. The maximum number of expressions is
            /// 100.
            /// </param>
            /// <param name='select'>
            /// Limits the properties returned in the result.
            /// </param>
            /// <param name='orderby'>
            /// Specifies the order in which items are returned. The maximum number of
            /// expressions is 5.
            /// </param>
            /// <param name='top'>
            /// Limits the number of items returned from a collection. The maximum value is
            /// 1000.
            /// </param>
            /// <param name='skip'>
            /// Excludes the specified number of items of the queried collection from the
            /// result.
            /// </param>
            /// <param name='count'>
            /// Indicates whether the total count of items within a collection are returned
            /// in the result.
            /// </param>
            public static ODataValueOfIEnumerableOfLibraryDto GetVersionsByPackageid(this ILibraries operations, string packageId, string expand = default(string), string filter = default(string), string select = default(string), string orderby = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?))
            {
                return operations.GetVersionsByPackageidAsync(packageId, expand, filter, select, orderby, top, skip, count).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns a collection of all available versions of a given package. Allows
            /// odata query options.
            /// </summary>
            /// <remarks>
            /// Client Credentials Flow required permissions: Execution or Execution.Read.
            ///
            /// Required permissions: Libraries.View.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='packageId'>
            /// The Id of the package for which the versions are fetched.
            /// </param>
            /// <param name='expand'>
            /// Indicates the related entities to be represented inline. The maximum depth
            /// is 2.
            /// </param>
            /// <param name='filter'>
            /// Restricts the set of items returned. The maximum number of expressions is
            /// 100.
            /// </param>
            /// <param name='select'>
            /// Limits the properties returned in the result.
            /// </param>
            /// <param name='orderby'>
            /// Specifies the order in which items are returned. The maximum number of
            /// expressions is 5.
            /// </param>
            /// <param name='top'>
            /// Limits the number of items returned from a collection. The maximum value is
            /// 1000.
            /// </param>
            /// <param name='skip'>
            /// Excludes the specified number of items of the queried collection from the
            /// result.
            /// </param>
            /// <param name='count'>
            /// Indicates whether the total count of items within a collection are returned
            /// in the result.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ODataValueOfIEnumerableOfLibraryDto> GetVersionsByPackageidAsync(this ILibraries operations, string packageId, string expand = default(string), string filter = default(string), string select = default(string), string orderby = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetVersionsByPackageidWithHttpMessagesAsync(packageId, expand, filter, select, orderby, top, skip, count, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Uploads a new package or a new version of an existing package. The content
            /// of the package is sent as a .nupkg file embedded in the HTTP request.
            /// </summary>
            /// <remarks>
            /// Client Credentials Flow required permissions: Execution or Execution.Write.
            ///
            /// Required permissions: Libraries.Create.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='file'>
            /// </param>
            /// <param name='expand'>
            /// Indicates the related entities to be represented inline. The maximum depth
            /// is 2.
            /// </param>
            /// <param name='filter'>
            /// Restricts the set of items returned. The maximum number of expressions is
            /// 100.
            /// </param>
            /// <param name='select'>
            /// Limits the properties returned in the result.
            /// </param>
            /// <param name='orderby'>
            /// Specifies the order in which items are returned. The maximum number of
            /// expressions is 5.
            /// </param>
            /// <param name='count'>
            /// Indicates whether the total count of items within a collection are returned
            /// in the result.
            /// </param>
            /// <param name='file1'>
            /// </param>
            /// <param name='file2'>
            /// </param>
            /// <param name='file3'>
            /// </param>
            /// <param name='file4'>
            /// </param>
            /// <param name='file5'>
            /// </param>
            /// <param name='file6'>
            /// </param>
            /// <param name='file7'>
            /// </param>
            /// <param name='file8'>
            /// </param>
            /// <param name='file9'>
            /// </param>
            public static ODataValueOfIEnumerableOfBulkItemDtoOfString UploadPackage(this ILibraries operations, Stream file, string expand = default(string), string filter = default(string), string select = default(string), string orderby = default(string), bool? count = default(bool?), Stream file1 = default(Stream), Stream file2 = default(Stream), Stream file3 = default(Stream), Stream file4 = default(Stream), Stream file5 = default(Stream), Stream file6 = default(Stream), Stream file7 = default(Stream), Stream file8 = default(Stream), Stream file9 = default(Stream))
            {
                return operations.UploadPackageAsync(file, expand, filter, select, orderby, count, file1, file2, file3, file4, file5, file6, file7, file8, file9).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Uploads a new package or a new version of an existing package. The content
            /// of the package is sent as a .nupkg file embedded in the HTTP request.
            /// </summary>
            /// <remarks>
            /// Client Credentials Flow required permissions: Execution or Execution.Write.
            ///
            /// Required permissions: Libraries.Create.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='file'>
            /// </param>
            /// <param name='expand'>
            /// Indicates the related entities to be represented inline. The maximum depth
            /// is 2.
            /// </param>
            /// <param name='filter'>
            /// Restricts the set of items returned. The maximum number of expressions is
            /// 100.
            /// </param>
            /// <param name='select'>
            /// Limits the properties returned in the result.
            /// </param>
            /// <param name='orderby'>
            /// Specifies the order in which items are returned. The maximum number of
            /// expressions is 5.
            /// </param>
            /// <param name='count'>
            /// Indicates whether the total count of items within a collection are returned
            /// in the result.
            /// </param>
            /// <param name='file1'>
            /// </param>
            /// <param name='file2'>
            /// </param>
            /// <param name='file3'>
            /// </param>
            /// <param name='file4'>
            /// </param>
            /// <param name='file5'>
            /// </param>
            /// <param name='file6'>
            /// </param>
            /// <param name='file7'>
            /// </param>
            /// <param name='file8'>
            /// </param>
            /// <param name='file9'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ODataValueOfIEnumerableOfBulkItemDtoOfString> UploadPackageAsync(this ILibraries operations, Stream file, string expand = default(string), string filter = default(string), string select = default(string), string orderby = default(string), bool? count = default(bool?), Stream file1 = default(Stream), Stream file2 = default(Stream), Stream file3 = default(Stream), Stream file4 = default(Stream), Stream file5 = default(Stream), Stream file6 = default(Stream), Stream file7 = default(Stream), Stream file8 = default(Stream), Stream file9 = default(Stream), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.UploadPackageWithHttpMessagesAsync(file, expand, filter, select, orderby, count, file1, file2, file3, file4, file5, file6, file7, file8, file9, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}