using Assets.Scripts.Common.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPlcConnection : IDisposable
{
    bool Open(Action<string> messageBus);
    Task<bool> OpenAsync(Action<string> messageBus);
    void SendToPlc(IEnumerable<object> values, Action<string> messageBus, params IValueConverter[] converters);

}
