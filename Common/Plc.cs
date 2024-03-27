using S7.Net;
using System.Collections.Generic;
using Unity.VisualScripting;

class PlcConnection
{
    private readonly Plc plc;

    public PlcConnection(string ip)
    {
        plc = new Plc(CpuType.S71500, ip, 0, 0);
    }

    public void SendToPlc(List<float> angles)
    {
        plc.Write("DB1.DBD0", angles[0]);
        plc.Write("DB1.DBD4", angles[1]);
        plc.Write("DB1.DBD8", angles[2]);
        plc.Write("DB1.DBD12", angles[2]);
        plc.Write("DB1.DBD16", angles[2]);
    }

    public static PlcConnection Default => new PlcConnection("172.25.25.21");
}