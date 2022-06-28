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
using System.Xml;

namespace The_Guild_2_Network_Helper
{
    public partial class clientForm : Form
    {
        public static string hostIP; // The host's IP address.
        public static int hostPort; // The host's Port.
        public static int clientPort; // The client's Port.
        public static string clientsavegameDirectory; // The directory of the user's savegames folder.
        public static string clientUsername; // The username that will be sent to the host.
        public static bool clientAutoRestart; // If the users' game should auto-restart.
        public static bool clientStart; // Set to true once the user presses start.
        public static bool noClient; // Set to true if there was no client values found in the settings file (Need to make them);
        public static bool formClosing = false;

        // Caches the state of the folderDialog's selected path before it's checked again. When the directory path is later verified, if the selected path is the same as the cache, it means either
        // the path didn't change, or the cancel button was pressed on the folderDialog. Used to prevent errors when pressing cancel.
        public static string folderDialogCache;

        string[] autoDetectDirectories = new string[1]; // Array of paths that will be used to verify if directories exist.


        public clientForm()
        {
            this.Shown += ClientForm_Shown; // Event that gets fired when the form is first loaded.
            this.FormClosing += ClientForm_FormClosing;
            InitializeComponent();
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ClientNetworking.client != null)
            {
                ClientNetworking.client.Shutdown("Shutting Down");
                formClosing = true;
            }
        }


        // Event is fired when form is loaded. Tries to find the default steam installation on most folders. If it finds the correct folder, it will set the game path automatically.
        // If it doesn't find the game's path, then the user must manually set the path.
        private void ClientForm_Shown(object sender, EventArgs e)
        {
            bool noFile = false; // Set to true if no settings file was found.

            // When the mouse enters the directoryPathBox control, this event gets fired to set the tooltip of the directoryPathBox.
            directoryPathBox.MouseEnter += DirectoryPathBox_MouseEnter;

            if (File.Exists(Directory.GetCurrentDirectory() + @"\Settings.xml"))    // If the settings file exists next to the .exe, it will try to read the XML file.
            {
                ReadClientXML();
            }
            else
            { 
                noFile = true;
            }
            // If the settings file does not exist, the program will try to automatically find the install directory by reading the install registry value. This is also done if no client attributes
            // were found which means no client settings exist.
            if (noFile == true | noClient == true)
            {
                clientsavegameDirectory = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 39680", "InstallLocation", null);
                if (clientsavegameDirectory != null)
                {
                    clientsavegameDirectory += @"\savegames";
                    if (Directory.Exists(clientsavegameDirectory))
                    {
                        MessageBox.Show("Autodetected Game Path!", "Game Path Detected!");
                        directoryPathBox.Text = clientsavegameDirectory;
                    }
                }
            }
        }

        /*
        public void ActivateForm()
        {
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }
        */

        // Checks to see if required variables are set and begins to initialize the client. If certain variables are not set, certain error messages are thrown at the user. The User is no
        // longer allowed to modify settings when started.
        public void clientStartStopButton_Click(object sender, EventArgs e)
        {
            if (clientStart == false) // If the client has not been started yet.
            {
                // CheckDirectories gets fired with the "Start" parameter to make sure the path is valid before the client is fully started.
                if (usernameBox.Text != "" & CheckDirectories("Start") == true & hostIPBox.Text != "" & hostPortBox.Value != 0 & clientPortBox.Value != 0)
                {
                    UpdateClientInfoBox("Starting Client...",true);
                    formClosing = false;
                    clientStart = true; // Client starting

                    // Change color of start button to change states.
                    clientStartStopButton.BackColor = Color.FromArgb(192, 0, 0);
                    clientStartStopButton.Text = "Disconnect";

                    hostIPBox.ReadOnly = true;
                    hostPortBox.ReadOnly = true;
                    hostPortBox.Enabled = false;
                    clientPortBox.ReadOnly = true;
                    clientPortBox.Enabled = false;
                    directoryPathBox.ReadOnly = true;
                    browseDirectoryButton.Enabled = false;
                    usernameBox.ReadOnly = true;
                    //autoRestartCheckBox.Enabled = false;

                    // Store all user settings as variables to work with later.
                    hostIP = hostIPBox.Text;
                    hostPort = (int)hostPortBox.Value;
                    clientPort = (int)clientPortBox.Value;
                    clientsavegameDirectory = directoryPathBox.Text;
                    clientUsername = usernameBox.Text;
                    //clientAutoRestart = autoRestartCheckBox.Checked;

                    WriteClientXML(); // Saves all important client variables to an XML file stored next to the .exe.

                    Thread networkingThread = new Thread(StartNetworking); // Setup a seperate thread for networking.
                    networkingThread.Start();
                }
                else if (hostIPBox.Text == "")
                {
                    MessageBox.Show("You must enter a Host IP before trying to connect!", "No Host IP!");
                }
                if (hostPortBox.Value == 0)
                {
                    MessageBox.Show("You must enter a Host Port before trying to connect!", "No Host Port!");
                }
                if (clientPortBox.Value == 0)
                {
                    MessageBox.Show("You must enter a Client Port before trying to connect!", "No Client Port!");
                }
                if (usernameBox.Text == "")
                {
                    MessageBox.Show("You must enter a Username before trying to connect!", "No Username!");
                }
            }
            else // Disconnect
            {
                ClientNetworking.client.Shutdown("");
                clientStart = false;
                ClientNetworking.connected = false;
                ClientNetworking.fileTransferStarted = false;

                while (ClientNetworking.client.Status != Lidgren.Network.NetPeerStatus.NotRunning)
                {
                    Thread.Sleep(1);
                }

                // Change color of stop button to change states.
                clientStartStopButton.BackColor = Color.FromArgb(0, 192, 0);
                clientStartStopButton.Text = "Connect";

                hostIPBox.ReadOnly = false;
                hostPortBox.ReadOnly = false;
                hostPortBox.Enabled = true;
                clientPortBox.ReadOnly = false;
                clientPortBox.Enabled = true;
                directoryPathBox.ReadOnly = false;
                browseDirectoryButton.Enabled = true;
                usernameBox.ReadOnly = false;
                //autoRestartCheckBox.Enabled = true;
                UpdateFileTransferBar("", "", true);
                UpdateClientInfoBox("Client Stopped", false);
            }
        }


        private void browseDirectoryButton_Click(object sender, EventArgs e) // Event handler activated when the folder browser button is pressed. Retrieves folder directory with a dialog.
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
            this.clientToolTips.SetToolTip(this.directoryPathBox, directoryPathBox.Text);
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


        public void UpdateClientInfoBox(string info, bool clear)
        {
            if (clear == true)
            {
                clientInfoBox.Clear();
            }
            if (clientInfoBox.Text != "")
            {
                clientInfoBox.Text += "\n";
            }
            clientInfoBox.Text += info;
            clientInfoBox.SelectionStart = clientInfoBox.TextLength;
            clientInfoBox.ScrollToCaret();
        }


        public void UpdateFileTransferBar(string amountDownloaded, string totalSize, bool reset)
        {
            if (reset == false & ClientNetworking.fileTransferStarted == true)
            {
                clientToolTips.SetToolTip(saveTransferBar, "Save Transfer Progress: " + amountDownloaded + "kb/" + totalSize + "kb" + " (Download will exceed savefile size)");

                int saveTransferBarValue = Convert.ToInt32((Convert.ToDouble(amountDownloaded) / Convert.ToDouble(totalSize)) * 100);
                if (saveTransferBarValue > 100)
                {
                    saveTransferBarValue = 100;
                }
                saveTransferBar.Value = saveTransferBarValue;
            }
            else
            {
                saveTransferBar.Value = 0;
                clientToolTips.SetToolTip(saveTransferBar, "Save Transfer Progress");
            }
        }

        // Creates or updates an XML file with client settings.
        public void WriteClientXML()
        {
            XmlDocument clientXML = new XmlDocument();

            XmlNode baseNode;
            XmlNode clientNode;

            if (File.Exists(Directory.GetCurrentDirectory() + @"\Settings.xml")) // Load existing setting file and existing nodes.
            {
                clientXML.Load("Settings.xml");
                baseNode = clientXML.ChildNodes[0];
                clientNode = baseNode.ChildNodes[0];
            }
            else // If settings file doesn't exist, create it.
            {
                XmlNode hostNode; // Create the host node as well to fully setup the xml file.
                baseNode = clientXML.CreateElement("ClientAndHostSettings");
                clientNode = clientXML.CreateElement("Client");
                hostNode = clientXML.CreateElement("Host");

                clientXML.AppendChild(baseNode);
                baseNode.AppendChild(clientNode);
                baseNode.AppendChild(hostNode);
            }

            XmlAttribute hostIPAttribute = clientXML.CreateAttribute("HostIP");
            XmlAttribute hostPortAttribute = clientXML.CreateAttribute("HostPort");
            XmlAttribute clientPortAttribute = clientXML.CreateAttribute("ClientPort");
            XmlAttribute clientGamePathAttribute = clientXML.CreateAttribute("GamePath");
            XmlAttribute clientUsernameAttribute = clientXML.CreateAttribute("Username");
            XmlAttribute clientAutostartAttribute = clientXML.CreateAttribute("Autostart");

            hostIPAttribute.Value = hostIP;
            hostPortAttribute.Value = Convert.ToString(hostPort);
            clientPortAttribute.Value = Convert.ToString(clientPort);
            clientGamePathAttribute.Value = clientsavegameDirectory;
            clientUsernameAttribute.Value = clientUsername;
            clientAutostartAttribute.Value = Convert.ToString(clientAutoRestart);

            clientNode.Attributes.Append(hostIPAttribute);
            clientNode.Attributes.Append(hostPortAttribute);
            clientNode.Attributes.Append(clientPortAttribute);
            clientNode.Attributes.Append(clientGamePathAttribute);
            clientNode.Attributes.Append(clientUsernameAttribute);
            clientNode.Attributes.Append(clientAutostartAttribute);

            clientXML.Save("Settings.xml");
        }


        public void ReadClientXML() // Loads the client node from the XML document and sets all the text boxes to the loaded values.
        {
            XmlDocument clientXML = new XmlDocument();

            clientXML.Load("Settings.xml");

            XmlNodeList clientNodeList = clientXML.ChildNodes[0].ChildNodes; // Load the base node, then the 2 child nodes (Client, Host)

            if (clientNodeList.Item(0).Attributes.Count != 0) // If the client node actually has attributes, or is empty.
            {
                hostIPBox.Text = clientNodeList[0].Attributes[0].Value;
                hostPortBox.Value = Convert.ToDecimal(clientNodeList[0].Attributes[1].Value);
                clientPortBox.Value = Convert.ToDecimal(clientNodeList[0].Attributes[2].Value);
                directoryPathBox.Text = clientNodeList[0].Attributes[3].Value;
                usernameBox.Text = clientNodeList[0].Attributes[4].Value;
                //autoRestartCheckBox.Checked = Convert.ToBoolean(clientNodeList[0].Attributes[5].Value);
            }
            else
            {
                // If no client attributes were found, but the settings file was found, it will save client attributes the next time the client is initialized by setting noClient to true.
                noClient = true;
            }
        }


        public void StartNetworking() // Networking started by a seperate thread
        {
            ClientNetworking clientNetwork = new ClientNetworking(); // Start networking
        }
    }
}
