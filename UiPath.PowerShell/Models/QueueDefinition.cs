using UiPath.Web.Client.Models;

namespace UiPath.PowerShell.Models
{
    public class QueueDefinition
    {
        public long Id { get; internal set; }

        public string Name { get; internal set; }

        public string Description { get; internal set; }

        public bool? AcceptAutomaticallyRetry { get; internal set; }

        public bool? EnforceUniqueReference { get; internal set; }

        public int? MaxNumberOfRetries { get; internal set; }

        internal static QueueDefinition FromDto(QueueDefinitionDto queue)
        {
            return new QueueDefinition
            {
                Id = queue.Id.Value,
                Name = queue.Name,
                AcceptAutomaticallyRetry = queue.AcceptAutomaticallyRetry,
                Description = queue.Description,
                EnforceUniqueReference = queue.EnforceUniqueReference,
                MaxNumberOfRetries = queue.MaxNumberOfRetries
            };
        }

        internal QueueDefinitionDto ToDto(QueueDefinition queueDefinition)
        {
            return new QueueDefinitionDto
            {
                Id = queueDefinition.Id,
                Name = queueDefinition.Name,
                Description = queueDefinition.Description,
                AcceptAutomaticallyRetry = queueDefinition.AcceptAutomaticallyRetry,
                EnforceUniqueReference = queueDefinition.EnforceUniqueReference,
                MaxNumberOfRetries = queueDefinition.MaxNumberOfRetries
            };
        }
    }
}
