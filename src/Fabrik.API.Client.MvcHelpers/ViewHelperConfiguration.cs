using System;
using System.Configuration;

namespace Fabrik.API.Client.MvcHelpers
{
    public class ViewHelperConfiguration : ConfigurationSection
    {
        private static Lazy<ViewHelperConfiguration> configuration
            = new Lazy<ViewHelperConfiguration>(() =>
            {
                return ConfigurationManager.GetSection("fabrikViewHelper") as ViewHelperConfiguration ?? new ViewHelperConfiguration();
            });
        
        public static ViewHelperConfiguration Configuration
        {
            get
            {
                return configuration.Value;
            }
        }

        private const string StorageServerSettingKey = "storageServerUri";
        [ConfigurationProperty(StorageServerSettingKey, IsRequired = false)]
        public string StorageServerUri
        {
            get { return this[StorageServerSettingKey] as string; }
            set { this[StorageServerSettingKey] = value; }
        }
        
        private const string MediaServerSettingKey = "mediaServerUri";
        [ConfigurationProperty(MediaServerSettingKey, IsRequired = false)]
        public string MediaServerUri
        {
            get { return this[MediaServerSettingKey] as string; }
            set { this[MediaServerSettingKey] = value; }
        }

        private const string UseMediaServerSettingKey = "useMediaServer";
        [ConfigurationProperty(UseMediaServerSettingKey, IsRequired = false, DefaultValue = false)]
        public bool UseMediaServer
        {
            get { return (bool)this[UseMediaServerSettingKey]; }
            set { this[UseMediaServerSettingKey] = value; }
        }

        private const string FallbackImageUriKey = "fallbackImageUri";
        [ConfigurationProperty(FallbackImageUriKey, IsRequired = false)]
        public string FallbackImageUri
        {
            get { return this[FallbackImageUriKey] as string; }
            set { this[FallbackImageUriKey] = value; }
        }
    }
}
