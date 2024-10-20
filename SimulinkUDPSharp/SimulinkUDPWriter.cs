using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace SimulinkUDPSharp
{
    public class SimulinkUDPWriter<T> : SimulinkUDPBase, IDisposable where T : SimulinkSignalsBase, new ()
    {
        private int clientPort;
        private int sendToPort;
        private UdpClient udpClient;
        private IPEndPoint remoteEP;

        public SimulinkUDPWriter(int clientPort, int sendToPort)
        {
            this.clientPort = clientPort;
            this.sendToPort = sendToPort;
            udpClient = new UdpClient(clientPort);
            remoteEP = new IPEndPoint(IPAddress.Broadcast, this.sendToPort);
            ProcessGenericType(typeof(T));
        }

        public void SendPacket(T sendInformation)
        {
            var bytes = new List<byte>();
            for (int i = 0; i < SimulinkSignalAssociations.Count; i++)
            {
                var simSignal = SimulinkSignalAssociations[(uint)i];
                var value = sendInformation.GetPropertyFromSimSignal<T>(simSignal);
                var valueBytes = System.BitConverter.GetBytes(value);
                bytes.AddRange(valueBytes);
            }
            udpClient.Send(bytes.ToArray(), bytes.Count, remoteEP);
        }

        public void Dispose()
        {
            udpClient.Close();
            udpClient.Dispose();
        }
    }
}
