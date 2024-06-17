using System;

namespace Assets.Scripts.Common.Models
{
    [Serializable]
    public record ConfigurationModel
    {
        public ConnectionConfig ConnectionConfiguraion { get; set; }
        public ApplicationConfig ApplicationConfiguration { get; set; }


        [Serializable]
        public record ConnectionConfig
        {
            public string Host { get; set; }
            public bool IsSecure { get; set; }
            public int Port { get; set; }
        }

        [Serializable]
        public record ApplicationConfig
        {
            public bool ShowAnglesLabels { get; set; }
            public bool ShowTargetLabelPosition { get; set; }
            public bool ShowTargetLabelRotation { get; set; }
        }

        public static ConfigurationModel GetDefault() => new ConfigurationModel
        {
            ConnectionConfiguraion = new ConnectionConfig
            {
                Host = "localhost",
                Port = 5053,
                IsSecure = false
            },
            ApplicationConfiguration = new ApplicationConfig
            {
                ShowAnglesLabels = true,
                ShowTargetLabelPosition = true,
                 ShowTargetLabelRotation = true
            }
        };
    }
}
