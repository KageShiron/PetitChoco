using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet;
using PetitChoco.Models;

namespace PetitChoco.Nuspec
{
    class NuspecReader
    {
        public static Package Read(string fileName)
        {
            var reader = new NuGet.Packaging.PackageFolderReader(fileName).NuspecReader;
            var pack = new Package();
            pack.Id = reader.GetId();
            pack.Version = new SemanticVersion(reader.GetVersion().Version);
            pack.Owners = reader.GetOwners().Split();
            Uri uri;
            bool p = Uri.TryCreate(reader.GetMetadataValue("packageSourceUrl"),UriKind.Absolute, out uri);
            if(p)pack.PackageSourceUrl = uri;

            pack.Title = reader.GetTitle();
            pack.Authors = reader.GetAuthors().Split();
            Uri.TryCreate(reader.GetProjectUrl(), UriKind.Absolute, out uri);
            if (p) pack.ProjectUrl = uri;
            Uri.TryCreate(reader.GetIconUrl(), UriKind.Absolute, out uri);
            if (p) pack.IconUrl = uri;
            pack.Copyright = reader.GetCopyright();
            Uri.TryCreate(reader.GetLicenseUrl(), UriKind.Absolute, out uri);
            if (p) pack.LicenseUrl = uri;
            Uri.TryCreate(reader.GetProjectUrl(), UriKind.Absolute, out uri);
            if (p) pack.ProjectSourceUrl = uri;
            Uri.TryCreate(reader.GetMetadataValue("docsUrl"), UriKind.Absolute, out uri);
            if (p) pack.DocsUrl = uri;
            Uri.TryCreate(reader.GetMetadataValue("mailingListUrl"), UriKind.Absolute, out uri);
            if (p) pack.MailingListUrl = uri;
            Uri.TryCreate(reader.GetMetadataValue("bugTrackerUrl"), UriKind.Absolute, out uri);
            if (p) pack.BugTrackerUrl = uri;
            pack.Tags = reader.GetTags();
            pack.Summary = reader.GetSummary();
            pack.Description = reader.GetDescription();

            pack.ReleaseNotes = reader.GetReleaseNotes();


            return pack;
        }
    }
}
