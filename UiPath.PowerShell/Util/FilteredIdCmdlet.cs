using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace UiPath.PowerShell.Util
{
    public abstract class FilteredIdCmdlet: FilteredBaseCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "Id")]
        public long? Id { get; set; }

        /*
        [Parameter(Mandatory = false, ParameterSetName = "Filter")]
        public SwitchParameter Filter { get; set; }
        */

            /*
        [Parameter(Mandatory = false, ParameterSetName = "All")]
        public SwitchParameter All { get; set; }
        */

        protected void ProcessImpl<TDto>(Func<string, IList<TDto>> getCollection, Func<long, TDto> getItem, Func<TDto, object> writer)
        {
            if (Id.HasValue)
            {
                var dto = getItem(Id.Value);
                WriteObject(writer(dto));
            }
            else
            {
                var dtos = getCollection(BuildFilter());
                foreach(var dto in dtos)
                {
                    WriteObject(writer(dto));
                }
            }
        }
    }
}
