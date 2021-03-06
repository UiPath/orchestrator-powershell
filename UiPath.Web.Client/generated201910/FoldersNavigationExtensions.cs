// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace UiPath.Web.Client201910
{
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for FoldersNavigation.
    /// </summary>
    public static partial class FoldersNavigationExtensions
    {
            /// <summary>
            /// Returns the folders the current user has access to.
            /// The response will be a list of folders; the hierarchy can be reconstructed
            /// using the ParentId properties. From the root to the folders the user has
            /// actually been assigned to, the folders will be marked as non-selectable
            /// and only the paths to the assigned-to folders will be included.
            /// From the assigned-to folders down to the leaves, the nodes will be marked
            /// as
            /// selectable and their children lists fully populated.
            /// </summary>
            /// <remarks>
            /// Requires authentication.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<ExtendedFolderDto> GetAllFoldersForCurrentUser(this IFoldersNavigation operations)
            {
                return operations.GetAllFoldersForCurrentUserAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns the folders the current user has access to.
            /// The response will be a list of folders; the hierarchy can be reconstructed
            /// using the ParentId properties. From the root to the folders the user has
            /// actually been assigned to, the folders will be marked as non-selectable
            /// and only the paths to the assigned-to folders will be included.
            /// From the assigned-to folders down to the leaves, the nodes will be marked
            /// as
            /// selectable and their children lists fully populated.
            /// </summary>
            /// <remarks>
            /// Requires authentication.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<ExtendedFolderDto>> GetAllFoldersForCurrentUserAsync(this IFoldersNavigation operations, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.GetAllFoldersForCurrentUserWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Returns a filtered subset (paginated) of the folders the current user has
            /// access to.
            /// </summary>
            /// <remarks>
            /// Requires authentication.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='skip'>
            /// </param>
            /// <param name='take'>
            /// </param>
            /// <param name='searchText'>
            /// </param>
            public static PageResultDtoFolderDto GetFoldersForCurrentUser(this IFoldersNavigation operations, int skip, int take, string searchText = default(string))
            {
                return operations.GetFoldersForCurrentUserAsync(skip, take, searchText).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns a filtered subset (paginated) of the folders the current user has
            /// access to.
            /// </summary>
            /// <remarks>
            /// Requires authentication.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='skip'>
            /// </param>
            /// <param name='take'>
            /// </param>
            /// <param name='searchText'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<PageResultDtoFolderDto> GetFoldersForCurrentUserAsync(this IFoldersNavigation operations, int skip, int take, string searchText = default(string), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.GetFoldersForCurrentUserWithHttpMessagesAsync(skip, take, searchText, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Returns a subset (paginated) of direct children for a given folder, which
            /// are accessible to the current user.
            /// To ease a folder tree structure navigation design, the list of ancestors
            /// for the given folder is also returned.
            /// </summary>
            /// <remarks>
            /// Requires authentication.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='skip'>
            /// </param>
            /// <param name='take'>
            /// </param>
            /// <param name='folderId'>
            /// </param>
            public static FolderNavigationContextDto GetFolderNavigationContextForCurrentUser(this IFoldersNavigation operations, int skip, int take, long? folderId = default(long?))
            {
                return operations.GetFolderNavigationContextForCurrentUserAsync(skip, take, folderId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns a subset (paginated) of direct children for a given folder, which
            /// are accessible to the current user.
            /// To ease a folder tree structure navigation design, the list of ancestors
            /// for the given folder is also returned.
            /// </summary>
            /// <remarks>
            /// Requires authentication.
            /// </remarks>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='skip'>
            /// </param>
            /// <param name='take'>
            /// </param>
            /// <param name='folderId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<FolderNavigationContextDto> GetFolderNavigationContextForCurrentUserAsync(this IFoldersNavigation operations, int skip, int take, long? folderId = default(long?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
            {
                using (var _result = await operations.GetFolderNavigationContextForCurrentUserWithHttpMessagesAsync(skip, take, folderId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
