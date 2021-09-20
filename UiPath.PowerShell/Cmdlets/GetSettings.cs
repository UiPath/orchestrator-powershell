using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Setting)]
    public class GetSettings : FilteredBaseCmdlet
    {
        public enum SettingsType
        {
            General,
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
            }
        }
    }
}
