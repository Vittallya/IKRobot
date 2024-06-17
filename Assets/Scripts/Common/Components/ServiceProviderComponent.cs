using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using UnityEngine;

namespace Assets.Scripts.Common.Components
{
    public class ServiceProviderComponent : MonoBehaviour
    {
        public IServiceProvider ServiceProvider { get; private set; }

        private void Awake()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient(sp => new ConfigurationJsonFileService("config.json"));

            ServiceProvider = serviceCollection.BuildServiceProvider();

            //serviceCollection.AddTransient<GrpcChannel>(y =>
            //{
                
            //});
        }
    }
}
