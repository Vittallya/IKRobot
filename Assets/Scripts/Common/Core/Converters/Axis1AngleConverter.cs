using Assets.Scripts.Common.Core.Interfaces;

public class Axis1AngleConverter : IValueConverter
{
    public object Convert(object value)
    {
        if (value is float fValue)
        {
            if (fValue < 0)
                fValue += 360;

            return fValue;
        }
        return value;
    }
}