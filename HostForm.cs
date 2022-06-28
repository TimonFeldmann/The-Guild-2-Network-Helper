using Lidgren.Network;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;

namespace The_Guild_2_Network_Helper
{
    public partial class hostForm : Form
    {
        public static int hostPort; // The host's Port.
        public static string hostsavegameDirectory; // The directory of the user's savegames folder.
        public static int hostMaxClients; // The amount of clients in the Host's game.
        public static int connectionsCountCache = 0;
        public static bool hostUPnP; // If UPnP should be used to portforward.
        public static bool hostAutoRestart; // If the users' game should auto-restart.
        public static bool hostStart; // Set to true once the user presses start.
        public static bool noHost; // Set to true if there was no host values found in the settings file (Need to make them);
        public static Point clientControlsOrigin;
        public static bool firstClient = true;
        public static bool formClosing = false;
        public static bool syncButtonClicked;


        public static List<Label> clientUsernameControls = new List<Label>();
        public static List<Label> clientIPControls = new List<Label>();
        public static List<ProgressBar> clientTransferBarControls = new List<ProgressBar>();
        public static List<Button> clientDropButtonControls = new List<Button>();
        public static List<PictureBox> clientConnectionBoxControls = new List<PictureBox>();
        public static List<PictureBox> clientTransferBoxControls = new List<PictureBox>();
        public static List<PictureBox> clientVerifiedBoxControls = new List<PictureBox>();

        public static Dictionary<System.Net.IPEndPoint, string> connectedClients = new Dictionary<System.Net.IPEndPoint, string>();

        // Caches the state of the folderDialog's selected path before it's checked again. When the directory path is later verified, if the selected path is the same as the cache, it means either
        // the path didn't change, or the cancel button was pressed on the folderDialog. Used to prevent errors when pressing cancel.
        public static string folderDialogCache;

        string[] autoDetectDirectories = new string[1]; // Array of paths that will be used to verify if directories exist.


        public hostForm()
        {
            this.Shown += HostForm_Shown; // Event that gets fired when the form is first loaded.
            this.FormClosing += HostForm_FormClosing; // Used to tell networking to shutdown when the form is closing.
            InitializeComponent();
        }

        private void HostForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (HostNetworking.host != null)
            {
                formClosing = true;
                HostNetworking.host.Shutdown("Server Shutting Down");
            }
        }

        private void HostForm_Shown(object sender, EventArgs e)
        {
            bool noFile = false;
            // When the mouse enters the directoryPathBox control, this event gets fired to set the tooltip of the directoryPathBox.
            directoryPathBox.MouseEnter += DirectoryPathBox_MouseEnter;

            // If the settings file exists next to the .exe, it will try to read the XML file.
            if (File.Exists(Directory.GetCurrentDirectory() + @"\Settings.xml"))
            {
                ReadHostXML();
            }
            // If the settings file does not exist, the program will try to automatically find the install directory by reading the install registry value.
            else
            {
                noFile = true;
            }
            // If the settings file does not exist, the program will try to automatically find the install directory by reading the install registry value. This is also done if no host attributes
            // were found which means no host settings exist.
            if (noFile == true | noHost == true)
            {
                hostsavegameDirectory = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 39680", "InstallLocation", null);
                if (hostsavegameDirectory != null)
                {
                    hostsavegameDirectory += @"\savegames";
                    if (Directory.Exists(hostsavegameDirectory))
                    {
                        MessageBox.Show("Autodetected Game Path!", "Game Path Detected!");
                        directoryPathBox.Text = hostsavegameDirectory;
                    }
                }
            }
        }


        // Checks to see if required variables are set and begins to initialize the host. If certain variables are not set, certain error messages are thrown at the user. The User is no
        // longer allowed to modify settings when started.
        public void hostStartStopButton_Click(object sender, EventArgs e)
        {
            if (hostStart == false) // If the host has not been started yet.
            {
                // CheckDirectories gets fired with the "Start" parameter to make sure the path is valid before the host is fully started.
                if (hostPortBox.Value != 0 & CheckDirectories("Start") == true & maxClientsBox.Value != 0)
                {
                    formClosing = false;
                    hostStart = true; // Host starting

                    // Change color of start button to change states.
                    hostStartStopButton.BackColor = Color.FromArgb(192, 0, 0);
                    hostStartStopButton.Text = "Stop Server";

                    outOfSyncButton.Visible = true; // Show out of sync button
                    outOfSyncButton.Enabled = true;

                    hostPortBox.ReadOnly = true;
                    hostPortBox.Enabled = false;
                    directoryPathBox.ReadOnly = true;
                    browseDirectoryButton.Enabled = false;
                    maxClientsBox.ReadOnly = true;
                    maxClientsBox.Enabled = false;
                    upnpCheckBox.Enabled = false;
                    //autoRestartCheckBox.Enabled = false;

                    hostPort = (int)hostPortBox.Value;
                    hostsavegameDirectory = directoryPathBox.Text;
                    hostMaxClients = (int)maxClientsBox.Value;
                    hostUPnP = upnpCheckBox.Checked;
                    //hostAutoRestart = autoRestartCheckBox.Checked;

                    WriteHostXML(); // Saves all important host variables to an XML file stored next to the .exe.

                    Thread networkingThread = new Thread(StartNetworking);
                    networkingThread.Start();
                }
                else if (hostPortBox.Value == 0)
                {
                    MessageBox.Show("You must enter a Port before trying to start the server!", "No Port!");
                }
                if (maxClientsBox.Value == 0)
                {
                    MessageBox.Show("You must enter the maximum amount of clients before trying to start the server!", "No Max Clients!");
                }
            }
            else // Stop Server
            {
                HostNetworking.host.Shutdown("Server Shutting Down");

                clientUsernameControls.Clear();
                clientIPControls.Clear();
                clientTransferBarControls.Clear();
                clientDropButtonControls.Clear();
                clientConnectionBoxControls.Clear();
                clientTransferBoxControls.Clear();
                clientVerifiedBoxControls.Clear();

                connectedClients.Clear();

                while (clientsPanel.Controls.Count != 0)
                {
                    clientsPanel.Controls.RemoveAt(0);
                }

                // Change color of stop button to change states.
                hostStartStopButton.BackColor = Color.FromArgb(0, 192, 0);
                hostStartStopButton.Text = "Start Server";

                outOfSyncButton.Visible = false; // Hide outOfSyncButton again

                hostPortBox.ReadOnly = false;
                hostPortBox.Enabled = true;
                directoryPathBox.ReadOnly = false;
                browseDirectoryButton.Enabled = true;
                maxClientsBox.ReadOnly = false;
                maxClientsBox.Enabled = true;
                upnpCheckBox.Enabled = true;
                //autoRestartCheckBox.Enabled = true;
                outOfSyncButton.Enabled = true;

                HostNetworking.outOfSync = false;

                firstClient = true;
                hostStart = false;
            }
        }


        private void browseDirectoryButton_Click(object sender, EventArgs e)
        {
            folderDialogCache = folderDialog.SelectedPath; // Cache the folderDialog selected path.
            folderDialog.ShowDialog();
            if (CheckDirectories("Dialog") == true) // Fires the method that checks to see if the selected directory is valid with the "Dialog" parameter.
            {
                directoryPathBox.Text = folderDialog.SelectedPath;
            }
        }


        private void DirectoryPathBox_MouseEnter(object sender, EventArgs e) // When the mouse enters the directoryPathBox, set the tooltip to the current directory entered.
        {
            this.hostToolTips.SetToolTip(this.directoryPathBox, directoryPathBox.Text);
        }


        // Method that checks to see if the directory selected either with the folder dialog option or the directory inputed into the Game Path textbox is valid. The method can be used
        // to check the folderDialog path by passing the string "Dialog" or it can be used to check the validity of the directoryPathBox by passing the string "Start". "Dialog" is
        // fired when the user makes his folderdialog choice. "Start" is fired when the user presses the start button.
        public bool CheckDirectories(string dialogOrStart)
        {
            bool validPath = false;
            if (dialogOrStart == "Dialog") // Fired by the Dialog.
            {
                if (Directory.Exists(folderDialog.SelectedPath)) // Gets the path from the folderDialog control.
                {
                    // Splits the path the user chose at "save". If the correct path is selected, this should result in a seperate string in the array with the value of "games".
                    autoDetectDirectories = folderDialog.SelectedPath.Split(new string[1] { "save" }, StringSplitOptions.None);
                    validPath = true;
                }
                else if (folderDialog.SelectedPath != folderDialogCache) // Directory does not exist and cancel was not pressed.
                {
                    MessageBox.Show("Selected directory does not exist!", "Invalid Directory!");
                    validPath = false;
                }
            }
            else if (dialogOrStart == "Start") // Fired by the start button.
            {
                if (Directory.Exists(directoryPathBox.Text)) // If the directory in directoryPathBox exists.
                {
                    autoDetectDirectories = directoryPathBox.Text.Split(new string[1] { "save" }, StringSplitOptions.None); // Same as above
                    validPath = true;
                }
                else // Same as above
                {
                    MessageBox.Show("Selected directory does not exist!", "Invalid Directory!");
                    validPath = false;
                }
            }

            // Once the strings of the directory path have been retrieved, all entries in the autoDetectDirectories array get looped through to see if there is a "games" string.
            // If there is a games string, it most likely means that the user chose the correct folder. If the loop reaches the end of the array without finding "games", then it most
            // likely means the user did not select the correct directory. Since the entire path is split by the word "save", the correct array would have two strings, one string with
            // the first part of the directory, and a second string just with "games".
            for (int i = 0; i <= autoDetectDirectories.Length - 1; i++)
            {
                if (autoDetectDirectories[i] == "games" & validPath == true)
                {
                    return (true);
                }
                else if (i == autoDetectDirectories.Length - 1) // Did not find the "games" string.
                {
                    if (dialogOrStart == "Start")
                    {
                        MessageBox.Show("Incorrect savegames path! \n\nPlease select the savegames folder in your Guild 2: Renaissance installation folder.\n\nExample:\n" + @"C:\Program Files (x86)\Steam\steamapps\common\The Guild 2 Renaissance\savegames", "Incorrect Folder Path!");
                    }
                    else if (folderDialog.SelectedPath != folderDialogCache) // Checks to see if the cancel button was hit or another non-choice was made.
                    {
                        MessageBox.Show("Incorrect savegames path! \n\nPlease select the savegames folder in your Guild 2: Renaissance installation folder.\n\nExample:\n" + @"C:\Program Files (x86)\Steam\steamapps\common\The Guild 2 Renaissance\savegames", "Incorrect Folder Path!");
                    }
                    return (false);
                }
            }
            return (false);
        }


        // Creates or updates an XML file with host settings.
        public void WriteHostXML()
        {
            XmlDocument hostXML = new XmlDocument();

            XmlNode baseNode;
            XmlNode hostNode;

            if (File.Exists(Directory.GetCurrentDirectory() + @"\Settings.xml")) // Load existing setting file and existing nodes.
            {
                hostXML.Load("Settings.xml");
                baseNode = hostXML.ChildNodes[0];
                hostNode = baseNode.ChildNodes[1];
            }
            else // If settings file doesn't exist, create it.
            {
                XmlNode clientNode; // Create the client node as well to fully setup the xml file.
                baseNode = hostXML.CreateElement("ClientAndHostSettings");
                clientNode = hostXML.CreateElement("Client");
                hostNode = hostXML.CreateElement("Host");

                hostXML.AppendChild(baseNode);
                baseNode.AppendChild(clientNode);
                baseNode.AppendChild(hostNode);
            }

            XmlAttribute hostPortAttribute = hostXML.CreateAttribute("HostPort");
            XmlAttribute hostGamePathAttribute = hostXML.CreateAttribute("GamePath");
            XmlAttribute hostMaxClientsAttribute = hostXML.CreateAttribute("MaxClients");
            XmlAttribute hostUPnPAttribute = hostXML.CreateAttribute("UPnP");
            XmlAttribute hostAutostartAttribute = hostXML.CreateAttribute("Autostart");

            hostPortAttribute.Value = Convert.ToString(hostPort);
            hostGamePathAttribute.Value = hostsavegameDirectory;
            hostMaxClientsAttribute.Value = Convert.ToString(hostMaxClients);
            hostUPnPAttribute.Value = Convert.ToString(hostUPnP);
            hostAutostartAttribute.Value = Convert.ToString(hostAutoRestart);

            hostNode.Attributes.Append(hostPortAttribute);
            hostNode.Attributes.Append(hostGamePathAttribute);
            hostNode.Attributes.Append(hostMaxClientsAttribute);
            hostNode.Attributes.Append(hostUPnPAttribute);
            hostNode.Attributes.Append(hostAutostartAttribute);

            hostXML.Save("Settings.xml");
        }


        public void ReadHostXML() // Loads the host node from the XML document and sets all the text boxes to the loaded values.
        {
            XmlDocument hostXML = new XmlDocument();

            hostXML.Load("Settings.xml");

            XmlNodeList hostNodeList = hostXML.ChildNodes[0].ChildNodes; // Load the base node, then the 2 child nodes (Client, Host)

            if (hostNodeList.Item(1).Attributes.Count != 0) // If the host node actually has attributes, or is empty.
            {
                hostPortBox.Value = Convert.ToDecimal(hostNodeList[1].Attributes[0].Value);
                directoryPathBox.Text = hostNodeList[1].Attributes[1].Value;
                maxClientsBox.Value = Convert.ToDecimal(hostNodeList[1].Attributes[2].Value);
                upnpCheckBox.Checked = Convert.ToBoolean(hostNodeList[1].Attributes[3].Value);
                //autoRestartCheckBox.Checked = Convert.ToBoolean(hostNodeList[1].Attributes[4].Value);
            }
            else
            {
                // If no host attributes were found, but the settings file was found, it will save host attributes the next time the server is initialized by setting noHost to true.
                noHost = true;
            }
        }


        public void StartNetworking() // Networking started by a seperate thread
        {
            HostNetworking hostNetwork = new HostNetworking(); // Start networking
        }


        // Adds new controls to the clientsPanel as more clients get added to the connection. The first client is generated based on coordinates. After the first client, the second client builds
        // upon the location of the first client's username label. Each client after that builds up from the previous client's username label.
        public void AddClientControls(string username, string ip)
        {
            for (int i = 0; i <= 6; i++) // Loop through each control to be added
            {
                if (firstClient == true) // Establishes the first client to be added to the clientsPanel. All other clients will build on top of each other.
                {
                    switch (i)
                    {
                        case 0: // Username
                            clientUsernameControls.Insert(0, new Label());

                            clientUsernameControls[0].AutoSize = true;
                            clientUsernameControls[0].Location = new Point(2, 8);
                            clientUsernameControls[0].Name = ip;
                            clientUsernameControls[0].Size = new Size(143, 13);
                            clientUsernameControls[0].Text = "Client: " + username;

                            clientsPanel.Controls.Add(clientUsernameControls[0]);
                            break;

                        case 1: // IP
                            clientIPControls.Insert(0,new Label());

                            clientIPControls[0].AutoSize = true;
                            clientIPControls[0].Location = new Point(2, 24);
                            clientIPControls[0].Name = ip;
                            clientIPControls[0].Size = new Size(104, 13);
                            clientIPControls[0].Text = "Address: " + ip;

                            clientsPanel.Controls.Add(clientIPControls[0]);
                            break;

                        case 2: // File transfer bar
                            clientTransferBarControls.Insert(0, new ProgressBar());

                            clientTransferBarControls[0].Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
                            clientTransferBarControls[0].Location = new Point(5, 40);
                            clientTransferBarControls[0].Name = ip;
                            clientTransferBarControls[0].Size = new Size(clientsPanel.Size.Width - 65, 15);
                            hostToolTips.SetToolTip(clientTransferBarControls[0], "Save transfer progress");

                            clientsPanel.Controls.Add(clientTransferBarControls[0]);

                            break;

                        case 3: // Drop client button
                            clientDropButtonControls.Insert(0,new Button());

                            clientDropButtonControls[0].BackColor = Color.Red;
                            clientDropButtonControls[0].BackgroundImage = Properties.Resources.Disconnect_9957;
                            clientDropButtonControls[0].BackgroundImageLayout = ImageLayout.Center;
                            clientDropButtonControls[0].FlatStyle = FlatStyle.Flat;
                            clientDropButtonControls[0].Location = new Point(170, 11);
                            clientDropButtonControls[0].Name = ip;
                            clientDropButtonControls[0].Size = new Size(20, 20);
                            clientDropButtonControls[0].TabStop = false;

                            clientDropButtonControls[0].Click += DropButton_Click;

                            hostToolTips.SetToolTip(clientDropButtonControls[0], "Drops this client from your connection.");

                            clientsPanel.Controls.Add(clientDropButtonControls[0]);
                            break;

                        case 4: // Connection box
                            clientConnectionBoxControls.Insert(0,new PictureBox());

                            clientConnectionBoxControls[0].Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                            clientConnectionBoxControls[0].BackColor = Color.Red;
                            clientConnectionBoxControls[0].Location = new Point(clientTransferBarControls[0].Size.Width + 8, 40);
                            clientConnectionBoxControls[0].Name = ip;
                            clientConnectionBoxControls[0].Size = new Size(15, 15);
                            hostToolTips.SetToolTip(clientConnectionBoxControls[0], "File transfer connection established");

                            clientsPanel.Controls.Add(clientConnectionBoxControls[0]);
                            break;

                        case 5: // Transfer box
                            clientTransferBoxControls.Insert(0,new PictureBox());

                            clientTransferBoxControls[0].Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                            clientTransferBoxControls[0].BackColor = Color.Red;
                            clientTransferBoxControls[0].Location = new Point(clientConnectionBoxControls[0].Location.X + 17, 40);
                            clientTransferBoxControls[0].Name = ip;
                            clientTransferBoxControls[0].Size = new Size(15, 15);
                            hostToolTips.SetToolTip(clientTransferBoxControls[0], "Save transfer started");

                            clientsPanel.Controls.Add(clientTransferBoxControls[0]);
                            break;


                        case 6: // Verify box
                            clientVerifiedBoxControls.Insert(0, new PictureBox());

                            clientVerifiedBoxControls[0].Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                            clientVerifiedBoxControls[0].BackColor = Color.Red;
                            clientVerifiedBoxControls[0].Location = new Point(clientTransferBoxControls[0].Location.X + 17, 40);
                            clientVerifiedBoxControls[0].Name = ip;
                            clientVerifiedBoxControls[0].Size = new Size(15, 15);
                            hostToolTips.SetToolTip(clientVerifiedBoxControls[0], "File verified");

                            clientsPanel.Controls.Add(clientVerifiedBoxControls[0]);
                            break;
                    }
                    if (i == 6) // Last control made, future clients will get generated differently.
                    {
                        // Generate future client controls procedurally.
                        firstClient = false;
                        // Set the origin for future controls to be built upon to the Username label defined above.
                        clientControlsOrigin = new Point(clientUsernameControls[0].Location.X, clientUsernameControls[0].Location.Y + 55);
                    }
                }
                else
                {
                    switch (i)
                    {
                        case 0: // Username
                            clientUsernameControls.Insert(clientUsernameControls.Count, new Label());

                            clientUsernameControls[clientUsernameControls.Count - 1].Location = clientControlsOrigin;
                            clientUsernameControls[clientUsernameControls.Count - 1].Name = ip;
                            clientUsernameControls[clientUsernameControls.Count - 1].Size = new Size(143, 13);
                            clientUsernameControls[clientUsernameControls.Count - 1].Text = "Client: " + username;

                            clientsPanel.Controls.Add(clientUsernameControls[clientUsernameControls.Count - 1]);

                            clientControlsOrigin = new Point(clientUsernameControls[clientUsernameControls.Count - 1].Location.X, clientUsernameControls[clientUsernameControls.Count - 1].Location.Y);
                            break;

                        case 1: // IP
                            clientIPControls.Insert(clientIPControls.Count, new Label());

                            clientIPControls[clientIPControls.Count - 1].AutoSize = true;
                            clientIPControls[clientIPControls.Count - 1].Location = new Point(clientControlsOrigin.X, clientControlsOrigin.Y + 16);
                            clientIPControls[clientIPControls.Count - 1].Name = ip;
                            clientIPControls[clientIPControls.Count - 1].Size = new Size(104, 13);
                            clientIPControls[clientIPControls.Count - 1].Text = "Address: " + ip;

                            clientsPanel.Controls.Add(clientIPControls[clientIPControls.Count - 1]);
                            break;

                        case 2: // File transfer bar
                            clientTransferBarControls.Insert(clientTransferBarControls.Count, new ProgressBar());

                            clientTransferBarControls[clientTransferBarControls.Count - 1].Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
                            clientTransferBarControls[clientTransferBarControls.Count - 1].Location = new Point(clientControlsOrigin.X + 3, clientControlsOrigin.Y + 32);
                            clientTransferBarControls[clientTransferBarControls.Count - 1].Name = ip;
                            clientTransferBarControls[clientTransferBarControls.Count - 1].Size = new Size(clientsPanel.Size.Width - 65, 15);
                            hostToolTips.SetToolTip(clientTransferBarControls[clientTransferBarControls.Count - 1], "Save transfer progress");

                            clientsPanel.Controls.Add(clientTransferBarControls[clientTransferBarControls.Count - 1]);

                            break;

                        case 3: // Drop client button
                            clientDropButtonControls.Insert(clientDropButtonControls.Count, new Button());

                            clientDropButtonControls[clientDropButtonControls.Count - 1].BackColor = Color.Red;
                            clientDropButtonControls[clientDropButtonControls.Count - 1].BackgroundImage = Properties.Resources.Disconnect_9957;
                            clientDropButtonControls[clientDropButtonControls.Count - 1].BackgroundImageLayout = ImageLayout.Center;
                            clientDropButtonControls[clientDropButtonControls.Count - 1].FlatStyle = FlatStyle.Flat;
                            clientDropButtonControls[clientDropButtonControls.Count - 1].Location = new Point(clientControlsOrigin.X + 168, clientControlsOrigin.Y + 3);
                            clientDropButtonControls[clientDropButtonControls.Count - 1].Name = ip;
                            clientDropButtonControls[clientDropButtonControls.Count - 1].Size = new Size(20, 20);
                            clientDropButtonControls[clientDropButtonControls.Count - 1].TabStop = false;

                            clientDropButtonControls[clientDropButtonControls.Count - 1].Click += DropButton_Click;

                            hostToolTips.SetToolTip(clientDropButtonControls[clientDropButtonControls.Count - 1], "Drops this client from your connection.");

                            clientsPanel.Controls.Add(clientDropButtonControls[clientDropButtonControls.Count - 1]);
                            break;

                        case 4: // Connection box
                            clientConnectionBoxControls.Insert(clientConnectionBoxControls.Count, new PictureBox());

                            clientConnectionBoxControls[clientConnectionBoxControls.Count - 1].Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                            clientConnectionBoxControls[clientConnectionBoxControls.Count - 1].BackColor = Color.Red;
                            clientConnectionBoxControls[clientConnectionBoxControls.Count - 1].Location = new Point(clientTransferBarControls[clientTransferBarControls.Count - 1].Size.Width + 8, clientControlsOrigin.Y + 32);
                            clientConnectionBoxControls[clientConnectionBoxControls.Count - 1].Name = ip;
                            clientConnectionBoxControls[clientConnectionBoxControls.Count - 1].Size = new Size(15, 15);
                            hostToolTips.SetToolTip(clientConnectionBoxControls[clientConnectionBoxControls.Count - 1], "File transfer connection established");

                            clientsPanel.Controls.Add(clientConnectionBoxControls[clientConnectionBoxControls.Count - 1]);
                            break;

                        case 5: // Transfer box
                            clientTransferBoxControls.Insert(clientTransferBoxControls.Count, new PictureBox());

                            clientTransferBoxControls[clientTransferBoxControls.Count - 1].Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                            clientTransferBoxControls[clientTransferBoxControls.Count - 1].BackColor = Color.Red;
                            clientTransferBoxControls[clientTransferBoxControls.Count - 1].Location = new Point(clientConnectionBoxControls[clientConnectionBoxControls.Count - 1].Location.X + 17, clientControlsOrigin.Y + 32);
                            clientTransferBoxControls[clientTransferBoxControls.Count - 1].Name = ip;
                            clientTransferBoxControls[clientTransferBoxControls.Count - 1].Size = new Size(15, 15);
                            hostToolTips.SetToolTip(clientTransferBoxControls[clientTransferBoxControls.Count - 1], "Save transfer started");

                            clientsPanel.Controls.Add(clientTransferBoxControls[clientTransferBoxControls.Count - 1]);
                            break;


                        case 6: // Verify box
                            clientVerifiedBoxControls.Insert(clientVerifiedBoxControls.Count, new PictureBox());

                            clientVerifiedBoxControls[clientVerifiedBoxControls.Count - 1].Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                            clientVerifiedBoxControls[clientVerifiedBoxControls.Count - 1].BackColor = Color.Red;
                            clientVerifiedBoxControls[clientVerifiedBoxControls.Count - 1].Location = new Point(clientTransferBoxControls[clientConnectionBoxControls.Count - 1].Location.X + 17, clientControlsOrigin.Y + 32);
                            clientVerifiedBoxControls[clientVerifiedBoxControls.Count - 1].Name = ip;
                            clientVerifiedBoxControls[clientVerifiedBoxControls.Count - 1].Size = new Size(15, 15);
                            hostToolTips.SetToolTip(clientVerifiedBoxControls[clientVerifiedBoxControls.Count - 1], "File verified");

                            clientsPanel.Controls.Add(clientVerifiedBoxControls[clientVerifiedBoxControls.Count - 1]);
                            break;
                    }
                    if (i == 6)
                    {
                        // Set the origin to the previously created username label plus an offset.
                        clientControlsOrigin = new Point(clientUsernameControls[clientUsernameControls.Count - 1].Location.X, clientUsernameControls[clientUsernameControls.Count - 1].Location.Y + 55);
                    }
                }
            }
        }


        public void RemoveClientControls(string ip)
        {
            int indexRemove = 0;
            for (int i = 0; i < clientDropButtonControls.Count; i++)
            {
                if (ip == clientDropButtonControls[i].Name)
                {
                    clientUsernameControls[i].Dispose();
                    clientIPControls[i].Dispose();
                    clientTransferBarControls[i].Dispose();
                    clientDropButtonControls[i].Dispose();
                    clientConnectionBoxControls[i].Dispose();
                    clientTransferBoxControls[i].Dispose();
                    clientVerifiedBoxControls[i].Dispose();

                    for (int i2 = i + 1; i2 < clientDropButtonControls.Count; i2++)
                    {
                        clientUsernameControls[i2].Location = new Point(clientUsernameControls[i2].Location.X, clientUsernameControls[i2].Location.Y - 55);
                        clientIPControls[i2].Location = new Point(clientIPControls[i2].Location.X, clientIPControls[i2].Location.Y - 55);
                        clientTransferBarControls[i2].Location = new Point(clientTransferBarControls[i2].Location.X, clientTransferBarControls[i2].Location.Y - 55);
                        clientDropButtonControls[i2].Location = new Point(clientDropButtonControls[i2].Location.X, clientDropButtonControls[i2].Location.Y - 55);
                        clientConnectionBoxControls[i2].Location = new Point(clientConnectionBoxControls[i2].Location.X, clientConnectionBoxControls[i2].Location.Y - 55);
                        clientTransferBoxControls[i2].Location = new Point(clientTransferBoxControls[i2].Location.X, clientTransferBoxControls[i2].Location.Y - 55);
                        clientVerifiedBoxControls[i2].Location = new Point(clientVerifiedBoxControls[i2].Location.X, clientVerifiedBoxControls[i2].Location.Y - 55);
                    }

                    if (i == 0)
                    {
                        clientUsernameControls[0].Location = new Point(clientUsernameControls[0].Location.X, clientUsernameControls[0].Location.Y - 55);
                        clientIPControls[0].Location = new Point(clientIPControls[0].Location.X, clientIPControls[0].Location.Y - 55);
                        clientTransferBarControls[0].Location = new Point(clientTransferBarControls[0].Location.X, clientTransferBarControls[0].Location.Y - 55);
                        clientDropButtonControls[0].Location = new Point(clientDropButtonControls[0].Location.X, clientDropButtonControls[0].Location.Y - 55);
                        clientConnectionBoxControls[0].Location = new Point(clientConnectionBoxControls[0].Location.X, clientConnectionBoxControls[0].Location.Y - 55);
                        clientTransferBoxControls[0].Location = new Point(clientTransferBoxControls[0].Location.X, clientTransferBoxControls[0].Location.Y - 55);
                        clientVerifiedBoxControls[0].Location = new Point(clientVerifiedBoxControls[0].Location.X, clientVerifiedBoxControls[0].Location.Y - 55);
                    }

                    clientUsernameControls.RemoveAt(i);
                    clientIPControls.RemoveAt(i);
                    clientTransferBarControls.RemoveAt(i);
                    clientDropButtonControls.RemoveAt(i);
                    clientConnectionBoxControls.RemoveAt(i);
                    clientTransferBoxControls.RemoveAt(i);
                    clientVerifiedBoxControls.RemoveAt(i);

                    clientControlsOrigin = new Point(clientControlsOrigin.X, clientControlsOrigin.Y - 55);
                }
            }
        }

        /// <summary>
        /// Modify outOfSync Button.
        /// </summary>
        public void ModifyControls()
        {
            if (outOfSyncButton.Enabled == true)
            {
                outOfSyncButton.Enabled = false;
            }
            else
            {
                Thread.Sleep(1000);
                outOfSyncButton.Enabled = true;
            }
        }

        /// <summary>
        /// Modify client indicators.
        /// </summary>
        /// <param name="indicatorBox"></param>
        /// <param name="On"></param>
        public void ModifyControls(PictureBox indicatorBox, bool On)
        {
            if (On == true)
            {
                indicatorBox.BackColor = Color.Green;
            }
            else
            {
                indicatorBox.BackColor = Color.Red;
            }
        }

        /// <summary>
        /// Modify client progress bars.
        /// </summary>
        /// <param name="progressBar"></param>
        /// <param name="value"></param>
        public void ModifyControls(ProgressBar progressBar, int barValue, int fileSize, int amountSent, bool reset)
        {
            if (reset == false & HostNetworking.outOfSync == true)
            {
                progressBar.Value = barValue;

                hostToolTips.SetToolTip(progressBar, "Save transfer progress: " + amountSent / 1024 + "kb/" + fileSize / 1024 + "kb" + " (Upload will exceed savefile size)");
            }
            else
            {
                progressBar.Value = 0;

                hostToolTips.SetToolTip(progressBar, "Save transfer progress");
            }
        }

   
        private void DropButton_Click(object sender, EventArgs e)
        {
            Button dropButton = (Button)sender;
            foreach (NetConnection connection in HostNetworking.host.Connections)
            {
                if (connection.RemoteEndPoint.ToString() == dropButton.Name)
                {
                    RemoveClientControls(connection.RemoteEndPoint.ToString());
                    HostNetworking.host.GetConnection(connection.RemoteEndPoint).Disconnect("Kicked From Server");
                }
            }
        }

        private void outOfSyncButton_Click(object sender, EventArgs e)
        {
            HostNetworking.outOfSync = true;
        }
    }
}
