using System;
using System.Collections.Generic;
using System.Text;

namespace SimulinkUDPSharp
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SimulinkSignalAttribute : Attribute
    {
        private string propertyName;
        private uint index;

        public SimulinkSignalAttribute(uint index, string propertyName)
        {
            this.index = index;
            this.propertyName = propertyName;
        }

        public string PropertyName
        {
            get
            {
                return this.propertyName;
            }
        }

        public uint Index
        {
            get
            {
                return this.index;
            }
        }
    }
}
