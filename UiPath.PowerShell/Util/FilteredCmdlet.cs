using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace UiPath.PowerShell.Util
{
    public abstract class FilteredCmdlet: AuthenticatedCmdlet
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

        protected string BuildFilter()
        {
            StringBuilder sb = new StringBuilder();
            string and = null;
            foreach(var p in this.GetType()
                .GetProperties()
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
                        SwitchParameter sw = (SwitchParameter) value;
                        if (!sw.IsPresent)
                        {
                            continue;
                        }
                        value = true;
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
                    else
                    {
                        eqToken = $"'{value.ToString().Replace("'", "''")}'";
                    }
                    sb.Append($"{and}{p.Name} eq {eqToken}");
                    and = " and ";
                }
            }
            return sb.Length > 0 ? sb.ToString() : null;
        }
    }
}
