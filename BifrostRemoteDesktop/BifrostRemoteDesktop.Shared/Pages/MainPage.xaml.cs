using BifrostRemote.Network;
using BifrostRemoteDesktop.BusinessLogic.Models;
using BifrostRemoteDesktop.BusinessLogic.Network;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BifrostRemoteDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private const char START_OF_TEXT_CHR = '\x02';
        private const char END_OF_TEXT_CHR = '\x03';

        private double _mouseX;
        private double _mouseY;
        private bool _mouseRightButton = false;
        private bool _mouseLeftButton = false;
        private string _selectedEndpoint;

        private InputNetworkTransmitter _inputSender;


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

        public ObservableCollection<string> AvailableEndpoints { get; set; }
        public bool IsInputTransmitterConnected { get; set; }

        public MainPage()
        {
            LoadAvailableEndpoints();
            this.InitializeComponent();

            new Thread(StartReceiver).Start();

            _inputSender = new InputNetworkTransmitter();

            if (AvailableEndpoints.Count > 0)
            {
                SelectedEndpoint = AvailableEndpoints[0];
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadAvailableEndpoints()
        {
            // TODO: Load endpoints from database.
            AvailableEndpoints = new ObservableCollection<string>()
            {
                "127.0.0.1"
            };
        }

        public static void StartReceiver()
        {
            var localhost = IPAddress.Parse("127.0.0.1");
            var listener = new TcpListener(localhost, Context.INPUT_TCP_PORT);

            listener.Start();

            byte[] buffer = new byte[256];
            string data = "";

            while (true)
            {
                var receiver = listener.AcceptTcpClient();
                var stream = receiver.GetStream();
                int i;

                while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    data += Encoding.UTF8.GetString(buffer, 0, i);

                    var startCharIndex = data.IndexOf(START_OF_TEXT_CHR) + 1;
                    var endCharIndex = data.IndexOf(END_OF_TEXT_CHR, startCharIndex)-1;

                    //Check data for full package.
                    if (startCharIndex == -1 || endCharIndex == -1)
                    {
                        break;
                    }

                    var package = data.Substring(startCharIndex, endCharIndex - startCharIndex);
                    data = "";
                    var parts = package.Split(';', 3);

                    var objType = Type.GetType(parts[0]);
                    var obj = JsonConvert.DeserializeObject(parts[1], objType);

                    switch (obj)
                    {
                        case PointerMovedEventDetail detail:
                            Debug.WriteLine(string.Format("[{0}]:{1},{2}", parts[2], detail.X, detail.Y));
                            break;
                    }
                }

                receiver.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        volatile int sendPackageCount = 0;
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

            var message = string.Format("{0}{1};{2};{4}{3}",
                START_OF_TEXT_CHR,
                typeof(PointerMovedEventDetail).FullName,
                json,
                END_OF_TEXT_CHR,
                sendPackageCount++);

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
                IsInputTransmitterConnected = _inputSender.Connected;


            }
        }
    }
}