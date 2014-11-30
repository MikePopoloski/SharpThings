using System;
using System.Collections.Generic;
using System.Reflection;
using SharpThings.Prelude;

namespace SharpThings {
    /// <summary>
    /// Provides helper methods for parsing and accessing command line arguments.
    /// </summary>
    public static class CommandLine {
        /// <summary>
        /// Parses the process's command line arguments and applies them to a custom type.
        /// </summary>
        /// <typeparam name="T">The type to which to apply arguments.</typeparam>
        /// <returns>The populated options result.</returns>
        public static T Parse<T>() => Parse<T>(Environment.GetCommandLineArgs());

        /// <summary>
        /// Parses the given command line arguments and applies them to a custom type.
        /// </summary>
        /// <typeparam name="T">The type to which to apply arguments.</typeparam>
        /// <param name="arguments">The command line arguments to parse.</param>
        /// <returns>The populated options result.</returns>
        public static T Parse<T>(string[] arguments) {
            assert(arguments);

            var propertyMap = new Dictionary<string, PropertyInfo>();
            foreach (var prop in typeof(T).GetProperties()) {
                var attr = prop.GetCustomAttribute<OptionAttribute>();
                if (attr == null)
                    continue;

                if (prop.SetMethod == null)
                    throw new InvalidOperationException("Option attribute is applied to a property that has no setter.");
                if (prop.PropertyType != typeof(bool))
                    throw new InvalidOperationException("Option attribute can only be applied to boolean properties.");

                propertyMap[attr.Name] = prop;
            }

            var result = Activator.CreateInstance<T>();
            foreach (var arg in arguments) {
                if (!arg.StartsWith("-", StringComparison.Ordinal))
                    continue;

                var prop = propertyMap.Get(arg.Substring(1));
                if (prop != null)
                    prop.SetValue(result, true);
            }

            return result;
        }
    }

    /// <summary>
    /// An attribute used to mark a property as being a command line option.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class OptionAttribute : Attribute {
        /// <summary>
        /// The full name of the option.
        /// </summary>
        public string Name {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionAttribute"/> class.
        /// </summary>
        /// <param name="name">The option name.</param>
        public OptionAttribute (string name) {
            assert(name);

            Name = name;
        }
    }
}
