using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;

public class S7PlcConnection : PlcConnection
{

    private readonly Plc plc;
    private readonly List<string> plcAdresses;

    public Plc Plc => plc;

    public S7PlcConnection(string ip, string[] plcAdresses)
    {
        plc = new Plc(CpuType.S71500, ip, 0, 0);
        this.plcAdresses = plcAdresses.ToList();
    }


    public override void Open(Action<string> messageBus)
    {
        try
        {
            plc?.Open();
        }
        catch (Exception e)
        {
            messageBus.Invoke(e.Message);
        }
    }

    public override void SendToPlc(List<float> angles, Action<string> messageBus)
    {

        try
        {
            for (int i = 0; i < angles.Count; i++)
            {
                plc.Write(plcAdresses[i], angles[i]);
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
}