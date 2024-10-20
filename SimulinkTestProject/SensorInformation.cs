using SimulinkUDPSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulinkTestProject
{
    public class SensorInformation : SimulinkSignalsBase
    {
        [SimulinkSignal(0, "Sensor1")]
        public double Sensor1 { get; set; }

        [SimulinkSignal(1, "Sensor2")]
        public double Sensor2 { get; set; }

        [SimulinkSignal(2, "Sensor3")]
        public double Sensor3 { get; set; }

        [SimulinkSignal(3, "Sensor4")]
        public double Sensor4 { get; set; }

        [SimulinkSignal(4, "Sensor5")]
        public double Sensor5 { get; set; }

        [SimulinkSignal(5, "Sensor6")]
        public double Sensor6 { get; set; }
    }
}
