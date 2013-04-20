using Fabrik.Common;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Fabrik.API.Client.Core
{
    internal class QuerystringCollection : NameValueCollection
    {
        public QuerystringCollection(object parameters)
        {
            Ensure.Argument.NotNull(parameters, "parameters");

            var properties = TypeDescriptor.GetProperties(parameters);
            foreach (PropertyDescriptor descriptor in properties)
            {
                object val = descriptor.GetValue(parameters);

                if (val != null)
                {
                    base.Add(descriptor.Name, val.ToString());
                }
            }
        }

        private string CreateQuerystring()
        {
            var parameters = base.AllKeys.Select(param => string.Concat(param, "=", Uri.EscapeUriString(this[param])));
            return string.Join("&", parameters);
        }

        public override string ToString()
        {
            return CreateQuerystring();
        }
    }
}
