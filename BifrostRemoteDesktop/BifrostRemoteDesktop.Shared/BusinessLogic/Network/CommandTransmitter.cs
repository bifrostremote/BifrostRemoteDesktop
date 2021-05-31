using BifrostRemote.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Network
{

    public class CommandTransmitter
    {
        private TcpClient _tcp;

        public bool Connected => (_tcp == null) ? false : _tcp.Connected;

        public CommandTransmitter()
        {

        }

        public bool Send(string data)
        {
            if (Connected)
            {
                PackData(data);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                _tcp.GetStream().Write(bytes, 0, bytes.Length);
                //_tcp.Client.Send(bytes, bytes.Length, SocketFlags.None);
                return true;
            }
            else
            {
                return false;
            }
        }

        private string PackData(string data)
        {
            return $"{Context.START_OF_TEXT_CHR}{data}{Context.END_OF_TEXT_CHR}";
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
            _tcp.Connect(remoteHostname, port: Context.INPUT_TCP_PORT);

        }

        public void Disconnect()
        {
            _tcp.Close();
        }
    }
}
