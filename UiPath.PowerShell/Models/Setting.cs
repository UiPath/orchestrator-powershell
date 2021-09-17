﻿using System.ComponentModel;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class Setting
    {
        public string Name { get; private set; }
        public string Scope { get; private set; }
        public string Value { get; private set; }

        internal static Setting FromDto(SettingsDto dto)
        {
            return new Setting
            {
                Name = dto.Name,
                Scope = dto.Scope.ToString(),
                Value = dto.Value
            };
        }

        internal static object FromDto(KeyValuePairStringString dto, SettingsDtoScope? scope = null)
        {
            return new Setting
            {
                Name = dto.Key,
                Value = dto.Value,
                Scope = scope?.ToString()
            };
        }
    }
}
