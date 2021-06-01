using BifrostRemote.Network;
using BifrostRemoteDesktop.BusinessLogic.Models.Commands;
using BifrostRemoteDesktop.BusinessLogic.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using static BifrostRemoteDesktop.BusinessLogic.Factories.CommandFactory;

namespace BifrostRemoteDesktop.BusinessLogic.Network
{

    public class CommandTransmitter
    {
        private TcpClient _tcp;

        public bool Connected => (_tcp == null) ? false : _tcp.Connected;

        public bool SendCommand(CommandType type, RemoteControlCommandArgs commandArgs)
        {
            string args = string.Join(TransmissionContext.TEXT_SEGMENTATION_CHAR, commandArgs);
            string message = string.Join("", type, TransmissionContext.TEXT_SEGMENTATION_CHAR, args);
            return Send(message);
        }

        private bool Send(string data)
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
            return string.Join("", TransmissionContext.START_OF_TEXT_CHAR, data, TransmissionContext.END_OF_TEXT_CHAR);
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
            _tcp.Connect(remoteHostname, port: TransmissionContext.INPUT_TCP_PORT);

        }

        public void Disconnect()
        {
            _tcp.Close();
        }
    }
}
