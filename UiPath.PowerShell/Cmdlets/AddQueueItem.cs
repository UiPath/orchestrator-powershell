using System;
using System.Collections;
using System.Linq;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.QueueItem)]
    public class AddQueueItem: AuthenticatedCmdlet
    {
        [ValidateNotNullOrEmpty]
        [Parameter(Mandatory = true)]
        public string QueueName { get; set; }

        [ValidateEnum(typeof(QueueItemDtoPriority))]
        [Parameter]
        public string Priority { get; set; }

        [Parameter]
        public DateTime?  DeferDate { get; set; }

        [Parameter]
        public DateTime? DueDate { get; set; }

        [Parameter]
        public string Reference { get; set; }

        [Parameter]
        public Hashtable SpecificContent { get; set; }

        protected override void ProcessRecord()
        {
            var dto = new QueueItemDataDto
            {
                Name = QueueName,
                Reference = Reference,
                DeferDate = DeferDate,
                DueDate = DueDate,
                SpecificContent =  new QueueItemSpecificContent
                {
                  DynamicProperties = SpecificContent.Cast<DictionaryEntry>().ToDictionary(kv => (string) kv.Key, kv => kv.Value)
                }
            };
            QueueItemDataDtoPriority priority;
            if (Enum.TryParse<QueueItemDataDtoPriority>(Priority, out priority))
            {
                dto.Priority = priority;
            }
            var queueItem = Api.Queues.AddQueueItem(new QueueItemParameters
            {
                ItemData = dto
            });

            WriteObject(QueueItem.FromDto(queueItem));
        }
    }
}
