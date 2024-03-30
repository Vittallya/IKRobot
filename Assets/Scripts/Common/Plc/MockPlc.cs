using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MockPlc : IPlcConnection
{
    public void Dispose()
    {
    }

    public async Task<bool> OpenAsync(Action<string> messageBus)
    {
        return true;
    }

    public void SendToPlc(List<float> angles, Action<string> messageBus)
    {
    }

    bool IPlcConnection.Open(Action<string> messageBus)
    {
        return true;
    }
}