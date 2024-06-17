using Assets.Scripts.Common.Core.Interfaces;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class S7PlcConnection : PlcConnection
{

    private readonly Plc plc;
    private readonly List<string> plcAdressesOutput;
    private readonly List<string> plcAdressesInput;

    public Plc Plc => plc;

    public S7PlcConnection(string ip,
                           List<string> plcAdressesOutput,
                           List<string> plcAdressesInput)
    {
        plc = new Plc(CpuType.S71500, ip, 0, 0);
        this.plcAdressesOutput = plcAdressesOutput;
        this.plcAdressesInput = plcAdressesInput;
    }


    public override bool Open(Action<string> messageBus)
    {
        try
        {
            plc?.Open();
            return true;
        }
        catch (Exception e)
        {
            messageBus.Invoke(e.Message);
            return false;
        }
    }

    public override void Dispose()
    {
        plc?.Close();
    }

    public override IReadOnlyCollection<object> GetFromPlc(Action<string> messageBus, IValueConverter converter = null)
    {
        try
        {
            var result = plcAdressesInput.Select(address => {
                var val = plc.Read(address);
                return converter?.Convert(val) ?? val;

                }).ToList();
            return result;
        }
        catch (Exception e)
        {
            messageBus.Invoke(e.Message);
            return new List<object>();
        }
    }

    public override async Task<bool> OpenAsync(Action<string> messageBus)
    {
        try
        {
            await plc?.OpenAsync();
            return true;
        }
        catch (Exception e)
        {
            messageBus.Invoke(e.Message);
            return false;
        }
    }

    public override void SendToPlc(IEnumerable<object> values, Action<string> messageBus, params IValueConverter[] converters)
    {
        try
        {
            plcAdressesOutput
                .Select((address, i) => (address, i))
                .Zip(values, (item1, val) => (item1.address, converters.Length > item1.i ? converters[item1.i].Convert(val) : val))
                .ToList()
                .ForEach(y => 
                {

                    plc.Write(y.address, y.Item2);
                });
        }
        catch (Exception e)
        {
            messageBus.Invoke(e.Message);
        }
    }
}