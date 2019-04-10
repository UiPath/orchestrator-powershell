using System;
using System.ComponentModel;
using UiPath.PowerShell.Util;
using UiPath.Web.Client20183.Models;

namespace UiPath.PowerShell.Models
{
    [TypeConverter(typeof(UiPathTypeConverter))]
    public class Library
    {
        public string Id { get; private set; }
        public string Authors { get; private set; }
        public string Description { get; private set; }
        public bool? IsLatestVersion { get; private set; }
        public string Key { get; private set; }
        public string OldVersion { get; private set; }
        public DateTime? Published { get; private set; }
        public string ReleaseNotes { get; private set; }
        public string Title { get; private set; }
        public string Version { get; private set; }

        public static Library FromDto(LibraryDto dto)
        {
            return new Library
            {
                Id = dto.Id,
                Authors =dto.Authors,
                Description =dto.Description,
                IsLatestVersion = dto.IsLatestVersion,
                Key = dto.Key,
                OldVersion = dto.OldVersion,
                Published = dto.Published,
                ReleaseNotes = dto.ReleaseNotes,
                Title = dto.Title,
                Version = dto.Version,
            };
        }
    }
}
