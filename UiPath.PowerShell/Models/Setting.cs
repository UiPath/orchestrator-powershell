using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Models
{
    public class Setting
    {
        public string Name { get; private set; }
        public SettingsDtoScope? Scope { get; private set; }
        public string Value { get; private set; }

        internal static Setting FromDto(SettingsDto dto)
        {
            return new Setting
            {
                Name = dto.Name,
                Scope = dto.Scope,
                Value = dto.Value
            };
        }

        internal static object FromDto(KeyValuePairStringString dto, SettingsDtoScope? scope = null)
        {
            return new Setting
            {
                Name = dto.Key,
                Value = dto.Value,
                Scope = scope
            };
        }
    }
}
