using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimulinkUDPSharp
{
    public abstract class SimulinkUDPBase
    {
        public Dictionary<uint, SimulinkSignalAttribute> SimulinkSignalAssociations { get; set; } = new Dictionary<uint, SimulinkSignalAttribute>();

        public void ProcessGenericType(Type type)
        {
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    SimulinkSignalAttribute simSigAttr = attr as SimulinkSignalAttribute;
                    if (simSigAttr == null)
                    {
                        continue;
                    }
                    SimulinkSignalAssociations.Add(simSigAttr.Index, simSigAttr);
                }
            }
        }
    }
}
