using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public abstract class PlcConnection : IPlcConnection, IDisposable
{

    public abstract bool Open(Action<string> messageBus);

    public abstract void SendToPlc(IEnumerable<object> values, Action<string> messageBus);
    public abstract IReadOnlyCollection<object> GetFromPlc(Action<string> messageBus);

    public abstract void Dispose();

    public abstract Task<bool> OpenAsync(Action<string> messageBus);

    public static IPlcConnection DefaultConnection => new S7PlcConnection
        ("172.25.25.21", new List<string>
        {
            "DB1.DBD0",
            "DB1.DBD4",
            "DB1.DBD8",
            "DB1.DBD12",
            "DB1.DBD16",
        }, new List<string>
        {
            "DB1.DBD20",
            "DB1.DBD24",
            "DB1.DBD28",
            "DB1.DBD32",
            "DB1.DBD36"
        });

    public static IPlcConnection MockConnection => new MockPlc();
}
