using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lidgren.Network;

namespace The_Guild_2_Network_Helper
{
    class ClientNetworking
    {
        public delegate void StartStopButtonDelegate(object sender, FormClosedEventArgs e);
        StartStopButtonDelegate stopClientInvoke;
        public delegate void UpdateClientInfoDelegate(string info, bool clear);
        UpdateClientInfoDelegate updateClientInfoInvoke;
        public delegate void ActivateClientFormDelegate();
        //ActivateClientFormDelegate activateClientFormInvoke;
        public delegate void FileTransferBarDelegate(string totalSize, string amountDownloaded, bool reset);
        FileTransferBarDelegate fileTransferBarInvoke;

        public static NetClient client;
        NetPeerConfiguration config;

        public static bool connected;
        public static bool fileTransferStarted;

        public bool transferConnected;
        public bool transferStarted;
        public bool transferVerified;
        public bool allDone;

        public string fileName;
        public int savefileSize;
        public int kilobytesDownloaded;

        Thread watchDownloadThread;

        NetIncomingMessage incomingMessage;
        public string incomingMessageData;
        NetOutgoingMessage outgoingMessage;

        AutoResetEvent incomingMessageEvent;

        NetConnectionStatus hostStatus;

        bool connectTimerTick;


        public ClientNetworking()
        {
            config = new NetPeerConfiguration("MyNetwork");
            config.Port = clientForm.clientPort;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.EnableMessageType(NetIncomingMessageType.Data);
            config.EnableMessageType(NetIncomingMessageType.UnconnectedData);

            config.DisableMessageType(NetIncomingMessageType.DebugMessage);

            config.ConnectionTimeout = 9999999;

            client = new NetClient(config);
            client.Start();

            stopClientInvoke = ChoiceMessageBox.clientForm.clientStartStopButton_Click;
            //activateClientFormInvoke = ChoiceMessageBox.clientForm.ActivateForm;
            updateClientInfoInvoke = ChoiceMessageBox.clientForm.UpdateClientInfoBox;
            fileTransferBarInvoke = ChoiceMessageBox.clientForm.UpdateFileTransferBar;

            watchDownloadThread = new Thread(WatchDownload);
            watchDownloadThread.Start();

            Initialize();
        }


        public void Initialize()
        {
            try
            {
                RetryDiscovery:
                while (connected == false & clientForm.formClosing == false & clientForm.clientStart == true)
                {
                    Thread.Sleep(1);

                    bool messageReceived;

                    client.DiscoverKnownPeer(clientForm.hostIP, clientForm.hostPort);

                    Debug.WriteLine("Discovering");

                    incomingMessageEvent = client.MessageReceivedEvent;
                    messageReceived = incomingMessageEvent.WaitOne(5000);

                    if (messageReceived == false)
                    {
                        incomingMessageEvent.Reset();
                        goto RetryDiscovery;
                    }
                    else
                    {
                        incomingMessageEvent.Reset();
                        RetryConnect:
                        while (connected == false & clientForm.formClosing == false & clientForm.clientStart == true)
                        {
                            Thread.Sleep(1);
                            incomingMessage = client.ReadMessage();
                            while (client.ConnectionsCount == 0 & connected == false & clientForm.formClosing == false & clientForm.clientStart == true)
                            {
                                Thread.Sleep(1);

                                if (incomingMessage != null)
                                {
                                    switch (incomingMessage.MessageType)
                                    {
                                        case NetIncomingMessageType.StatusChanged:
                                            hostStatus = (NetConnectionStatus)incomingMessage.ReadByte();

                                            ChoiceMessageBox.clientForm.clientInfoBox.Invoke(updateClientInfoInvoke, hostStatus + "\n" + incomingMessage.ReadString(), false);

                                            break;
                                    }
                                }

                                outgoingMessage = client.CreateMessage(clientForm.clientUsername);

                                client.Connect(incomingMessage.SenderEndPoint, outgoingMessage);

                                System.Windows.Forms.Timer connectTimer = new System.Windows.Forms.Timer();
                                connectTimer.Interval = 5000;
                                connectTimer.Tick += ConnectTimer_Tick;
                                connectTimer.Start();

                                while (connectTimerTick == false & client.ConnectionsCount == 0 & connected == false & clientForm.formClosing == false & clientForm.clientStart == true)
                                {
                                    Thread.Sleep(1);
                                    incomingMessage = client.ReadMessage();
                                    if (incomingMessage != null)
                                    {
                                        switch (incomingMessage.MessageType)
                                        {
                                            case NetIncomingMessageType.StatusChanged:
                                                hostStatus = (NetConnectionStatus)incomingMessage.ReadByte();
                                                if (hostStatus == NetConnectionStatus.Disconnected)
                                                {
                                                    ChoiceMessageBox.clientForm.clientStartStopButton.Invoke(stopClientInvoke, this, null);
                                                }

                                                ChoiceMessageBox.clientForm.clientInfoBox.Invoke(updateClientInfoInvoke, incomingMessage.ReadString() + "\n" + hostStatus, false);

                                                break;

                                            case NetIncomingMessageType.UnconnectedData:
                                                string unconnectedData = incomingMessage.ReadString();
                                                if (unconnectedData == "Username Already Used!" | unconnectedData == "Connection Denied")
                                                {
                                                    ChoiceMessageBox.clientForm.clientStartStopButton.Invoke(stopClientInvoke, this, null);
                                                }

                                                ChoiceMessageBox.clientForm.clientInfoBox.Invoke(updateClientInfoInvoke, unconnectedData, false);
                                                break;
                                        }
                                    }
                                }

                            }

                            connectTimerTick = false;

                            if (client.ConnectionsCount == 1)
                            {
                                connected = true;
                                ReceiveMessages();
                            }
                            else
                            {
                                goto RetryConnect;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }


        public void ReceiveMessages()
        {
            while (clientForm.formClosing == false & clientForm.clientStart == true & connected == true)
            {
                Thread.Sleep(1);
                incomingMessage = client.ReadMessage();

                if (incomingMessage != null)
                {
                    switch (incomingMessage.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            string dataMessage = "";
                            try
                            {
                                dataMessage = incomingMessage.ReadString();
                            }
                            catch(Exception e)
                            {
                            }
                            if (dataMessage == "Out of Sync")
                            {
                                int dataMessageType;
                                transferConnected = false;
                                transferStarted = false;
                                transferVerified = false;
                                allDone = false;

                                //ChoiceMessageBox.clientForm.Invoke(activateClientFormInvoke);

                                ChoiceMessageBox.clientForm.clientInfoBox.Invoke(updateClientInfoInvoke, "Game out of sync!\nStarting savefile transfer", false);

                                outgoingMessage = client.CreateMessage();
                                byte aByte1 = 0;
                                outgoingMessage.Write(aByte1);
                                client.SendMessage(outgoingMessage, NetDeliveryMethod.ReliableOrdered, 10);

                                transferConnected = true;

                                ReReadData1:

                                incomingMessage = client.ReadMessage();

                                if (incomingMessage != null)
                                {
                                    switch (incomingMessage.MessageType)
                                    {
                                        case NetIncomingMessageType.StatusChanged:
                                            hostStatus = (NetConnectionStatus)incomingMessage.ReadByte();
                                            if (hostStatus == NetConnectionStatus.Disconnected & clientForm.clientStart == true)
                                            {
                                                ChoiceMessageBox.clientForm.clientStartStopButton.Invoke(stopClientInvoke, this, null);
                                            }

                                            ChoiceMessageBox.clientForm.clientInfoBox.Invoke(updateClientInfoInvoke, hostStatus + "\n" + incomingMessage.ReadString(), false);
                                            break;
                                    }

                                    dataMessageType = incomingMessage.ReadByte();

                                    if (dataMessageType == 0)
                                    {
                                        fileName = incomingMessage.ReadString();

                                        ChoiceMessageBox.clientForm.clientInfoBox.Invoke(updateClientInfoInvoke, "Savegame name received: " + fileName, false);

                                        ReReadData2:

                                        incomingMessage = client.ReadMessage();
                                        if (incomingMessage != null)
                                        {
                                            switch (incomingMessage.MessageType)
                                            {
                                                case NetIncomingMessageType.StatusChanged:
                                                    hostStatus = (NetConnectionStatus)incomingMessage.ReadByte();
                                                    if (hostStatus == NetConnectionStatus.Disconnected & clientForm.clientStart == true)
                                                    {
                                                        ChoiceMessageBox.clientForm.clientStartStopButton.Invoke(stopClientInvoke, this, null);
                                                    }

                                                    ChoiceMessageBox.clientForm.clientInfoBox.Invoke(updateClientInfoInvoke, hostStatus + "\n" + incomingMessage.ReadString(), false);
                                                    break;
                                            }

                                            dataMessageType = incomingMessage.ReadByte();
                                            if (dataMessageType == 1)
                                            {
                                                savefileSize = Convert.ToInt32(incomingMessage.ReadString());

                                                ChoiceMessageBox.clientForm.clientInfoBox.Invoke(updateClientInfoInvoke, "Savegame filesize received: " + savefileSize / 1024 + "kb", false);

                                                outgoingMessage = client.CreateMessage();
                                                byte aByte2 = 1;
                                                outgoingMessage.Write(aByte2);
                                                client.SendMessage(outgoingMessage, NetDeliveryMethod.ReliableOrdered, 11);

                                                transferStarted = true;

                                                fileTransferStarted = true;

                                                ReReadData3:

                                                incomingMessage = client.ReadMessage();

                                                if (incomingMessage != null)
                                                {
                                                    switch (incomingMessage.MessageType)
                                                    {
                                                        case NetIncomingMessageType.StatusChanged:
                                                            hostStatus = (NetConnectionStatus)incomingMessage.ReadByte();
                                                            if (hostStatus == NetConnectionStatus.Disconnected & clientForm.clientStart == true)
                                                            {
                                                                ChoiceMessageBox.clientForm.clientStartStopButton.Invoke(stopClientInvoke, this, null);
                                                            }

                                                            ChoiceMessageBox.clientForm.clientInfoBox.Invoke(updateClientInfoInvoke, hostStatus + "\n" + incomingMessage.ReadString(), false);
                                                            break;
                                                            
                                                        case NetIncomingMessageType.Data:
                                                            if (incomingMessage.LengthBits == 72)
                                                            {
                                                                dataMessage = incomingMessage.ReadString();
                                                            }
                                                            if (dataMessage == "Recheck")
                                                            {
                                                                dataMessage = "";
                                                                dataMessageType = incomingMessage.ReadByte();

                                                                switch (dataMessageType)
                                                                {
                                                                    case 0:
                                                                        outgoingMessage = client.CreateMessage();
                                                                        byte aByte4 = 0;
                                                                        outgoingMessage.Write(aByte4);
                                                                        client.SendMessage(outgoingMessage, NetDeliveryMethod.ReliableOrdered, 10);
                                                                        break;

                                                                    case 1:
                                                                        outgoingMessage = client.CreateMessage();
                                                                        byte aByte5 = 1;
                                                                        outgoingMessage.Write(aByte5);
                                                                        client.SendMessage(outgoingMessage, NetDeliveryMethod.ReliableOrdered, 10);
                                                                        break;

                                                                    case 2:
                                                                        goto ReReadData3;
                                                                }
                                                                goto ReReadData3;
                                                            }
                                                        break;
                                                    }

                                                    dataMessageType = incomingMessage.ReadByte();
                                                    if (dataMessageType == 2)
                                                    {
                                                        ChoiceMessageBox.clientForm.clientInfoBox.Invoke(updateClientInfoInvoke, "Savefile transfer complete\nWaiting for others to finish downloading...", false);

                                                        string saveFileAsString = incomingMessage.ReadString();
                                                        File.WriteAllBytes(clientForm.clientsavegameDirectory + @"\" + fileName, Convert.FromBase64String(saveFileAsString));

                                                        outgoingMessage = client.CreateMessage();
                                                        byte aByte3 = 2;
                                                        outgoingMessage.Write(aByte3);
                                                        client.SendMessage(outgoingMessage, NetDeliveryMethod.ReliableOrdered, 12);
                                                        transferVerified = true;
                                                    }
                                                    else if (dataMessageType == 3)
                                                    {
                                                        goto ReReadData3;
                                                    }
                                                    else
                                                    {
                                                        //  MessageBox.Show("Connection Error! This should not happen (unless you disconnected while downloading a savefile). If you see this message, please report it to the Steam discussion page.", "Connection Error!");
                                                        // MessageBox.Show("Unknown Connection Error 1! Tell the host to restart the server and try again.", "Connection Error!");
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    Thread.Sleep(1);
                                                    goto ReReadData3;
                                                }
                                            }
                                            else
                                            {
                                                // MessageBox.Show("Unknown Connection Error 2! Tell the host to restart the server and try again.", "Connection Error!");
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Thread.Sleep(1000);
                                            goto ReReadData2;
                                        }
                                    }
                                    else
                                    {
                                        // MessageBox.Show("Unknown Connection Error 3! Tell the host to restart the server and try again.", "Connection Error!");
                                        break;
                                    }
                                }
                                else
                                {
                                    Thread.Sleep(1000);
                                    goto ReReadData1;
                                }
                            }

                            if (dataMessage == "Recheck")
                            {
                                int dataMessageType = incomingMessage.ReadByte();

                                switch (dataMessageType)
                                {
                                    case 0:
                                        outgoingMessage = client.CreateMessage();
                                        byte aByte1 = 0;
                                        outgoingMessage.Write(aByte1);
                                        client.SendMessage(outgoingMessage, NetDeliveryMethod.ReliableOrdered, 10);
                                        break;

                                    case 1:
                                        outgoingMessage = client.CreateMessage();
                                        byte aByte2 = 1;
                                        outgoingMessage.Write(aByte2);
                                        client.SendMessage(outgoingMessage, NetDeliveryMethod.ReliableOrdered, 10);
                                        break;

                                    case 2:
                                        outgoingMessage = client.CreateMessage();
                                        byte aByte3 = 2;
                                        outgoingMessage.Write(aByte3);
                                        client.SendMessage(outgoingMessage, NetDeliveryMethod.ReliableOrdered, 10);
                                        break;
                                }
                            }

                            if (dataMessage == "AllDone" & allDone == false)
                            {
                                allDone = true;
                                ChoiceMessageBox.clientForm.clientInfoBox.Invoke(updateClientInfoInvoke, "\nAll clients have completed the download\nYou can now rejoin the Host's game", false);
                                outgoingMessage = client.CreateMessage();
                                byte aByte = 3;
                                outgoingMessage.Write(aByte);
                                client.SendMessage(outgoingMessage, NetDeliveryMethod.ReliableOrdered);
                            }
                            fileTransferStarted = false;
                            ChoiceMessageBox.clientForm.Invoke(fileTransferBarInvoke, "", "", true);
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            hostStatus = (NetConnectionStatus)incomingMessage.ReadByte();
                            if (hostStatus == NetConnectionStatus.Disconnected & clientForm.clientStart == true)
                            {
                                ChoiceMessageBox.clientForm.clientStartStopButton.Invoke(stopClientInvoke, this, null);
                            }

                            ChoiceMessageBox.clientForm.clientInfoBox.Invoke(updateClientInfoInvoke, hostStatus + "\n" + incomingMessage.ReadString(), false);
                            break;
                    }
                }
            }
        }


        public void WatchDownload()
        {
            while (clientForm.formClosing == false & clientForm.clientStart == true)
            {
                Thread.Sleep(1);
                bool first = true;
                int bytesReceivedCache = 0;
                while (fileTransferStarted == true)
                {
                    Thread.Sleep(1);
                    if (first == true)
                    {
                        bytesReceivedCache = client.Statistics.ReceivedBytes;
                        first = false;
                    }
                    else
                    {
                        kilobytesDownloaded = (client.Statistics.ReceivedBytes - bytesReceivedCache) / 1024;
                        ChoiceMessageBox.clientForm.Invoke(fileTransferBarInvoke, kilobytesDownloaded.ToString(), (savefileSize / 1024).ToString(), false);
                    }
                }
            }
        }


        private void ConnectTimer_Tick(object sender, EventArgs e)
        {
            connectTimerTick = true;
        }
    }
}
