using System;
using System.Reflection;

namespace SimulinkUDPSharp
{
    public abstract class SimulinkSignalsBase
    {
        public void SetPropertyFromSimSignal<T> (SimulinkSignalAttribute simulinkSignal, double value) where T : SimulinkSignalsBase
        {
            Type myType = typeof(T);
            PropertyInfo myPropInfo = myType.GetProperty(simulinkSignal.PropertyName);
            myPropInfo.SetValue(this, value);
        }

        public double GetPropertyFromSimSignal<T>(SimulinkSignalAttribute simulinkSignal) where T : SimulinkSignalsBase
        {
            Type myType = typeof(T);
            PropertyInfo myPropInfo = myType.GetProperty(simulinkSignal.PropertyName);
            return (double)myPropInfo.GetValue(this);
        }
    }
}
