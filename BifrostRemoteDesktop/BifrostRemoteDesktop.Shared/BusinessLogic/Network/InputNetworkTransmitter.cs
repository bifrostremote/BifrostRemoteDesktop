using BifrostRemote.Network;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Network
{
    public enum SendResponseType
    {
        Success, NotConnected
    }

    public class InputNetworkTransmitter
    {
        private TcpClient _tcp;

        public bool Connected => (_tcp == null) ? false : _tcp.Connected;

        public InputNetworkTransmitter()
        {

        }

        public SendResponseType Send(string message)
        {
            if (Connected)
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                _tcp.Client.Send(bytes, bytes.Length, SocketFlags.None);
                return SendResponseType.Success;
            }
            else
            {
                return SendResponseType.NotConnected;
            }
        }

        public void Connect(string hostname)
        {
            if (_tcp == null)
            {
                _tcp = new TcpClient();
            }

            if (Connected)
            {
                if (_tcp.Client.RemoteEndPoint.ToString() != hostname) return;

                _tcp.Close();
            }

            _tcp.Connect(hostname, port: Context.INPUT_TCP_PORT);
        }

        public void Disconnect()
        {
            _tcp.Close();
        }
    }
}
