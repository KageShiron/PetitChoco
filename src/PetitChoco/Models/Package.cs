using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Security.AccessControl;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet;
using NuGet.Packaging.Core;
using Prism.Mvvm;
using Reactive.Bindings;
using PackageDependency = NuGet.Packaging.Core.PackageDependency;

namespace PetitChoco.Models
{
    public class MetaData : BindableBase
    {
        public static IEnumerable<KnwonMetaData> KnwonMetaData { get; }
        static MetaData()
        {
            KnwonMetaData = new List<KnwonMetaData>()
            {
                new KnwonMetaData("id", "", EditMode.PackageId),
                new KnwonMetaData("version", "", EditMode.Version),
                new KnwonMetaData("packageSourceUrl", "", EditMode.Url),
                new KnwonMetaData("owners", "", EditMode.SingleLineString),

                new KnwonMetaData("title", "", EditMode.SingleLineString),
                new KnwonMetaData("authors", "", EditMode.CommaSeparatedStrings),
                new KnwonMetaData("projectUrl", "", EditMode.Url),
                new KnwonMetaData("iconUrl", "", EditMode.Url),
                new KnwonMetaData("copyright", "", EditMode.SingleLineString),
                new KnwonMetaData("licenseUrl", "", EditMode.Url),
                new KnwonMetaData("requireLicenseAcceptance", "", EditMode.Boolean),
                new KnwonMetaData("projectSourceUrl", "", EditMode.Url),
                new KnwonMetaData("docsUrl", "", EditMode.Url),
                new KnwonMetaData("mailingListUrl", "", EditMode.Url),
                new KnwonMetaData("bugTrackerUrl", "", EditMode.Url),
                new KnwonMetaData("tags", "", EditMode.SpaceSeparatedStrings),
                new KnwonMetaData("summary", "", EditMode.MultiLineString),
                new KnwonMetaData("description", "", EditMode.Markdown),
                new KnwonMetaData("releaseNotes", "", EditMode.Markdown),
            };
        }
        

        public MetaData()
        {
            EditMode = EditMode.MultiLineString;
            Description = "";
        }

        public MetaData( string name , string value , EditMode editMode , string desc )
        {
            Name = name;
            Value = value;
            EditMode = editMode;
            Description = desc;
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

        public MetaData CreateMetaData( string value )
        {
            return new MetaData(Name,value,EditMode,Description);
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

    public class PackageFile
    {
        public PackageFile()
        {
            
        }

        public PackageFile( string src , string trg)
        {
            Source = src;
            Target = trg;
        }
        public string Source { get; set; }
        public string Target { get; set; }
    }

    public class Package : BindableBase
    {
        public string DirectoryName { get; }
        public ObservableCollection<MetaData> MetaData { get; }
        public ObservableCollection<PackageFile> Files { get; }
        public DirectoryInfo DirectoryInfo => new DirectoryInfo(DirectoryName);

        public ObservableCollection<PackageDependency> Dependencies { get; }
        public string NuspecFileName { get; set; }
        /*
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
        public string BugTrackerUrl { get; set; }*/

        public Package()
        {
            DirectoryName = "";
            MetaData = new ObservableCollection<MetaData>();
        }

        public Package( string dirName )
        {
            DirectoryName = dirName;
            try
            {
                var r = new NuGet.Packaging.PackageFolderReader(dirName);
                var reader = r.NuspecReader;
                NuspecFileName = r.GetNuspecFile();

                var ns = reader.Xml.Root.GetDefaultNamespace();
                Files = new ObservableCollection<PackageFile>(
                    reader.Xml.Root.Elements(ns + "files").Elements()
                    .Select(x => new PackageFile(x.Attribute("src").Value, x.Attribute("target").Value))
                );
                var ms = reader.GetMetadata().ToDictionary(x => x.Key, x => x.Value);
                Dependencies = new ObservableCollection<PackageDependency>();
                Dependencies.AddRange(reader.GetDependencyGroups().FirstOrDefault()?.Packages ??
                                      new PackageDependency[0]);
                MetaData = new ObservableCollection<MetaData>(Models.MetaData.KnwonMetaData.Select(m =>
                {
                    string value;
                    ms.TryGetValue(m.Name, out value);
                    return m.CreateMetaData(value);
                }));
            }
            catch (PackagingException e)
            {
                MetaData = Models.MetaData.KnwonMetaData.Select(m => m.CreateMetaData("")).ToObservable().ToReactiveCollection();
            }
            //.Select(m => new MetaData(m.Key, m.Value)).ToList();
        }
    }
}
