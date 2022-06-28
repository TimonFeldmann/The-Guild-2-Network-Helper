using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Guild_2_Network_Helper
{
    class HostNetworking
    {
        public static NetServer host;
        NetPeerConfiguration config;
        public delegate void RecheckTimerDelegate();

        Thread receiveMessagesThread;
        Thread sendMessagesThread;
        Thread manageConnectionsThread;

        System.Threading.Timer recheckTimer;

        NetIncomingMessage incomingMessage;
        NetOutgoingMessage outgoingMessage;
        NetOutgoingMessage unconnectedOutgoingMessage;
        NetOutgoingMessage outgoingDataMessage;

        NetConnectionStatus clientStatus;

        public delegate void startStopButton(object sender, FormClosedEventArgs e);
        startStopButton stopHostInvoke;
        public delegate void addClientControls(string username, string ip);
        addClientControls addClientControlsInvoke;
        public delegate void removeClientControls(string ip);
        public delegate void modifyOutOfSyncButton();
        modifyOutOfSyncButton modifyOutOfSyncButtonInvoke;
        public delegate void modifyConnectionIndicators(PictureBox indicatorBox, bool On);
        modifyConnectionIndicators modifyConnectionIndicatorsInvoke;
        public delegate void modifyTransferBars(ProgressBar progressBar, int barValue, int fileSize, int amountSent, bool reset);
        modifyTransferBars modifyTransferBarsInvoke;

        public List<IPAddress> blockedClients = new List<IPAddress>();
        public List<IPEndPoint> verifiedClients = new List<IPEndPoint>();
        public List<IPEndPoint> verifiedClientsStep2 = new List<IPEndPoint>();
        public Dictionary<IPEndPoint, long> connectionCaches = new Dictionary<IPEndPoint, long>();

        public static bool outOfSync;
        public bool firstOutOfSync = true;
        public bool savefilesSent;
        public bool recheck;
        public bool recheckTimerStarted;
        public bool UPnPMessageBoxShown;

        public string savefileSize;

        public HostNetworking()
        {
            config = new NetPeerConfiguration("MyNetwork");
            config.Port = hostForm.hostPort;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.Data);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            config.DisableMessageType(NetIncomingMessageType.DebugMessage);

            config.MaximumConnections = hostForm.hostMaxClients;

            config.ConnectionTimeout = 9999999;

            if (hostForm.hostUPnP == true)
            {
                config.EnableUPnP = true;
            }

            stopHostInvoke = ChoiceMessageBox.hostForm.hostStartStopButton_Click;
            addClientControlsInvoke = ChoiceMessageBox.hostForm.AddClientControls;
            modifyOutOfSyncButtonInvoke = ChoiceMessageBox.hostForm.ModifyControls;
            modifyConnectionIndicatorsInvoke = ChoiceMessageBox.hostForm.ModifyControls;
            modifyTransferBarsInvoke = ChoiceMessageBox.hostForm.ModifyControls;

            host = new NetServer(config);
            host.Start();

            if (hostForm.hostUPnP == true)
            {
                host.UPnP.ForwardPort(hostForm.hostPort, "The Guild 2 Network Helper");
            }

            receiveMessagesThread = new Thread(ReceiveMessages);
            receiveMessagesThread.Start();

            sendMessagesThread = new Thread(SendMessages);
            sendMessagesThread.Start();

            manageConnectionsThread = new Thread(ManageConnections);
            manageConnectionsThread.Start();
        }


        public void ReceiveMessages()
        {
            while (hostForm.formClosing == false & hostForm.hostStart == true)
            {
                try
                {
                    Thread.Sleep(1);

                    incomingMessage = host.ReadMessage();

                    if (incomingMessage != null)
                    {
                        bool connectionBlocked = false;

                        foreach (IPAddress blockedAddress in blockedClients)
                        {
                            if (incomingMessage.SenderEndPoint.Address.ToString() == blockedAddress.ToString())
                            {
                                connectionBlocked = true;
                                unconnectedOutgoingMessage = host.CreateMessage("Connection Denied");
                                host.SendUnconnectedMessage(unconnectedOutgoingMessage, incomingMessage.SenderEndPoint);
                                break;
                            }
                        }

                        if (connectionBlocked == false)
                        {
                            switch (incomingMessage.MessageType)
                            {
                                case NetIncomingMessageType.DiscoveryRequest:
                                    Debug.WriteLine("DiscoveryRequest");
                                    outgoingMessage = host.CreateMessage("Discovered");
                                    host.SendDiscoveryResponse(outgoingMessage, incomingMessage.SenderEndPoint);
                                    break;
                                case NetIncomingMessageType.ConnectionApproval:
                                    if (outOfSync == false)
                                    {
                                        string username;
                                        string address;

                                        username = incomingMessage.ReadString();
                                        address = incomingMessage.SenderEndPoint.ToString();

                                        bool usernameUsed = false;
                                        foreach (string checkUsername in hostForm.connectedClients.Values)
                                        {
                                            if (username == checkUsername)
                                            {
                                                unconnectedOutgoingMessage = host.CreateMessage("Username Already Used!");
                                                host.SendUnconnectedMessage(unconnectedOutgoingMessage, incomingMessage.SenderEndPoint);
                                                usernameUsed = true;
                                                break;
                                            }
                                        }
                                        if (usernameUsed == true)
                                        {
                                            break;
                                        }

                                        DialogResult YesNo;

                                        YesNo = MessageBox.Show("The following user would like to connect:\n\n" + "User: " + username + "\n" + "Address: " + address + "\n\nAccept the connection?", "Incoming Connection", MessageBoxButtons.YesNo);
                                        if (YesNo == DialogResult.Yes)
                                        {
                                            incomingMessage.SenderConnection.Approve();
                                        }
                                        else
                                        {
                                            incomingMessage.SenderConnection.Deny("Connection Denied");
                                            blockedClients.Add(incomingMessage.SenderEndPoint.Address);
                                        }
                                    }
                                    break;
                                case NetIncomingMessageType.StatusChanged:
                                    clientStatus = (NetConnectionStatus)incomingMessage.ReadByte();
                                    Debug.WriteLine("Client: " + clientStatus);
                                    if (clientStatus == NetConnectionStatus.Connected)
                                    {
                                        string statusUsername = incomingMessage.SenderConnection.RemoteHailMessage.ReadString();
                                        hostForm.connectedClients.Add(incomingMessage.SenderEndPoint, statusUsername);

                                        ChoiceMessageBox.hostForm.Invoke(addClientControlsInvoke, statusUsername, incomingMessage.SenderEndPoint.ToString());
                                    }
                                    break;

                                case NetIncomingMessageType.Data:
                                    byte dataMessage = incomingMessage.ReadByte();
                                    IPEndPoint dataAddress = incomingMessage.SenderEndPoint;

                                    if (outOfSync == true)
                                    {
                                        switch (dataMessage)
                                        {
                                            case 0:
                                                foreach (IPEndPoint connectionAddress in hostForm.connectedClients.Keys)
                                                {
                                                    if (dataAddress.ToString() == connectionAddress.ToString())
                                                    {
                                                        foreach (PictureBox connectionBox in hostForm.clientConnectionBoxControls)
                                                        {
                                                            if (connectionBox.Name == dataAddress.ToString())
                                                            {
                                                                ChoiceMessageBox.hostForm.Invoke(modifyConnectionIndicatorsInvoke, connectionBox, true);
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            case 1:
                                                foreach (IPEndPoint connectionAddress in hostForm.connectedClients.Keys)
                                                {
                                                    if (dataAddress.ToString() == connectionAddress.ToString())
                                                    {
                                                        foreach (PictureBox transferBox in hostForm.clientTransferBoxControls)
                                                        {
                                                            if (transferBox.Name == dataAddress.ToString())
                                                            {
                                                                ChoiceMessageBox.hostForm.Invoke(modifyConnectionIndicatorsInvoke, transferBox, true);
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            case 2:
                                                foreach (IPEndPoint connectionAddress in hostForm.connectedClients.Keys)
                                                {
                                                    if (dataAddress.ToString() == connectionAddress.ToString())
                                                    {
                                                        foreach (PictureBox verifiedBox in hostForm.clientVerifiedBoxControls)
                                                        {
                                                            if (verifiedBox.Name == dataAddress.ToString() & verifiedClients.Count != host.ConnectionsCount)
                                                            {
                                                                if (verifiedClients.Contains(connectionAddress))
                                                                {
                                                                }
                                                                else
                                                                {
                                                                    ChoiceMessageBox.hostForm.Invoke(modifyConnectionIndicatorsInvoke, verifiedBox, true);
                                                                    verifiedClients.Add(connectionAddress);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            case 3:
                                                if (verifiedClientsStep2.Contains(dataAddress))
                                                {
                                                }
                                                else
                                                {
                                                    verifiedClientsStep2.Add(dataAddress);
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }


        public void SendMessages()
        {
            while (hostForm.formClosing == false & hostForm.hostStart == true)
            {
                Thread.Sleep(1);
                if (outOfSync == true & host.ConnectionsCount > 0 & savefilesSent == false)
                {
                    ChoiceMessageBox.hostForm.Invoke(modifyOutOfSyncButtonInvoke);

                    outgoingDataMessage = host.CreateMessage("Out of Sync");
                    host.SendToAll(outgoingDataMessage, NetDeliveryMethod.ReliableOrdered);

                    List<string> savegames = new List<string>();
                    savegames = Directory.GetFiles(hostForm.hostsavegameDirectory).ToList<string>();

                    string newestSaveFile = "";
                    foreach (string saveFile in savegames)
                    {
                        if (newestSaveFile == "")
                        {
                            newestSaveFile = saveFile;
                        }
                        else
                        {
                            if (File.GetLastWriteTime(saveFile) > File.GetLastWriteTime(newestSaveFile) & Path.GetExtension(saveFile) == ".wld")
                            {
                                newestSaveFile = saveFile;
                            }
                        }
                    }
                    string saveFileName = Path.GetFileName(newestSaveFile);

                    outgoingDataMessage = host.CreateMessage();
                    byte aByte1 = 0;
                    outgoingDataMessage.Write(aByte1);
                    outgoingDataMessage.Write(saveFileName);
                    host.SendToAll(outgoingDataMessage, NetDeliveryMethod.ReliableOrdered);

                    byte[] savefileBytes = File.ReadAllBytes(newestSaveFile);
                    string savefileAsString = Convert.ToBase64String(savefileBytes);
                    savefileSize = Convert.ToString(savefileBytes.Length);

                    outgoingDataMessage = host.CreateMessage();
                    byte aByte2 = 1;
                    outgoingDataMessage.Write(aByte2);
                    outgoingDataMessage.Write(savefileSize);
                    host.SendToAll(outgoingDataMessage, NetDeliveryMethod.ReliableOrdered);

                    outgoingDataMessage = host.CreateMessage();
                    byte aByte3 = 2;
                    outgoingDataMessage.Write(aByte3);
                    outgoingDataMessage.Write(savefileAsString);

                    host.SendToAll(outgoingDataMessage, NetDeliveryMethod.ReliableOrdered);

                    savefilesSent = true;
                }
                if (host.ConnectionsCount == 0 & outOfSync == true)
                {
                    outOfSync = false;
                    firstOutOfSync = true;
                    recheck = false;
                    recheckTimerStarted = false;
                    savefilesSent = false;
                    savefileSize = "";
                    verifiedClients.Clear();
                    verifiedClientsStep2.Clear();
                    connectionCaches.Clear();
                    ChoiceMessageBox.hostForm.Invoke(modifyOutOfSyncButtonInvoke);
                    ChoiceMessageBox.hostForm.Invoke(stopHostInvoke, this, null);
                    MessageBox.Show("No clients connected!", "No Clients!");
                }

                if (outOfSync == true & recheck == true)
                {
                    try
                    {
                        foreach (PictureBox connectionBox in hostForm.clientConnectionBoxControls)
                        {
                            if (connectionBox.BackColor == System.Drawing.Color.Red)
                            {
                                NetConnection recheckConnection;
                                foreach (NetConnection connection in host.Connections)
                                {
                                    if (connection.RemoteEndPoint.ToString() == connectionBox.Name)
                                    {
                                        recheckConnection = connection;
                                        outgoingDataMessage = host.CreateMessage();
                                        byte aByte = 0;
                                        outgoingDataMessage.Write("Recheck");
                                        outgoingDataMessage.Write(aByte);
                                        host.SendMessage(outgoingDataMessage, recheckConnection, NetDeliveryMethod.ReliableUnordered);
                                    }
                                }
                            }
                        }

                        foreach (PictureBox transferBox in hostForm.clientTransferBoxControls)
                        {
                            if (transferBox.BackColor == System.Drawing.Color.Red)
                            {
                                NetConnection recheckConnection;
                                foreach (NetConnection connection in host.Connections)
                                {
                                    if (connection.RemoteEndPoint.ToString() == transferBox.Name)
                                    {
                                        recheckConnection = connection;
                                        outgoingDataMessage = host.CreateMessage();
                                        byte aByte = 1;
                                        outgoingDataMessage.Write("Recheck");
                                        outgoingDataMessage.Write(aByte);
                                        host.SendMessage(outgoingDataMessage, recheckConnection, NetDeliveryMethod.ReliableUnordered);
                                    }
                                }
                            }
                        }

                        foreach (PictureBox verifiedBox in hostForm.clientVerifiedBoxControls)
                        {
                            if (verifiedBox.BackColor == System.Drawing.Color.Red)
                            {
                                NetConnection recheckConnection;
                                foreach (NetConnection connection in host.Connections)
                                {
                                    if (connection.RemoteEndPoint.ToString() == verifiedBox.Name)
                                    {
                                        recheckConnection = connection;
                                        outgoingDataMessage = host.CreateMessage();
                                        byte aByte = 2;
                                        outgoingDataMessage.Write("Recheck");
                                        outgoingDataMessage.Write(aByte);
                                        host.SendMessage(outgoingDataMessage, recheckConnection, NetDeliveryMethod.ReliableUnordered);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    if (host.ConnectionsCount == verifiedClients.Count)
                    {
                        outgoingDataMessage = host.CreateMessage("AllDone");
                        host.SendToAll(outgoingDataMessage, NetDeliveryMethod.ReliableOrdered);
                    }
                    recheck = false;
                }
            }
        }


        public void ManageConnections()
        {
            while (hostForm.formClosing == false & hostForm.hostStart == true)
            {
                try
                {
                    Thread.Sleep(1);

                    if (hostForm.hostUPnP == true)
                    {
                        if (UPnPMessageBoxShown == false & host.UPnP.Status == UPnPStatus.NotAvailable)
                        {
                            MessageBox.Show("UPnP is unavailable!\n\nYour host port must be port forwarded in order for client connections to be established.", "No UPnP!");
                            UPnPMessageBoxShown = true;
                        }
                    }

                    if (host.ConnectionsCount < hostForm.connectedClients.Count)
                    {
                        List<IPEndPoint> connectionAddreses = new List<IPEndPoint>();

                        for (int i = 0; i < host.ConnectionsCount; i++)
                        {
                            connectionAddreses.Add(host.Connections[i].RemoteEndPoint);
                        }

                        List<IPEndPoint> connectedClientsToBeRemoved = new List<IPEndPoint>();

                        connectedClientsToBeRemoved = hostForm.connectedClients.Keys.Except(connectionAddreses).ToList();

                        for (int i2 = 0; i2 < connectedClientsToBeRemoved.Count(); i2++)
                        {
                            removeClientControls removeClientControlsInvoke;
                            removeClientControlsInvoke = ChoiceMessageBox.hostForm.RemoveClientControls;

                            ChoiceMessageBox.hostForm.Invoke(removeClientControlsInvoke, connectedClientsToBeRemoved.ElementAt(i2).ToString());

                            hostForm.connectedClients.Remove(connectedClientsToBeRemoved.ElementAt(i2));
                            if (hostForm.connectedClients.Count == 0)
                            {
                                hostForm.firstClient = true;
                            }
                        }

                        if (host.ConnectionsCount == 0)
                        {
                            hostForm.connectedClients.Clear();
                        }
                    }

                    if (outOfSync == true)
                    {
                        if (firstOutOfSync == true)
                        {
                            foreach (NetConnection connection in host.Connections)
                            {
                                connectionCaches.Add(connection.RemoteEndPoint, connection.Statistics.SentBytes);
                            }
                            firstOutOfSync = false;
                        }
                        else
                        {
                            foreach (NetConnection connection in host.Connections)
                            {
                                foreach (ProgressBar progressBar in hostForm.clientTransferBarControls)
                                {
                                    if (progressBar.Name == connection.RemoteEndPoint.ToString() & verifiedClients.Count != host.ConnectionsCount)
                                    {
                                        long connectionCache = 0;
                                        int amountUploaded = 0;
                                        int barValue = 0;
                                        connectionCaches.TryGetValue(connection.RemoteEndPoint, out connectionCache);
                                        amountUploaded = (int)connection.Statistics.SentBytes - (int)connectionCache;

                                        if (amountUploaded != 0 & savefileSize != null)
                                        {
                                            barValue = Convert.ToInt32(((double)amountUploaded / Convert.ToDouble(savefileSize)) * 100);
                                            if (connectionCache > amountUploaded & barValue == 0)
                                            {
                                                barValue = 0;
                                            }
                                            if (barValue >= 100)
                                            {
                                                barValue = 100;
                                            }
                                        }

                                        ChoiceMessageBox.hostForm.Invoke(modifyTransferBarsInvoke, progressBar, barValue, Convert.ToInt32(savefileSize), amountUploaded, false);
                                    }
                                }
                                if (recheckTimerStarted == false & outOfSync == true)
                                {
                                    recheckTimerStarted = true;
                                    recheckTimer = new System.Threading.Timer(RecheckTimer, this, 3000, 3000);
                                }
                                if (host.ConnectionsCount == verifiedClients.Count & host.ConnectionsCount == verifiedClientsStep2.Count)
                                {
                                    recheckTimerStarted = false;
                                    verifiedClients.Clear();
                                    verifiedClientsStep2.Clear();
                                    outOfSync = false;
                                    firstOutOfSync = true;
                                    savefilesSent = false;
                                    while (host.ReadMessage() != null)
                                    {
                                        Thread.Sleep(1);
                                    }

                                    connectionCaches.Clear();

                                    foreach (ProgressBar progressBar in hostForm.clientTransferBarControls)
                                    {
                                        ChoiceMessageBox.hostForm.Invoke(modifyTransferBarsInvoke, progressBar, 0, 0, 0, true);
                                    }

                                    foreach (PictureBox connectionBox in hostForm.clientConnectionBoxControls)
                                    {
                                        ChoiceMessageBox.hostForm.Invoke(modifyConnectionIndicatorsInvoke, connectionBox, false);
                                    }

                                    foreach (PictureBox transferBox in hostForm.clientTransferBoxControls)
                                    {
                                        ChoiceMessageBox.hostForm.Invoke(modifyConnectionIndicatorsInvoke, transferBox, false);
                                    }

                                    foreach (PictureBox verifiedBox in hostForm.clientVerifiedBoxControls)
                                    {
                                        ChoiceMessageBox.hostForm.Invoke(modifyConnectionIndicatorsInvoke, verifiedBox, false);
                                    }

                                    ChoiceMessageBox.hostForm.Invoke(modifyOutOfSyncButtonInvoke);
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
        }

        public void RecheckTimer(Object info)
        {
            if (outOfSync == true)
            {
                recheck = true;
            }
            else
            {
                recheckTimer.Dispose();
            }
        }
    }
}
