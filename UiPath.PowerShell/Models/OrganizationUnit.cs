using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Models
{
    public class OrganizationUnit
    {
        public long Id { get; set; }

        public string DisplayName { get; set; }

        public static OrganizationUnit FromDto(OrganizationUnitDto dto)
        {
            return new OrganizationUnit
            {
                Id = dto.Id.Value,
                DisplayName = dto.DisplayName
            };
        }
    }
}
