using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Web;
using UiPath.Web.Client.Extensions;

namespace UiPath.PowerShell.Util
{
    public class FilteredBaseCmdlet : AuthenticatedCmdlet
    {
        /// <summary>
        /// Use paging (Skip, Take) for handling large Orchestrator responses
        /// </summary>
        [Parameter]
        public SwitchParameter Paging { get; set; }

        [Parameter]
        public SwitchParameter ExactMatch { get; set; }

        protected void ProcessImpl<TDto>(Func<string, int?, int?, IODataValues<TDto>> getCollection, Func<TDto, object> writer)
        {
            HandlePaging(getCollection, writer);
        }

        protected void ProcessImpl<TDto>(Func<string, IEnumerable<TDto>> getCollection, Func<TDto, object> writer)
        {
            var (odataFilter, localFilter) = BuildFilter<TDto>();
            var response = HandleHttpOperationException(() => getCollection(odataFilter));
            foreach (var dto in response)
            {
                if (ExactMatch.IsPresent && !localFilter(dto))
                {
                    continue;
                }
                WriteObject(writer(dto));
            }
        }

        protected void HandlePaging<TDto>(Func<string, int?, int?, IODataValues<TDto>> getCollection, Func<TDto, object> writer)
        {
            int? top = Paging.IsPresent ? 1000 : default(int?), skip = Paging.IsPresent ? 0 : default(int?), last = 0;
            do
            {
                last = 0;
                var (odataFilter, localFilter) = BuildFilter<TDto>();
                var response = HandleHttpOperationException(() => getCollection(odataFilter, top, skip));
                foreach (var dto in response.Value)
                {
                    ++last;
                    if (ExactMatch.IsPresent && !localFilter(dto))
                    {
                        continue;
                    }
                    WriteObject(writer(dto));
                }
                skip = (skip ?? 0) + last;
            } while (Paging.IsPresent && last == top);
        }

        protected virtual string ImplicitFilter => default;

        protected (string, Func<TDto, bool>) BuildFilter<TDto>()
        {
            Func<TDto, bool> localFilter = (dto) => true;
            StringBuilder sb = new StringBuilder(ImplicitFilter);
            string and = string.IsNullOrWhiteSpace(ImplicitFilter) ? default : " and ";
            foreach (var p in this.GetType()
                .GetProperties()
                .Where(pi => MyInvocation.BoundParameters.ContainsKey(pi.Name))
                .Where(pi => pi.GetCustomAttributes(true)
                    .Where(o => o is FilterAttribute)
                    .Any()))
            {
                var value = p.GetValue(this);
                if (value != null)
                {
                    var type = value.GetType();
                    if (type.IsAssignableFrom(typeof(SwitchParameter)))
                    {
                        SwitchParameter sw = (SwitchParameter)value;
                        value = sw.ToBool();
                    }
                    type = value.GetType(); // value may had changed above
                    string eqToken;
                    if (type.IsAssignableFrom(typeof(bool)))
                    {
                        eqToken = (bool)value ? "true" : "false"; // OData view of True and False
                    }
                    else if (type.IsAssignableFrom(typeof(long)) || type.IsAssignableFrom(typeof(int)))
                    {
                        eqToken = value.ToString(); // "1" or "42"
                    }
                    else if (type.IsAssignableFrom(typeof(Guid)))
                    {
                        eqToken = $"'{value}'";
                    }
                    else
                    {
                        eqToken = $"'{HttpUtility.UrlEncode(value.ToString().Replace("'", "''"))}'";
                    }
                    sb.Append($"{and}{p.Name} eq {eqToken}");

                    var dtoProperty = typeof(TDto).GetProperty(p.Name);
                    if (dtoProperty != null)
                    {
                        var oldFilter = localFilter;
                        Func<TDto, bool> newFilter = (dto) =>
                        {
                            var dtoValue = dtoProperty.GetValue(dto);
                            return string.Compare(dtoValue.ToString(), value.ToString()) == 0;
                        };
                        localFilter = (dto) => oldFilter(dto) && newFilter(dto);
                    }

                    and = " and ";
                }
            }
            WriteVerbose($"filter: {sb}");
            return (sb.Length > 0 ? sb.ToString() : null, localFilter);
        }
    }
}
