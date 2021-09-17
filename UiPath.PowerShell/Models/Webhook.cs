using System.Collections;
using System.ComponentModel;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class Webhook
    {
        public string Url { get; internal set; }
        public string Secret { get; internal set; }
        public bool Enabled { get; internal set; }
        public long? Id { get; private set; }
        public bool AllowInsecureSsl { get; private set; }
        public Hashtable[] Events { get; private set; }
        public bool AllEvents { get; private set; }

        internal static Webhook FromDto(WebhookDto dto) => dto.To<Webhook>();
    }
}
