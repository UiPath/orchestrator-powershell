using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiPath.Web.Client20181.Models;

namespace UiPath.PowerShell.Models
{
    public class Environment
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public EnvironmentDtoType Type { get; private set; }

        internal static Environment FromDto(EnvironmentDto dto)
        {
            return new Environment
            {
                Id = dto.Id.Value,
                Name = dto.Name,
                Description = dto.Description,
                Type = dto.Type
            };
        }
    }
}
