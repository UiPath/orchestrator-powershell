using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Models
{
    public class Timezone
    {
        public string Name { get; internal set; }
        public string Value { get; internal set; }

        internal static Timezone FromDto(NameValueDto dto)
        {
            return new Timezone
            {
                Name = dto.Name,
                Value = dto.Value
            };
        }
    }
}
