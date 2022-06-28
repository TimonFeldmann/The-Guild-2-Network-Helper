using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Guild_2_Network_Helper
{
    public partial class SplashScreenForm : Form
    {
        Timer splashTimer;
        public SplashScreenForm()
        {
            InitializeComponent();
            this.Shown += SplashScreen_Shown;
        }

        private void SplashScreen_Shown(object sender, EventArgs e)
        {
            splashTimer = new Timer();
            splashTimer.Interval = 3000;
            splashTimer.Tick += SplashTimer_Tick;
            splashTimer.Start();
        }

        private void SplashTimer_Tick(object sender, EventArgs e)
        {
            splashTimer.Stop();
            this.Close();
        }
    }
}
