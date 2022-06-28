using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Guild_2_Network_Helper
{
    public partial class ChoiceMessageBox : Form
    {
        public static bool isHost; // Variable that stores if the user is a client or the Host.
        public static hostForm hostForm; // The form opened if the user chooses to be a host.
        public static clientForm clientForm; // The form opened if the user chooses to be a client.
        public static Thread formThread; // For starting the client or host form.
        public static bool showClientHelp;
        public static bool showHostHelp;
        public delegate void HostClientDelegate(bool isHost); // Delegate that assigns the user either Host or Client.
        public event HostClientDelegate choiceMade; // Event that triggers the user being made either a client or host.

        public ChoiceMessageBox()
        {
            choiceMade += ChoiceMessageBox_choiceMade; // Subscribe the event to the event handler
            InitializeComponent();
        }

        private void clientButton_Click(object sender, EventArgs e)
        {
            // Gives the user an option of becoming a client or host. After the user makes a choice, the messagebox is closed and a new thread is started that handles either the client or
            // host forms.
            this.Close();
            clientForm = new clientForm();
            isHost = false;
            choiceMade(isHost);
        }

        private void hostButton_Click(object sender, EventArgs e)
        {
            // Same as above.
            this.Close();
            hostForm = new hostForm();
            isHost = true;
            choiceMade(isHost);
        }

        [STAThread] // Needs to be STA because of Windows Forms requirements.
        public void ChoiceMessageBox_choiceMade(bool isHost)
        {
            if (isHost == true)
            {
                formThread = new Thread(StartHost); // Start a new thread for the new form. The messagebox thread should get discarded.
                formThread.SetApartmentState(ApartmentState.STA);
                formThread.Start();
            }
            else
            {
                formThread = new Thread(StartClient); // Same as above.
                formThread.SetApartmentState(ApartmentState.STA);
                formThread.Start();
            }
        }

        // Methods for the previously started threads to execute the forms under.
        public void StartHost()
        {
            Application.Run(hostForm);
        }

        public void StartClient()
        {
            Application.Run(clientForm);
        }

        private void clientHelpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Client settings are saved to an XML file next to the .exe on client start.\n\nWhen the host tells you to connect after filling out all of your client settings, press 'Connect'. After you are connected to the host, you are free to join their Guild 2: Renaissance game.\n\nThis is all you need to do, if the game goes out of sync, this program will display additional information.\n\nOpen the host help to find out more.", "Client Info", MessageBoxButtons.OK);
        }

        private void hostHelpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Host settings are saved to an XML file next to the .exe on host start.\n\nWhen you have filled out all the host settings, press 'Start Server'.\n\nAfter the server is fully started, clients will be able to request to connect to you. Once everyone has connected, simply start your game and play.\n\nIf the game goes out of sync, press the 'Out of Sync' button that will appear when the server starts. This program will then begin to send the newest savefile you have to all of the people you're connected with.\n\nAfter the file transfer is complete, simply re-host the game and tell everyone to connect. This entire process (depending on your bandwidth and the amount of people you're playing with) should take 1 - 5 minutes.\n\nNOTE: Amount uploaded may exceed file size, this is normal. Simply let the filetransfer continue until all clients have received the savefile.", "Host Info", MessageBoxButtons.OK);
        }
    }
}
