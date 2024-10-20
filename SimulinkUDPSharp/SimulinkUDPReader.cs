using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Reflection;

namespace SimulinkUDPSharp
{
    public class UDPReaderPacketReceivedEventArgs<T> : EventArgs where T : SimulinkSignalsBase, new()
    {
        public T Signals { get; set; }
    }

    public class SimulinkUDPReader<T> : SimulinkUDPBase, IDisposable where T : SimulinkSignalsBase, new()
    {
        private int clientPort;
        private int receiveFromPort;
        private UdpClient udpClient;
        private bool isInRecieveLoop;
        private CancellationTokenSource _source = new CancellationTokenSource();
        private IPEndPoint remoteEP;

        public event EventHandler<UDPReaderPacketReceivedEventArgs<T>> UDPReaderPacketReceived;

        public SimulinkUDPReader(int clientPort, int receiveFromPort)
        {
            this.clientPort = clientPort;
            this.receiveFromPort = receiveFromPort;
            udpClient = new UdpClient(clientPort);
            remoteEP = new IPEndPoint(IPAddress.Any, this.receiveFromPort);
            this.isInRecieveLoop = true;
            ProcessGenericType(typeof(T));
            Task.Run(() => ReadPacketsLoop(), _source.Token);
        }

        public void ReadPacketsLoop()
        {
            while (isInRecieveLoop)
            {
                var sensorInformation = new T();
                var data = udpClient.Receive(ref remoteEP);
                if (data.Length != SimulinkSignalAssociations.Count * 8)
                {
                    return;
                }
                for (int i = 0; i < SimulinkSignalAssociations.Count; i++)
                {
                    var span = data.Skip(i * 8).Take(8).ToArray();
                    var value = System.BitConverter.ToDouble(span, 0);
                    var concernedSimSignal = SimulinkSignalAssociations[(uint)i];
                    sensorInformation.SetPropertyFromSimSignal<T>(concernedSimSignal, value);
                }
                if (UDPReaderPacketReceived != null)
                {
                    UDPReaderPacketReceived.Invoke(this, new UDPReaderPacketReceivedEventArgs<T> { Signals = sensorInformation });
                }
            }
        }

        public void Destroy()
        {
            _source.Cancel();
            udpClient.Close();
        }

        public void Dispose()
        {
            Destroy();
            udpClient.Dispose();
        }
    }
}
