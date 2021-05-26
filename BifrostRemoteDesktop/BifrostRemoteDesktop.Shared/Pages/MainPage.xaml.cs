using BifrostRemoteDesktop.BusinessLogic;
using BifrostRemoteDesktop.BusinessLogic.Controls;
using BifrostRemoteDesktop.BusinessLogic.UIControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BifrostRemoteDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private double _mouseX;
        private double _mouseY;
        private bool _mouseRightButton = false;
        private bool _mouseLeftButton = false;

        private TcpClient tcp = new TcpClient();
        public MainPage()
        {
            this.InitializeComponent();


            new Thread(RunServer).Start();
            Thread.Sleep(3000);
            tcp.Connect("127.0.0.1", 25565);
        }

        public static void RunServer()
        {
            var listener = new TcpListener(25565);
            listener.Start();
            byte[] buffer = new byte[256];
            string data = "";
            while (true)
            {
                var server = listener.AcceptTcpClient();
                var stream = server.GetStream();
                int i;

                while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = Encoding.ASCII.GetString(buffer, 0, i);
                    Debug.WriteLine("Received: {0}", data);

                    // Process the data sent by the client.
                    data = data.ToUpper();

                    byte[] msg = Encoding.ASCII.GetBytes(data);
                }

                server.Close();
            }
        }

        public bool MouseRightButton
        {
            get => _mouseRightButton;
            set
            {
                _mouseRightButton = value;
                NotifyPropertyChanged(nameof(MouseRightButton));
            }
        }
        public bool MouseLeftButton
        {
            get => _mouseLeftButton;
            set
            {
                _mouseLeftButton = value;
                NotifyPropertyChanged(nameof(MouseLeftButton));
            }
        }

        public double MouseX
        {
            get => _mouseX;
            set
            {
                _mouseX = value;
                NotifyPropertyChanged(nameof(MouseX));
            }
        }
        public double MouseY
        {
            get => _mouseY;
            set
            {
                _mouseY = value;
                NotifyPropertyChanged(nameof(MouseY));
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class PointerMovedEvent
        {
            public double X { get; set; }
            public double Y { get; set; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint((UIElement)sender);
            MouseX = point.Position.X;
            MouseY = point.Position.Y;

            JsonConvert.SerializeObject(new PointerMovedEvent()
            {
                X = MouseX,
                Y = MouseY
            });

            //TODO: SEND TYPE AND OBJECT
            //var a = Type.GetType();
            tcp.Client.Send(Encoding.ASCII.GetBytes(String.Format("{0},{1}", MouseX.ToString(), MouseY.ToString())));
        }

        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint((UIElement)sender);
            MouseRightButton = point.Properties.IsRightButtonPressed;
            MouseLeftButton = point.Properties.IsLeftButtonPressed;

        }

        private void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint((UIElement)sender);
            MouseRightButton = point.Properties.IsRightButtonPressed;
            MouseLeftButton = point.Properties.IsLeftButtonPressed;

        }



    }

    public class InputSender
    {

    }

    public class InputReceiver
    {

    }

}
