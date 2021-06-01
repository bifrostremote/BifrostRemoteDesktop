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
using System.Threading;
using static BifrostRemoteDesktop.BusinessLogic.Factories.CommandFactory;

namespace BifrostRemoteDesktop.BusinessLogic.Network
{

    public class CommandReceiver
    {
        private ISystemController _systemController;
        private Thread thread;


        public CommandReceiver(ISystemController systemController)
        {
            _systemController = systemController;
        }

        public void Start()
        {
            if (!thread.IsAlive)
            {
                thread = new Thread(Listen);
                thread.Start(this);
            }
        }

        private void Stop()
        {
            if (thread.IsAlive)
            {
                thread.Join();
            }
        }

        private static void Listen(object obj)
        {
            if (obj is CommandReceiver commandReceiver)
            {
                IPAddress localhost = IPAddress.Parse("127.0.0.1");
                TcpListener listener = new TcpListener(localhost, TransmissionContext.INPUT_TCP_PORT);

                listener.Start();

                byte[] buffer = new byte[TransmissionContext.RECEIVER_BUFFER_SIZE];
                string data = "";
                while (true)
                {
                    TcpClient receiver = listener.AcceptTcpClient();
                    NetworkStream stream = receiver.GetStream();
                    int i;

                    while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        data += Encoding.UTF8.GetString(buffer, 0, i);

                        int startCharIndex = data.IndexOf(TransmissionContext.START_OF_TEXT_CHAR) + 1;
                        int endCharIndex = data.IndexOf(TransmissionContext.END_OF_TEXT_CHAR, startCharIndex) - 1;

                        //Check data for full package.
                        if (startCharIndex == -1 || endCharIndex == -1)
                        {
                            break;
                        }

                        int packageSize = endCharIndex - startCharIndex;
                        string package = data.Substring(startCharIndex, packageSize);
                        data = data.Remove(startCharIndex - 1, packageSize + 1);
                        string[] parts = package.Split(TransmissionContext.TEXT_SEGMENTATION_CHAR, 2);

                        string[] commandArgs = new string[parts.Length - 1];
                        Array.Copy(parts, 1, commandArgs, 0, commandArgs.Length);

                        //Parse package to command.
                        int type;
                        if (!int.TryParse(parts[0], out type))
                        {
                            throw new ArgumentException("The received type argument was invalid.");
                        }
                        CommandType commandType = (CommandType) type;
                        ICommand command = CommandFactory.CreateCommand(commandType, commandArgs, commandReceiver._systemController);

                        command.Execute();
                    }

                    receiver.Close();
                }
            }
        }
    }
}