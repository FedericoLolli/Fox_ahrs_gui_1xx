using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using x_IMU_IMU_and_AHRS_Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
namespace AHRSInterface
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            
           /* Form_3Dcuboid form_3DcuboidA = new Form_3Dcuboid();
            //Form_3Dcuboid form_3DcuboidA = new Form_3Dcuboid(new string[] { "RightInv.png", "LeftInv.png", "BackInv.png", "FrontInv.png", "TopInv.png", "BottomInv.png" });
            form_3DcuboidA.Text += " A";
            BackgroundWorker backgroundWorkerA = new BackgroundWorker();

            backgroundWorkerA.DoWork += new DoWorkEventHandler(delegate { form_3DcuboidA.ShowDialog(); });

            backgroundWorkerA.RunWorkerAsync();

            float[] num = new float[9] { 1, 0, 0, 1, 0, 0, 1, 0, 2 };

            form_3DcuboidA.RotationMatrix = num;*/

            Application.EnableVisualStyles();
            Application.Run(new AHRSInterface());
        }
    }
}
