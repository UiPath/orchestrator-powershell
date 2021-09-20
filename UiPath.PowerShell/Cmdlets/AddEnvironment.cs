using System;
using System.Management.Automation;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;
using UiPath.Web.Client20194.Models;
using Environment = UiPath.PowerShell.Models.Environment;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.Environment)]
    public class AddEnvironment: AuthenticatedCmdlet
    {
        [Parameter (Mandatory = true)]
        public string Name { get; set; }

        [Parameter]
        public string Description { get; set; }

        [ValidateEnum(typeof(EnvironmentDtoType))]
        [Parameter]
        public string Type { get; set; }

        protected override void ProcessRecord()
        {
            var environment = new EnvironmentDto
            {
                Name = Name,
                Description = Description,
            };
            EnvironmentDtoType type;
            if (Enum.TryParse<EnvironmentDtoType>(Type, out type))
            {
                environment.Type = type;
            }
            var dto = HandleHttpOperationException(() => Api.Environments.Post(environment));

            WriteObject(Environment.FromDto(dto));
        }
    }
}
