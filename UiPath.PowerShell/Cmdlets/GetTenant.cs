﻿using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Nouns.Tenant)]
    public class GetTenant: FilteredIdCmdlet
    {
        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string Name { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string AdminName { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string AdminSurname { get; private set; }

        [Filter]
        [Parameter(ParameterSetName = "Filter")]
        public string AdminEmailAddress { get; private set; }

        protected override void ProcessRecord()
        {
            ProcessImpl(
                (filter, top, skip) => Api.Tenants.GetTenants(filter: filter, top: top, skip: skip, count: false),
                id => Api.Tenants.GetById((int)id),
                dto => Tenant.ForDto(dto));
        }
    }
}
