using System;
using System.Management.Automation;
using UiPath.PowerShell.Models;
using UiPath.PowerShell.Util;

namespace UiPath.PowerShell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Changes the current Folder under which all other cmdlets are evaluated</para>
    /// <example>
    /// <code>Set-UiPathCurrentFolder Some/Folder/Path</code>
    /// </example>
    /// </summary>
    [Cmdlet(VerbsCommon.Set, Nouns.FolderCurrent)]
    public class SetCurrentFolder : AuthenticatedCmdlet
    {
        public const string FolderSet = "FolderSet";
        public const string FolderPathSet = "FolderPathSet";

        [Parameter(Mandatory = true, ParameterSetName = FolderSet, Position = 1, ValueFromPipeline = true)]
        public Folder Folder { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = FolderPathSet, Position = 1)]
        public string FolderPath { get; set; }

        protected override void ProcessRecord()
        {
            var token = InternalAuthToken;

            SetCurrentFolder(token, Folder?.FullyQualifiedName ?? FolderPath, TimeSpan.FromSeconds(token.RequestTimeout ?? 100));

            WriteObject(token);
        }
    }
}
