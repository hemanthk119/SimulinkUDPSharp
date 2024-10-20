using SimulinkUDPSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulinkTestProject
{
    public class ActuatorInfomation : SimulinkSignalsBase
    {
        [SimulinkSignal(0, "T1")]
        public double T1 { get; set; }
        [SimulinkSignal(1, "T1")]
        public double T2 { get; set; }
        [SimulinkSignal(2, "T1")]
        public double T3 { get; set; }
        [SimulinkSignal(3, "T1")]
        public double T4 { get; set; }
    }
}
