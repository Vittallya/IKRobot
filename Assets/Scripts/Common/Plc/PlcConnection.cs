using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class PlcConnection : IPlcConnection, IDisposable
{

    public abstract void Open(Action<string> messageBus);

    public abstract void SendToPlc(List<float> angles, Action<string> messageBus);

    public abstract void Dispose();

    public static IPlcConnection DefaultConnection => new S7PlcConnection
        ("172.25.25.21", new []
        {
            "DB1.DBD0",
            "DB1.DBD4",
            "DB1.DBD8",
            "DB1.DBD12",
            "DB1.DBD16",
        });

    public static IPlcConnection MockConnection => new MockPlc();
}
