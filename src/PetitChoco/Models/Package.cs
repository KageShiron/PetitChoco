using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using NuGet;
using Prism.Mvvm;

namespace PetitChoco.Models
{
    public class Package : BindableBase
    {
        public string DirectoryName { get; }
        public string RequireLicenseAcceptance { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ReleaseNotes { get; set; }
        public string Language { get; set; }
        public string Tags { get; set; }
        public string Copyright { get; set; }
        public string LicenseUrl { get; set; }
        public string ProjectUrl { get; set; }
        public string Id { get; set; }
        public string Version { get; set; }
        public string Authors { get; set; }
        public string Owners { get; set; }
        public string IconUrl { get; set; }
        public string Title { get; set; }
        public string PackageSourceUrl { get; set; }
        public string ProjectSourceUrl { get; set; }
        public string DocsUrl { get; set; }
        public string MailingListUrl { get; set; }
        public string BugTrackerUrl { get; set; }
        public DirectoryInfo DirectoryInfo =>  new DirectoryInfo(DirectoryName);

        public Package( string dirName )
        {
            DirectoryName = dirName;
            var reader = new NuGet.Packaging.PackageFolderReader(dirName).NuspecReader;
            Id = reader.GetId();
            Version = reader.GetMetadataValue(nameof(Version));
            Owners = reader.GetMetadataValue(nameof(Owners));
            ProjectSourceUrl = reader.GetMetadataValue(nameof(ProjectSourceUrl));
            Title = reader.GetMetadataValue(nameof(Title));
            Authors = reader.GetMetadataValue(nameof(Authors));
            ProjectUrl = reader.GetIconUrl();
            IconUrl = reader.GetIconUrl();
            Copyright = reader.GetCopyright();
            LicenseUrl = reader.GetLicenseUrl();
            ProjectSourceUrl = reader.GetMetadataValue(nameof(ProjectSourceUrl));
            DocsUrl = reader.GetMetadataValue(nameof(DocsUrl));
            MailingListUrl = reader.GetMetadataValue(nameof(MailingListUrl));
            BugTrackerUrl = reader.GetMetadataValue(nameof(BugTrackerUrl));
            Tags = reader.GetTags();
            Summary = reader.GetSummary();
            Description = reader.GetDescription();
            ReleaseNotes = reader.GetReleaseNotes();
        }
    }
}
