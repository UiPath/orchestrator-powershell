using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20204;
using UiPath.Web.Client20204.Models;

namespace UiPath.PowerShell.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, Nouns.Bucket)]
    public class AddBucket : AuthenticatedCmdlet
    {
        private const string FileSystemSet = nameof(FileSystemSet);
        private const string AmazonSet = nameof(AmazonSet);
        private const string AzureSet = nameof(AzureSet);
        private const string MinioSet = nameof(MinioSet);
        private const string OrchestratorSet = nameof(OrchestratorSet);

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = OrchestratorSet)]
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = FileSystemSet)]
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = AmazonSet)]
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = AzureSet)]
        [Parameter(Mandatory = true, Position = 1, ParameterSetName = MinioSet)]
        public string Name { get; set; }

        [Parameter]
        public string Description { get; set; }

        [Parameter]
        public SwitchParameter ReadOnly { get; set; }

        [Parameter]
        public SwitchParameter AuditReadAccess { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = FileSystemSet)]
        public SwitchParameter FileSystem { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = FileSystemSet)]
        public string Path { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = AmazonSet)]
        public SwitchParameter Aws { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, ParameterSetName = AmazonSet)]
        public string S3BucketName { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, ParameterSetName = AmazonSet)]
        public string EndpointRegion { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, ParameterSetName = AmazonSet)]
        [Parameter(Mandatory = true, ParameterSetName = MinioSet)]
        public string AccessKey { get; set; }

        [ValidateNotNull]
        [Parameter(Mandatory = true, ParameterSetName = AmazonSet)]
        [Parameter(Mandatory = true, ParameterSetName = MinioSet)]
        public string SecretKey { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = AzureSet)]
        public SwitchParameter Azure { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = AzureSet)]
        [Parameter(Mandatory = true, ParameterSetName = MinioSet)]
        public string ContainerName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = AzureSet)]
        public string AccountName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = AzureSet)]
        public string AccountKey { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = AzureSet)]
        public string EndpointSuffix { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = MinioSet)]
        public SwitchParameter MinIO { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = MinioSet)]
        public string Server { get; set; }

        [Parameter(ParameterSetName = AzureSet)]
        [Parameter(ParameterSetName = AmazonSet)]
        [Parameter(ParameterSetName = MinioSet)]
        public CredentialStore CredentialStore { get; set; }

        protected override void ProcessRecord()
        {
            var dto = new BucketDto
            {
                Name = Name,
                Description = Description,
                Options = BucketDtoOptions.None,
                Identifier = Guid.NewGuid(),
            };

            if (ReadOnly.IsPresent)
            {
                dto.Options |= BucketDtoOptions.ReadOnly;
            }

            if (AuditReadAccess.IsPresent)
            {
                dto.Options |= BucketDtoOptions.AuditReadAccess;
            }

            switch(ParameterSetName)
            {
                case FileSystemSet:
                    dto.StorageProvider = "FileSystem";
                    dto.StorageParameters = $"RootPath={Path}";
                    break;
                case AmazonSet:
                    dto.StorageProvider = "Amazon";
                    dto.Password = SecretKey;
                    dto.StorageContainer = S3BucketName;
                    dto.StorageParameters = $"EndpointRegion={EndpointRegion};accessKey={AccessKey};secretKey=$Password";
                    dto.CredentialStoreId = CredentialStore?.Id ?? this.GetDefaultCredentialStore(CredentialStoreResourceDtoType.BucketCredential.ToString());
                    break;
                case AzureSet:
                    dto.StorageProvider = "Azure";
                    dto.Password = AccountKey;
                    dto.StorageContainer = ContainerName;
                    dto.StorageParameters = $"EndpointSuffix={EndpointSuffix};AccountName={AccountName};AccountKey=$Password";
                    dto.CredentialStoreId = CredentialStore?.Id ?? this.GetDefaultCredentialStore(CredentialStoreResourceDtoType.BucketCredential.ToString());
                    break;
                case MinioSet:
                    dto.StorageProvider = "Minio";
                    dto.Password = SecretKey;
                    dto.StorageContainer = ContainerName;
                    dto.StorageParameters = $"host={Server};accessKey={AccessKey};secretKey=$Password";
                    dto.CredentialStoreId = CredentialStore?.Id ?? this.GetDefaultCredentialStore(CredentialStoreResourceDtoType.BucketCredential.ToString());
                    break;
            }

            dto = Api_20_4.Buckets.Post(dto);
            WriteObject(dto);
        }
    }
}
