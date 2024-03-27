using System;
using System.Collections.Generic;

public class MockPlc : IPlcConnection
{
    public void Dispose()
    {
    }

    public void Open(Action<string> messageBus)
    {
    }

    public void SendToPlc(List<float> angles, Action<string> messageBus)
    {
    }
}