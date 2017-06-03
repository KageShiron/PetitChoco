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
    public class MetaData
    {
        private static IDictionary<string,KnwonMetaData> KnwonMetaData { get; }
        static MetaData()
        {
            KnwonMetaData = new Dictionary<string,KnwonMetaData>()
            {
                {"id",new KnwonMetaData("id", "", EditMode.PackageId)},
                {"version",new KnwonMetaData("version", "", EditMode.Version)},
                {"packageSourceUrl",new KnwonMetaData("packageSourceUrl", "", EditMode.Url)},
                {"owners",new KnwonMetaData("owners", "", EditMode.SingleLineString)},

                {"title",new KnwonMetaData("title", "", EditMode.SingleLineString)},
                {"authors",new KnwonMetaData("authors", "", EditMode.CommaSeparatedStrings)},
                {"projectUrl",new KnwonMetaData("projectUrl", "", EditMode.Url)},
                {"iconUrl",new KnwonMetaData("iconUrl", "", EditMode.Url)},
                {"copyright",new KnwonMetaData("copyright", "", EditMode.SingleLineString)},
                {"licenseUrl",new KnwonMetaData("licenseUrl", "", EditMode.Url)},
                {"requireLicenseAcceptance",new KnwonMetaData("requireLicenseAcceptance", "", EditMode.Boolean)},
                {"projectSourceUrl",new KnwonMetaData("projectSourceUrl", "", EditMode.Url)},
                {"docsUrl",new KnwonMetaData("docsUrl", "", EditMode.Url)},
                {"mailingListUrl",new KnwonMetaData("mailingListUrl", "", EditMode.Url)},
                {"bugTrackerUrl",new KnwonMetaData("bugTrackerUrl", "", EditMode.Url)},
                {"tags",new KnwonMetaData("tags", "", EditMode.SpaceSeparatedStrings)},
                {"summery",new KnwonMetaData("summery", "", EditMode.MultiLineString)},
                {"description",new KnwonMetaData("description", "", EditMode.Markdown)},
                {"releaseNotes",new KnwonMetaData("releaseNotes", "", EditMode.Markdown)},
            };
        }
        

        public MetaData()
        {
            EditMode = EditMode.MultiLineString;
            Description = "";
        }

        public MetaData( string name , string value )
        {
            Name = name;
            Value = value;
        }

        private bool _nameReadOnly = false;

        /// <summary>
        /// メタデータがnuspec内に存在するかを示します
        /// </summary>
        public virtual bool IsExist { get; set; }
        /// <summary>
        /// メタデータのキー名
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// メタデータの中身
        /// </summary>
        public virtual string Value { get; set; }
        /// <summary>
        /// メタデータの概要
        /// </summary>
        public virtual string Description { get;}

        /// <summary>
        /// メタデータがChocolateyで一般に使われているかを示します。
        /// </summary>
        public virtual bool IsKwnown => Description != "";

        public virtual EditMode EditMode { get; }
    }

    public class KnwonMetaData
    {
        public KnwonMetaData( string name , string descroption, EditMode editMode)
        {
            Name = name;
            Description = descroption;
            EditMode = editMode;
        }
        public string Name { get; }
        public string Description { get;  }
        public EditMode EditMode { get; }

        public MetaData CreateMetaData()
        {
            
        }
    }

    public enum EditMode
    {
        /// <summary>
        /// 複数行入力
        /// </summary>
        MultiLineString,
        /// <summary>
        /// 一行入力
        /// </summary>
        SingleLineString,
        /// <summary>
        /// Markdown記述可能
        /// </summary>
        Markdown,
        /// <summary>
        /// スペース区切りの配列
        /// </summary>
        SpaceSeparatedStrings,
        /// <summary>
        /// コンマ区切りの配列
        /// </summary>
        CommaSeparatedStrings,
        /// <summary>
        /// Url
        /// </summary>
        Url,
        /// <summary>
        /// パッケージID
        /// </summary>
        PackageId,
        /// <summary>
        /// 真偽値
        /// </summary>
        Boolean,
        /// <summary>
        /// バージョン番号
        /// </summary>
        Version
    }

    public class Package : BindableBase
    {
        public string DirectoryName { get; }
        public IList<MetaData> MetaData { get; }
        public string RequireLicenseAcceptance { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ReleaseNotes { get; set; }
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
            MetaData = reader.GetMetadata().Select(m => new MetaData(m.Key, m.Value)).ToList();
        }
    }
}
