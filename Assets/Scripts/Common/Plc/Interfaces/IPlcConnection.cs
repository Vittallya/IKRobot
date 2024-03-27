using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;

public interface IPlcConnection : IDisposable
{
    void Open(Action<string> messageBus);
    void SendToPlc(List<float> angles, Action<string> messageBus);

}
