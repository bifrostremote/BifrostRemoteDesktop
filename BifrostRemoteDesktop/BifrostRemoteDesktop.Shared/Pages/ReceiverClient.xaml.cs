using BifrostRemoteDesktop.BusinessLogic.Controllers;
using BifrostRemoteDesktop.BusinessLogic.Network;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BifrostRemoteDesktop.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReceiverClientPage : Page
    {
        private CommandReceiver _commandReceiver;

        public ReceiverClientPage()
        {
            this.InitializeComponent();

            _commandReceiver = new CommandReceiver(
                new SystemControllerProvider().GetPlatformSystemController());
        }

        private void Start_Server_Click(object sender, RoutedEventArgs e)
        {
            _commandReceiver.Start();
        }

        private void Stop_Server_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
