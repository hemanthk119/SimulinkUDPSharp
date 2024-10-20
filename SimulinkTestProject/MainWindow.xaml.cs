using SimulinkUDPSharp;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimulinkTestProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SimulinkUDPReader<SensorInformation> sensorUdpReader = new SimulinkUDPReader<SensorInformation>(25000, 12500);
            SimulinkUDPWriter<ActuatorInfomation> simulinkUDPWriter = new SimulinkUDPWriter<ActuatorInfomation>(25001, 25002);

            sensorUdpReader.UDPReaderPacketReceived += (o, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    this.sensor1.Text = e.Signals.Sensor1.ToString();
                    this.sensor2.Text = e.Signals.Sensor2.ToString();
                    this.sensor3.Text = e.Signals.Sensor3.ToString();
                    this.sensor4.Text = e.Signals.Sensor4.ToString();
                    this.sensor5.Text = e.Signals.Sensor5.ToString();
                    this.sensor6.Text = e.Signals.Sensor6.ToString();
                });

                // sensors read furthur processing
                // like: Control systems, Complex DSP, etc
                // Place to decide on actuator information
                var error = targetAngle - e.Signals.Sensor1;
                var controlValue = (Kp * error) + (Ki * integralValue) + (Kd * (error - previousError));
                integralValue += error;
                previousError = error;

                var mass = 0.1;
                var torque = mass * (-9.8) * Math.Sin(targetAngle) * 0.5;
                var applied = ((-1) * torque) + controlValue;
                //

                simulinkUDPWriter.SendPacket(new ActuatorInfomation { T1 = applied, T2 = 0.3, T3 = 0.3, T4 = 0.3 });      
            };
        }

        private double previousValue;
        private double previousError;
        private double integralValue;
        private double targetAngle = 0;

        const double Kp = 0.01;
        const double Ki = 0;
        const double Kd = 0.02;

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            targetAngle = e.NewValue;
        }
    }
}