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

    public override void SendToPlc(List<float> angles, Action<string> messageBus)
    {

        try
        {
            for (int i = 0; i < angles.Count; i++)
            {
                plc.Write(plcAdressesOutput[i], angles[i]);
            }
        }
        catch (Exception e)
        {
            messageBus.Invoke(e.Message);
        }
    }

    public override void Dispose()
    {
        plc?.Close();

    }

    public override IReadOnlyCollection<object> GetFromPlc(Action<string> messageBus)
    {
        try
        {
            var result = plcAdressesInput.Select(address => plc.Read(address)).ToList();
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
}