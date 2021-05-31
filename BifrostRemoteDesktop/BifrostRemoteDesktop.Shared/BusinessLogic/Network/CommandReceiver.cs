using BifrostRemote.Network;
using BifrostRemoteDesktop.BusinessLogic.Factories;
using BifrostRemoteDesktop.BusinessLogic.Models;
using BifrostRemoteDesktop.BusinessLogic.Models.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static BifrostRemoteDesktop.BusinessLogic.Factories.CommandFactory;

namespace BifrostRemoteDesktop.BusinessLogic.Network
{
    public class CommandReceiver
    {
        private readonly ISystemController _systemController;

        public CommandReceiver(ISystemController systemController)
        {
            _systemController = systemController;
        }

        public static void StartReceiver()
        {
            IPAddress localhost = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(localhost, Context.INPUT_TCP_PORT);

            listener.Start();

            byte[] buffer = new byte[Context.RECEIVER_BUFFER_SIZE];
            string data = "";

            while (true)
            {
                TcpClient receiver = listener.AcceptTcpClient();
                NetworkStream stream = receiver.GetStream();
                int i;

                while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    data += Encoding.UTF8.GetString(buffer, 0, i);

                    int startCharIndex = data.IndexOf(Context.START_OF_TEXT_CHR) + 1;
                    int endCharIndex = data.IndexOf(Context.END_OF_TEXT_CHR, startCharIndex) - 1;

                    //Check data for full package.
                    if (startCharIndex == -1 || endCharIndex == -1)
                    {
                        break;
                    }

                    int packageSize = endCharIndex - startCharIndex;
                    string package = data.Substring(startCharIndex, packageSize);
                    data = data.Remove(startCharIndex, packageSize);
                    string[] parts = package.Split(';', 2);

                    string[] commandArgs = new string[parts.Length - 1];
                    Array.Copy(parts, 1, commandArgs, 0, commandArgs.Length);

                    CommandType commandType = (CommandType)(int.Parse(parts[0]));

                    CommandFactory.CreateCommand(commandType, commandArgs, _systemController);

                    switch (parts[0])
                    {

                    }
                }

                receiver.Close();
            }

        }


    }
}