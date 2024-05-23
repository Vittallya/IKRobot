using Assets.Scripts.Common.Core.Interfaces;

public class PlcAngleConverter : IValueConverter
{
    public object Convert(object value)
    {
        if(value is float fValue)
            return -fValue;

        return value;
    }
}
