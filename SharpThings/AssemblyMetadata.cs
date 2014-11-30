using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace SharpThings {
    /// <summary>
    /// Extracts various bits of metadata that can be attached to an assembly.
    /// </summary>
    public class AssemblyMetadata {
        /// <summary>
        /// The company that published the assembly.
        /// </summary>
        public string Company { get; }

        /// <summary>
        /// The build configuration used to produce the assembly, such as debug or release.
        /// </summary>
        public string Configuration { get; }

        /// <summary>
        /// Copyright information.
        /// </summary>
        public string Copyright { get; }

        /// <summary>
        /// The default culture for the assembly.
        /// </summary>
        public CultureInfo Culture { get; }

        /// <summary>
        /// A human-friendly description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The version of the assembly file on disk.
        /// </summary>
        public Version FileVersion { get; }

        /// <summary>
        /// The actual assembly version, used by the CLR loader.
        /// </summary>
        public Version Version { get; }

        /// <summary>
        /// A human friendly version string.
        /// </summary>
        public string ProductVersion { get; }

        /// <summary>
        /// The name of the assembly.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Trademark information.
        /// </summary>
        public string Trademark { get; }

        /// <summary>
        /// The product line of which the assembly is a part.
        /// </summary>
        public string Product { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyMetadata"/> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public AssemblyMetadata (Assembly assembly) {
            var culture = GetAttribute<AssemblyCultureAttribute>(assembly, a => a.Culture);
            var version = GetAttribute<AssemblyVersionAttribute>(assembly, a => a.Version);
            var fileVersion = GetAttribute<AssemblyFileVersionAttribute>(assembly, a => a.Version);

            Company = GetAttribute<AssemblyCompanyAttribute>(assembly, a => a.Company);
            Configuration = GetAttribute<AssemblyConfigurationAttribute>(assembly, a => a.Configuration);
            Copyright = GetAttribute<AssemblyCopyrightAttribute>(assembly, a => a.Copyright);
            Description = GetAttribute<AssemblyDescriptionAttribute>(assembly, a => a.Description);
            Title = GetAttribute<AssemblyTitleAttribute>(assembly, a => a.Title);
            Trademark = GetAttribute<AssemblyTrademarkAttribute>(assembly, a => a.Trademark);
            Product = GetAttribute<AssemblyProductAttribute>(assembly, a => a.Product);
            ProductVersion = GetAttribute<AssemblyInformationalVersionAttribute>(assembly, a => a.InformationalVersion);
            Culture = string.IsNullOrEmpty(culture) ? null : new CultureInfo(culture);
            Version = string.IsNullOrEmpty(version) ? new Version() : new Version(version);
            FileVersion = string.IsNullOrEmpty(fileVersion) ? new Version() : new Version(fileVersion);
        }

        static string GetAttribute<T>(Assembly assembly, Func<T, string> selector) where T : Attribute {
            var attribute = assembly.GetCustomAttributes(typeof(T), false).OfType<T>().FirstOrDefault();
            return attribute == null ? string.Empty : selector(attribute);
        }
    }
}
