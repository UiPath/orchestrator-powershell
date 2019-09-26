using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20181;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Setting)]
    public class GetSettings : FilteredBaseCmdlet
    {
        public enum SettingsType
        {
            General,
            Authentication,
            Web
        }

        [Parameter]
        [ValidateEnum(typeof(SettingsType))]
        public string Type { get; set; }

        protected override void ProcessRecord()
        {
            SettingsType type = SettingsType.General;
            Enum.TryParse<SettingsType>(Type, out type);

            switch (type)
            {
                case SettingsType.General:
                    ProcessImpl(
                        (filter, top, skip) => Api.Settings.GetSettings(filter: filter, top: top, skip: skip, count: false),
                        dto => Setting.FromDto(dto));
                    break;
                case SettingsType.Authentication:
                    ProcessImpl(
                        filter => Api.Settings.GetAuthenticationSettings().Value,
                        dto => Setting.FromDto(dto));
                    break;
                case SettingsType.Web:
                    ProcessImpl(
                        filter => Api.Settings.GetWebSettings().Value,
                        dto => Setting.FromDto(dto));
                    break;
            }
        }
    }
}
