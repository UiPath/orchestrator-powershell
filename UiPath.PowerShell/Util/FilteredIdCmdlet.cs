using System;
using System.Collections.Generic;
using System.Management.Automation;
using UiPath.Web.Client.Extensions;

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
                var dto = HandleHttpOperationException(() => getItem(Id.Value));
                WriteObject(writer(dto));
            }
            else
            {
                var (odataFilter, localFilter) = BuildFilter<TDto>();
                var dtos = HandleHttpOperationException(() => getCollection(odataFilter));
                foreach(var dto in dtos)
                {
                    if (ExactMatch.IsPresent && !localFilter(dto))
                    {
                        continue;
                    }
                    WriteObject(writer(dto));
                }
            }
        }

        protected void ProcessImpl<TDto>(Func<string, int?, int?, IODataValues<TDto>> getCollection, Func<long, TDto> getItem, Func<TDto, object> writer)
        {
            if (Id.HasValue)
            {
                var dto = HandleHttpOperationException(() => getItem(Id.Value));
                WriteObject(writer(dto));
            }
            else
            {
                HandlePaging(getCollection, writer);
            }
        }
    }
}
