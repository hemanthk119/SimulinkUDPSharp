# SimulinkUDPSharp - Connect Simulink and C#

This library, SimulinkUDPSharp provides easy way to exchange information to and from SimuLink over muxed UDP ports.

## Usage Example

Start by creating a class `SensorInformation.cs` in the project, with the following contents.

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

Details to note are the base class and the attribute. Attribute second parameter name must be same as the double property name, all indexes since 0 covered. This class is what signals we are receiving over the UDP port. This will be auto-populated and available. Number of signals needed vary according to the SimuLink project. 

Add a class `ActuatorInformation.cs` with the following contents

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
This class should be created populated by code, so that other library classes and transmit it over UDP. 

The entry point to you program could contain the below lines of code

    SimulinkUDPReader<SensorInformation> sensorUdpReader = new SimulinkUDPReader<SensorInformation>(25000, 12500);
    SimulinkUDPWriter<ActuatorInfomation> simulinkUDPWriter = new SimulinkUDPWriter<ActuatorInfomation>(25001, 25002);
    
    sensorUdpReader.UDPReaderPacketReceived += (o, e) =>
    {
    	var sensor1Val = e.Signals.Sensor1;
    	
        // sensors read furthur processing
        // like: Control systems, Complex DSP, etc
        // Place to decide on actuator information
        var applied = 0.3;
        //
    
        simulinkUDPWriter.SendPacket(new ActuatorInfomation { T1 = applied, T2 = 0.3, T3 = 0.3, T4 = 0.3 });      
    };
So, according the code block above, everytime a UDP signals packet is sent and processed as containing one SensorInformation object, an actuator information is sent out after. Do not forget disposing the two objects, as they use UDP ports.

## SimuLink Side

The input to this library are SimuLink UDP Read and UDP Send blocks, connected to demux and mux blocks of approriate inputs and outputs, reading any double vector signal. The ports on UDP blocks have to be configured properly.