using BifrostRemote.Network;
using BifrostRemoteDesktop.BusinessLogic.Enums;
using BifrostRemoteDesktop.BusinessLogic.Models.Commands;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using static BifrostRemoteDesktop.BusinessLogic.Factories.CommandFactory;

namespace BifrostRemoteDesktop.BusinessLogic.Network
{

    public class CommandTransmitter
    {
        private TcpClient _tcp;

        public bool Connected => (_tcp == null) ? false : _tcp.Connected;

        /// <summary>
        /// Connection Failed. No receiver found.
        /// </summary>
        public event EventHandler NoReceiverFound;

        public void OnNoReceiverFound(EventArgs e)
        {
            NoReceiverFound?.Invoke(this, e);
        }

        public bool SendCommand(CommandType type, RemoteControlCommandArgs commandArgs)
        {
            string[] packageParts = new string[] {
                type.ToString(),
                commandArgs.ToString()
            };

            string package = string.Join(TransmissionContext.TEXT_SEGMENTATION_CHAR, packageParts);
            return Send(WrapPackage(package));
        }

        private bool Send(string data)
        {
            if (Connected)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);

                // TODO: Find out which send method is best.
                _tcp.GetStream().Write(bytes, 0, bytes.Length);
                //_tcp.Client.Send(bytes, bytes.Length, SocketFlags.None);

                return true;
            }
            else
            {
                return false;
            }
        }

        private string WrapPackage(string data)
        {
            return TransmissionContext.START_OF_TEXT_CHAR + data + TransmissionContext.END_OF_TEXT_CHAR;
        }

        public void Connect(string remoteHostname)
        {
            if (_tcp == null)
            {
                _tcp = new TcpClient();
            }

            if (Connected)
            {
                if (_tcp.Client.RemoteEndPoint.ToString() != remoteHostname) return;

                _tcp.Close();
            }

            try
            {
                _tcp.Connect(remoteHostname, port: TransmissionContext.INPUT_TCP_PORT);
            }
            catch (SocketException)
            {
                Debug.WriteLine("No receiver available.");
                OnNoReceiverFound(EventArgs.Empty);
            }

        }

        public void Disconnect()
        {
            _tcp.Close();
        }
    }
}
