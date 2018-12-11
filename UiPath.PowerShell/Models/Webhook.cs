using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiPath.Web.Client20184.Models;

namespace UiPath.PowerShell.Models
{
    public class Webhook
    {
        public string Url { get; internal set; }
        public string Secret { get; internal set; }
        public bool Enabled { get; internal set; }
        public long? Id { get; private set; }
        public bool AllowInsecureSsl { get; private set; }
        public WebhookEventDto[] Events { get; private set; }
        public bool AllEvents { get; private set; }

        internal static Webhook FromDto(WebhookDto dto)
        {
            return new Webhook
            {
                Id = dto.Id,
                Url = dto.Url,
                Secret = dto.Secret,
                Enabled = dto.Enabled,
                AllowInsecureSsl = dto.AllowInsecureSsl,
                Events = dto.Events?.ToArray(),
                AllEvents = dto.SubscribeToAllEvents,
            };
        }
    }
}
