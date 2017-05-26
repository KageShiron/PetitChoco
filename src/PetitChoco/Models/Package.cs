using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using NuGet;

namespace PetitChoco.Models
{
    public class Package : LocalPackage
    {
        public Uri PackageSourceUrl { get; set; }
        public Uri ProjectSourceUrl { get; set; }
        public Uri DocsUrl { get; set; }
        public Uri MailingListUrl { get; set; }
        public Uri BugTrackerUrl { get; set; }


        public Package()
        {
            
        }
        public override Stream GetStream()
        {
            throw new NotImplementedException();
        }

        public override void ExtractContents(IFileSystem fileSystem, string extractPath)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<IPackageFile> GetFilesBase()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<IPackageAssemblyReference> GetAssemblyReferencesCore()
        {
            throw new NotImplementedException();
        }
    }
}
