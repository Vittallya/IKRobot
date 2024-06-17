using Assets.Scripts.Common.Models;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assets.Scripts.Common.Components
{
    public class ConfigurationComponentOld : MonoBehaviour
    {
        public string ConfigFileName = "config.json";

        public GenericConfiguration Configuration;

        private void Awake()
        {
#if UNITY_EDITOR
            Configuration = GetDefaultConfig();
#else

            if (File.Exists(ConfigFileName))
            {
                Configuration = DeserializeFile<GenericConfiguration>(ConfigFileName);
            }
            else
            {
                Configuration = GetDefaultConfig();
                Save();
            }
#endif
        }

        public void Save()
        {
            SerializeFile(Configuration, ConfigFileName);
        }

        public static T DeserializeFile<T>(string path)
        {

            //todo добавить это все в configuration service
            using var stream = File.OpenRead(path);
            using var streamReader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(streamReader);
            return JsonSerializer.Create().Deserialize<T>(jsonReader);
        }

        public static void SerializeFile(object data, string path)
        {
            //todo добавить это все в configuration service
            using var stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            using var streamWriter = new StreamWriter(stream);
            using var jsonWriter = new JsonTextWriter(streamWriter);
            JsonSerializer.Create().Serialize(jsonWriter, data);
        }

        private GenericConfiguration GetDefaultConfig() =>
            new()
            {
                ConnectionConfiguration = new()
                {
                    CpuType = "S71500",
                    IpAddress = "172.25.25.21",
                    Rack = 0,
                    Slot = 0,
                    VarAdressesOutput = new List<string>
                    {
                        "DB1.DBD0",
                        "DB1.DBD4",
                        "DB1.DBD8",
                        "DB1.DBD12",
                        "DB1.DBD16",
                    },
                    VarAdressesInput = new List<string>
                    {
                        "DB1.DBD20",
                        "DB1.DBD24",
                        "DB1.DBD28",
                        "DB1.DBD32",
                        "DB1.DBD36",
                    }
                },
                RobotConfiguration = new()
                {
                    RobotUnions = new List<RobotUnion>
                    {
                        new() {
                            D = 1.5f,
                            Alpha = Mathf.PI / 2,
                            LimitMinDeg = -90,
                            LimitMaxDeg = 90
                        },
                        new()
                        {
                            A = 3.5f,
                            LimitMinDeg = -75,
                            LimitMaxDeg = 5
                        },
                        new()
                        {
                            A = 2.5f,
                            LimitMinDeg = -5,
                            LimitMaxDeg = 100
                        },
                        new()
                        {
                            Alpha = Mathf.PI / 2,
                            TettaOffset = Mathf.PI / 2,
                            LimitMinDeg = -90,
                            LimitMaxDeg = 80
                        },
                        new()
                        {
                            D = 3,
                            LimitMinDeg = -90,
                            LimitMaxDeg = 90
                        },
                    }
                }
            };
    }
}
