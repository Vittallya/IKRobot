using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Common.Components
{
    public enum ConnectionStatus
    {
        Connected, Disconnected, ConnectionError
    }

    public class ConnectionComponent : MonoBehaviour
    {
        public UnityEvent<PlcConnection> OnConnected;
        public UnityEvent OnConnecting;

        public UnityEvent OnDisconnected;

        public UnityEvent<string> OnConnectionFailed;

        public PlcConnection Connection;

        public ConfigurationComponentOld Configuration;

        public ConnectionStatus ConnectionStatus { get; private set; } = ConnectionStatus.Disconnected;

        private void Start()
        {
            OnDisconnected?.Invoke();
        }

        public async void TryConnect()
        {
            if (ConnectionStatus != ConnectionStatus.Connected)
            {

                var ip = Configuration.Configuration.ConnectionConfiguration.IpAddress;
                var adressesInput = Configuration.Configuration.ConnectionConfiguration.VarAdressesInput;
                var adressesOutput = Configuration.Configuration.ConnectionConfiguration.VarAdressesOutput;
                Connection = new S7PlcConnection(ip, adressesOutput, adressesInput);

                OnConnecting?.Invoke();

                var connected = await Connection.OpenAsync(str =>
                {
                    ConnectionStatus = ConnectionStatus.ConnectionError;
                    OnConnectionFailed?.Invoke(str);
                });

                if (connected)
                {
                    ConnectionStatus = ConnectionStatus.Connected;
                    OnConnected?.Invoke(Connection);
                }
            }
        }

        public void Disconnect()
        {
            OnDisconnected?.Invoke();
            Connection?.Dispose();
            Connection = null;
            ConnectionStatus = ConnectionStatus.Disconnected;
        }
    }
}
