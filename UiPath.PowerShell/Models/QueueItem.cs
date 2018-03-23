using System;
using System.Collections.Generic;
using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Models
{
    public class QueueItem
    {
        public long Id { get; internal set; }
        public long QueueDefinitionId { get; internal set; }
        public string ProcessingExceptionReason { get; private set; }
        public IDictionary<string, object> SpecificContent { get; private set; }
        public string OutputData { get; private set; }
        public QueueItemDtoStatus? Status { get; private set; }
        public QueueItemDtoReviewStatus? ReviewStatus { get; private set; }
        public Guid? Key { get; private set; }
        public string Reference { get; private set; }
        public QueueItemDtoProcessingExceptionType? ProcessingExceptionType { get; private set; }
        public DateTime? DueDate { get; private set; }
        public QueueItemDtoPriority? Priority { get; private set; }
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

        internal static QueueItem FromDto(QueueItemDto queueItem)
        {
            return new QueueItem
            {
                Id = queueItem.Id.Value,
                QueueDefinitionId = queueItem.QueueDefinitionId.Value,
                ProcessingExceptionReason = queueItem.ProcessingException?.Reason,
                SpecificContent = queueItem.SpecificContent.DynamicProperties,
                Output = queueItem.Output?.DynamicProperties,
                OutputData = queueItem.OutputData,
                Status = queueItem.Status,
                ReviewStatus = queueItem.ReviewStatus,
                Key = queueItem.Key,
                Reference = queueItem.Reference,
                ProcessingExceptionType = queueItem.ProcessingExceptionType,
                DueDate = queueItem.DueDate,
                Priority = queueItem.Priority,
                RobotId = queueItem.Robot?.Id,
                DeferDate = queueItem.DeferDate,
                StartProcessing = queueItem.StartProcessing,
                EndProcessing = queueItem.EndProcessing,
                SecondsInPreviousAttempts = queueItem.SecondsInPreviousAttempts,
                AncestorId = queueItem.AncestorId,
                RetryNumber = queueItem.RetryNumber,
                SpecificData = queueItem.SpecificData,
                CreationTime = queueItem.CreationTime,
                Progress = queueItem.Progress,
                RowVersion = queueItem.RowVersion
            };
        }
    }
}
