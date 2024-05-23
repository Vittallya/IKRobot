using System;
using System.Collections.Generic;
using S7.Net;

namespace Assets.Scripts.Common.Core.Extensions
{
    public static class ConnectionExtensions
    {
        public static IReadOnlyCollection<float> ReadAngles(this S7PlcConnection connection, int count = 5)
        {
            var plc = connection.Plc;
            var valueList = new List<float>(count);
            var bytesAddress = 20;

            for (int i = 1; i <= count; i++)
            {
                var val = plc.Read(DataType.DataBlock, 1, bytesAddress, VarType.LReal, 1);
                valueList.Add(Convert.ToSingle(val));
            }
            return valueList;
        }
    }
}
