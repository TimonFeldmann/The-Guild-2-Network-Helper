using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Guild_2_Network_Helper
{
    static class Entry
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SplashScreenForm()); // Runs the splashscreen for a few seconds, closes it, then gives the user a messagebox in the next line.
            Application.Run(new ChoiceMessageBox());
        }
    }
}
