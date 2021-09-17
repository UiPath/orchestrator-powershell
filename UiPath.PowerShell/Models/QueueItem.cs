using System;
using System.Collections.Generic;
using System.ComponentModel;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20194.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class QueueItem
    {
        public long Id { get; internal set; }
        public long QueueDefinitionId { get; internal set; }
        public string ProcessingExceptionReason { get; private set; }
        public IDictionary<string, object> SpecificContent { get; private set; }
        public string OutputData { get; private set; }
        public string Status { get; private set; }
        public string ReviewStatus { get; private set; }
        public Guid? Key { get; private set; }
        public string Reference { get; private set; }
        public string ProcessingExceptionType { get; private set; }
        public DateTime? DueDate { get; private set; }
        public string Priority { get; private set; }
        public long? RobotId { get; private set; }
        public DateTime? DeferDate { get; private set; }
        public DateTime? StartProcessing { get; private set; }
        public DateTime? EndProcessing { get; private set; }
        public int? SecondsInPreviousAttempts { get; private set; }
        public long? AncestorId { get; private set; }
        public int? RetryNumber { get; private set; }
        public string SpecificData { get; private set; }
        public DateTime? CreationTime { get; private set; }
        public string Progress { get; private set; }
        public byte[] RowVersion { get; private set; }
        public IDictionary<string, object> Output { get; private set; }

        internal static QueueItem FromDto(QueueItemDto queueItem) => queueItem.To<QueueItem>();
    }
}
