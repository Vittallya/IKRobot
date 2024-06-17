using Assets.Scripts.Common.Models;
using ClassLibrary1;
using Cysharp.Net.Http;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Common.Components
{
    public class ConfigurationComponent : MonoBehaviour
    {
        public string ConfigFileName = "config.json";

        public ConfigurationModel Configuration;

        public ConfigurationModel newConfig;

        private ConfigurationJsonFileService fileService;

        private void Start()
        {
            var serviceProvider = GameObject
                .Find("serviceProvider")
                .GetComponent<ServiceProviderComponent>()
                .ServiceProvider;
            
            fileService = serviceProvider.GetRequiredService<ConfigurationJsonFileService>();

#if UNITY_EDITOR

            Configuration = ConfigurationModel.GetDefault();

#else
            if (fileService.CreateFileIfNotExists())
            {
                Configuration = ConfigurationModel.GetDefault();
            }
            else
            {
                Configuration = fileService.Load<ConfigurationModel>();
            }
#endif
            newConfig = Configuration with
            {
                ApplicationConfiguration = Configuration.ApplicationConfiguration with { },
                ConnectionConfiguraion = Configuration.ConnectionConfiguraion with { }
            };
        }

        public void SetHost(string host)
        {
            newConfig.ConnectionConfiguraion = newConfig.ConnectionConfiguraion with { Host = host };
        }

        public void SetPort(string portStr)
        {
            if(int.TryParse(portStr, out int port))
            {
                newConfig.ConnectionConfiguraion = newConfig.ConnectionConfiguraion with { Port = port };
            }
        }

        public void SetSecure(bool secure)
        {
            newConfig.ConnectionConfiguraion = newConfig.ConnectionConfiguraion with { IsSecure = secure };
        }

        Coroutine connection;

        public IEnumerator ConnectionCoroutine(GrpcChannel grpcChannel)
        {

            yield return null;
        }

        private void OnDestroy()
        {
            if(connection != null)
            {
                StopCoroutine(connection);
            }
        }

        public async void TestConnection()
        {
            var prefix = newConfig.ConnectionConfiguraion.IsSecure ? "https://" : "http://";

            var str = prefix + newConfig.ConnectionConfiguraion.Host + ":" +
                newConfig.ConnectionConfiguraion.Port;

            using GrpcChannel grpcChannel = GrpcChannel.ForAddress(str, new GrpcChannelOptions
            {
                HttpHandler = new YetAnotherHttpHandler() { Http2Only = true }
            });

            var greeter = new Greeter.GreeterClient(grpcChannel);

            try
            {

                var responce = await greeter.SayHelloAsync(new HelloRequest { });
                TestConnectionOk.Invoke();
            }
            catch (Exception e)
            {
                TestConnectionNotOk.Invoke(e.Message);
            }
            //StartCoroutine(ConnectionCoroutine(grpcChannel));            
        }

        public void Save()
        {
            Configuration = newConfig with
            {
                ApplicationConfiguration = newConfig.ApplicationConfiguration with { },
                ConnectionConfiguraion = newConfig.ConnectionConfiguraion with { }
            };
            fileService.SaveFile(Configuration);
        }

        public UnityEvent TestConnectionOk;
        public UnityEvent<string> TestConnectionNotOk;
    }
}
