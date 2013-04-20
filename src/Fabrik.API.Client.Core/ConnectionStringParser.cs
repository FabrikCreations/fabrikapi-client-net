using Fabrik.Common;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Fabrik.API.Client.Core
{
    /// <summary>
    /// Utility class for parsing API connection strings.
    /// </summary>
    internal static class ConnectionStringParser
    {
        /// <summary>
        /// Parses the <paramref name="connectionString"/> and attempts to construct a <typeparamref name="TConnection"/> instance.
        /// </summary>
        /// <typeparam name="TConnection">The connection configuration type.</typeparam>
        /// <param name="connectionString">The connection string to parse.</param>
        internal static TConnection Parse<TConnection>(string connectionString) where TConnection : new()
        {
            Ensure.Argument.NotNullOrEmpty(connectionString, "connectionString");

            var connectionConfig = new TConnection();

            var pairs = from property in typeof(TConnection).GetProperties()
                        let match = Regex.Match(connectionString, "[^\\w]*{0}=(?<{0}>[^;]+)".FormatWith(property.Name), RegexOptions.IgnoreCase)
                        where match.Success
                        select new
                        {
                            Property = property,
                            match.Groups[property.Name].Value
                        };

            foreach (var pair in pairs)
            {
                pair.Property.SetValue(connectionConfig, TypeDescriptor.GetConverter(pair.Property.PropertyType).ConvertFromString(pair.Value));
            }

            return connectionConfig;
        }
    }
}
