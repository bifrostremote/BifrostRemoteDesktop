using BifrostRemote.Network;
using BifrostRemoteDesktop.BusinessLogic;
using BifrostRemoteDesktop.BusinessLogic.Controls;
using BifrostRemoteDesktop.BusinessLogic.Models;
using BifrostRemoteDesktop.BusinessLogic.Network;
using BifrostRemoteDesktop.BusinessLogic.UIControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
        private string _selectedEndpoint;

        private InputNetworkTransmitter _inputSender;

        public ObservableCollection<string> AvailableEndpoints { get; set; }

        public MainPage()
        {
            LoadAvailableEndpoints();
            this.InitializeComponent();

            new Thread(StartReceiver).Start();

            Thread.Sleep(3000);

            _inputSender = new InputNetworkTransmitter();


        }


        public string SelectedEndpoint
        {
            get => _selectedEndpoint;
            set
            {
                _selectedEndpoint = value;
                NotifyPropertyChanged(nameof(SelectedEndpoint));
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


        private void LoadAvailableEndpoints()
        {
            // TODO: Load endpoints from database.
            AvailableEndpoints = new ObservableCollection<string>()
            {
                "127.0.0.1"
            };
        }

        const char START_OF_TEXT_CHR = '\x02';
        const char END_OF_TEXT_CHR = '\x03';

        public static void StartReceiver()
        {
            var localhost = IPAddress.Parse("127.0.0.1");
            var listener = new TcpListener(localhost, Context.INPUT_TCP_PORT);

            listener.Start();

            byte[] buffer = new byte[256];
            string data;

            while (true)
            {
                var receiver = listener.AcceptTcpClient();
                var stream = receiver.GetStream();
                int i;

                while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    data = Encoding.UTF8.GetString(buffer, 0, i);

                    var startCharIndex = data.IndexOf(START_OF_TEXT_CHR);
                    var endCharIndex = data.IndexOf(END_OF_TEXT_CHR, startCharIndex);

                    if (startCharIndex == -1 || endCharIndex == -1)
                    {
                        break;
                    }

                    // TODO: FIX ME

                    var parts = data.Split(';', 2);
                    var type = Type.GetType(parts[0]);
                    var obj = JsonConvert.DeserializeObject(parts[1], type);

                    switch (obj)
                    {
                        case PointerMovedEventDetail detail:
                            Debug.WriteLine(detail.X + "," + detail.Y);
                            break;
                    }

                }

                receiver.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        int cnt = 0;

        private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint((UIElement)sender);
            MouseX = point.Position.X;
            MouseY = point.Position.Y;

            var json = JsonConvert.SerializeObject(new PointerMovedEventDetail()
            {
                X = MouseX,
                Y = MouseY
            });

            var message = string.Format("{0};{1};{2},{3}",
                START_OF_TEXT_CHR,
                typeof(PointerMovedEventDetail).FullName,
                json,
                END_CHR);

            _inputSender.Send(message);


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

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEndpoint != null)
            {
                _inputSender.Connect(SelectedEndpoint);
            }
        }

    }
}
