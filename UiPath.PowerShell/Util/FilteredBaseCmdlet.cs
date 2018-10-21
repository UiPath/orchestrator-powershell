using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Web;

namespace UiPath.PowerShell.Util
{
    public class FilteredBaseCmdlet : AuthenticatedCmdlet
    {
        protected void ProcessImpl<TDto>(Func<string, IList<TDto>> getCollection, Func<TDto, object> writer)
        {
            var dtos = HandleHttpOperationException(() => getCollection(BuildFilter()));
            foreach (var dto in dtos)
            {
                WriteObject(writer(dto));
            }
        }

        protected string BuildFilter()
        {
            StringBuilder sb = new StringBuilder();
            string and = null;
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
                    else if (type.IsAssignableFrom(typeof(Guid)))
                    {
                        eqToken = $"'{value.ToString()}'";
                    }
                    else
                    {
                        eqToken = $"'{HttpUtility.UrlEncode(value.ToString().Replace("'", "''"))}'";
                    }
                    sb.Append($"{and}{p.Name} eq {eqToken}");
                    and = " and ";
                }
            }
            WriteVerbose($"filter: {sb}");
            return sb.Length > 0 ? sb.ToString() : null;
        }
    }
}
