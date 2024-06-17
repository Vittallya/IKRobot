using ClassLibrary1;
using Grpc.Net.Client;
using UnityEngine;
using Cysharp.Net.Http;

public class grpcTest : MonoBehaviour
{
    GrpcChannel channel;
    void Start()
    {

        var handler = new YetAnotherHttpHandler() { Http2Only = true };
        channel = GrpcChannel.ForAddress("http://localhost:5043", new GrpcChannelOptions { HttpHandler = handler, DisposeHttpClient = true });
        var greeter = new Greeter.GreeterClient(channel);
        var result = greeter.SayHello(new HelloRequest { Name = "Unity" });
        Debug.Log(result.Message);
    }

    private void OnDestroy()
    {
        channel.Dispose();
    }
}
