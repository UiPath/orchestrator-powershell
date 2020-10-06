using System.Collections.Generic;
using UiPath.Web.Client.Extensions;

namespace UiPath.Web.Client.Extensions
{
    public interface IODataValues<TDto>
    {
        string Odatacontext { get; }

        IList<TDto> Value { get; }
    }
}

namespace UiPath.Web.Client20181.Models
{
    public partial class ODataResponseListAssetDto : IODataValues<AssetDto>
    {
    }

    public partial class ODataResponseListProcessDto : IODataValues<ProcessDto>
    {
    }

    public partial class ODataResponseListSettingsDto : IODataValues<SettingsDto>
    {
    }

    public partial class ODataResponseListEnvironmentDto : IODataValues<EnvironmentDto>
    {
    }

    public partial class ODataResponseListJobDto : IODataValues<JobDto>
    {
    }

    public partial class ODataResponseListOrganizationUnitDto : IODataValues<OrganizationUnitDto>
    {
    }

    public partial class ODataResponseListProcessDto : IODataValues<ProcessDto>
    {
    }

    public partial class ODataResponseListReleaseDto : IODataValues<ReleaseDto>
    {
    }

    public partial class ODataResponseListProcessScheduleDto : IODataValues<ProcessScheduleDto>
    {
    }

    public partial class ODataResponseListQueueDefinitionDto : IODataValues<QueueDefinitionDto>
    {
    }

    public partial class ODataResponseListRobotDto : IODataValues<RobotDto>
    {
    }

    public partial class ODataResponseListTenantDto : IODataValues<TenantDto>
    {
    }

    public partial class ODataResponseListUserDto : IODataValues<UserDto>
    {
    }

    public partial class ODataResponseListKeyValuePairStringString : IODataValues<KeyValuePairStringString>
    {
    }
}

namespace UiPath.Web.Client20182.Models
{
    public partial class ODataResponseListMachineDto : IODataValues<MachineDto>
    {
    }
}


namespace UiPath.Web.Client20183.Models
{
    public partial class ODataResponseListLibraryDto : IODataValues<LibraryDto>
    {
    }

    public partial class ODataResponseListMachineDto : IODataValues<MachineDto>
    {
    }

    public partial class ODataResponseListRobotDto : IODataValues<RobotDto>
    {
    }

    public partial class ODataResponseListJobDto : IODataValues<JobDto>
    {
    }

    public partial class ODataResponseListReleaseDto : IODataValues<ReleaseDto>
    {
    }
}

namespace UiPath.Web.Client20184.Models
{
    public partial class ODataResponseListWebhookDto : IODataValues<WebhookDto>
    {
    }
}

namespace UiPath.Web.Client201910.Models
{
    public partial class ODataResponseListFolderDto : IODataValues<FolderDto>
    {

    }

    public partial class ODataResponseListUserRolesDto: IODataValues<UserRolesDto>
    {

    }

    public partial class ODataResponseListCredentialStoreDto: IODataValues<CredentialStoreDto>
    {

    }

    public partial  class ODataResponseListUserDto : IODataValues<UserDto>
    {

    }

    public partial class ODataResponseListRobotDto : IODataValues<RobotDto>
    {

    }

    public partial class ODataResponseListReleaseDto : IODataValues<ReleaseDto>
    {

    }
}

namespace UiPath.Web.Client20204.Models
{
    public partial class ODataValueIEnumerableBucketDto : IODataValues<BucketDto>
    {

    }

    public partial class ODataValueIEnumerableUserDto : IODataValues<UserDto>
    {

    }

    public partial class ODataValueIEnumerableRobotDto : IODataValues<RobotDto>
    {

    }

    public partial class ODataValueIEnumerableExtendedMachineDto : IODataValues<ExtendedMachineDto>
    {

    }

    public partial class ODataValueIEnumerableMachineFolderDto : IODataValues<MachineFolderDto>
    {

    }
}
