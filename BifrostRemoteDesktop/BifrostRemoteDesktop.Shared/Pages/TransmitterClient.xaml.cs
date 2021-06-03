using BifrostRemote.Network;
using BifrostRemoteDesktop.BusinessLogic.Controllers;
using BifrostRemoteDesktop.BusinessLogic.Enums;
using BifrostRemoteDesktop.BusinessLogic.Models;
using BifrostRemoteDesktop.BusinessLogic.Models.Commands;
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
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using static BifrostRemoteDesktop.BusinessLogic.Factories.CommandFactory;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BifrostRemoteDesktop.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransmitterClientPage : Page, INotifyPropertyChanged
    {

        private double _mouseX;
        private double _mouseY;
        private bool _mouseRightButtonPressed = false;
        private bool _mouseLeftButtonPressed = false;
        private bool _mouseMiddleButtonPressed;

        private string _selectedEndpoint;

        private CommandTransmitter _commandTransmitter;
        private string _connectionStatus;

        public string SelectedEndpoint
        {
            get => _selectedEndpoint;
            set
            {
                _selectedEndpoint = value;
                NotifyPropertyChanged(nameof(SelectedEndpoint));
            }
        }

        public bool MouseRightButtonPressed
        {
            get => _mouseRightButtonPressed;
            set
            {
                _mouseRightButtonPressed = value;
                NotifyPropertyChanged(nameof(MouseRightButtonPressed));
            }
        }
        public bool MouseLeftButtonPressed
        {
            get => _mouseLeftButtonPressed;
            set
            {
                _mouseLeftButtonPressed = value;
                NotifyPropertyChanged(nameof(MouseLeftButtonPressed));
            }
        }

        public bool MouseMiddleButtonPressed
        {
            get => _mouseMiddleButtonPressed;
            set
            {
                _mouseMiddleButtonPressed = value;
                NotifyPropertyChanged(nameof(MouseMiddleButtonPressed));
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

        public string ConnectionStatus
        {
            get => _connectionStatus; set
            {
                _connectionStatus = value;
                NotifyPropertyChanged(nameof(ConnectionStatus));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TransmitterClientPage()
        {
            LoadAvailableEndpoints();
            this.InitializeComponent();

            _commandTransmitter = new CommandTransmitter();

            if (AvailableEndpoints.Count > 0)
            {
                SelectedEndpoint = AvailableEndpoints[0];
            }

            BindEvents();
            ConnectionStatus = "Not initialized.";

            //DEBUG
            new CommandReceiver(
                  new SystemControllerProvider()
                  .GetPlatformSystemController())
                .Start(); ;

        }

        private void BindEvents()
        {
            _commandTransmitter.NoReceiverFound += (s, e) =>
            {
                ConnectionStatus = "No receiver found!";
            };

            _commandTransmitter.ConnectionEstablished += (s, e) =>
            {
                ConnectionStatus = "Connection Established!";
            };
        }

        private void _commandTransmitter_NoReceiverFound(object sender, EventArgs e)
        {
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

        private void UpdateViewPointerDetails(PointerPoint point)
        {
            MouseRightButtonPressed = point.Properties.IsRightButtonPressed;
            MouseLeftButtonPressed = point.Properties.IsLeftButtonPressed;
            MouseMiddleButtonPressed = point.Properties.IsMiddleButtonPressed;
        }

        private void SendMovePointerCommand(MovePointerCommandArgs args)
        {
            _commandTransmitter.SendCommand(CommandType.MovePointer, args);
        }

        private void SendUpdatePointerStateCommand(PointerUpdateStateCommandArgs args)
        {
            _commandTransmitter.SendCommand(CommandType.UpdatePointerState, args);
        }

        private void SendUpdatePointerStateCommand(PointerPoint point)
        {
            SendUpdatePointerStateCommand(new PointerUpdateStateCommandArgs()
            {
                IsLeftPointerButtonPressed = point.Properties.IsLeftButtonPressed,
                IsRightPointerButtonPressed = point.Properties.IsRightButtonPressed,
                IsMiddlePointerButtonPressed = point.Properties.IsMiddleButtonPressed
            });
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEndpoint != null)
            {
                _commandTransmitter.Connect(SelectedEndpoint);
            }
        }

        private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint point = e.GetCurrentPoint((UIElement)sender);
            MouseX = point.Position.X;
            MouseY = point.Position.Y;

            SendMovePointerCommand(new MovePointerCommandArgs() { TargetX = MouseX, TargetY = MouseY });
        }

        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint point = e.GetCurrentPoint((UIElement)sender);
            UpdateViewPointerDetails(point);
            SendUpdatePointerStateCommand(point);
        }

        private void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint point = e.GetCurrentPoint((UIElement)sender);
            UpdateViewPointerDetails(point);
            SendUpdatePointerStateCommand(point);
        }
    }

}