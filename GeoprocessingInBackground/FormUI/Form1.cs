using ConsoleApplication1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using ESRI.ArcGIS.esriSystem;


namespace FormUI
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource cts;

        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;

            backgroundWorker1.DoWork +=
                new DoWorkEventHandler(BackgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            BackgroundWorker1_Completed);
            backgroundWorker1.ProgressChanged +=
                new ProgressChangedEventHandler(
            BackgroundWorker1_ProgressChanged);

            GetArcGISLicense();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            // Unpack the args sent from the main thread:

            GISCommand gisCommand = new GISCommand();

            cts = new CancellationTokenSource(); //instantiate cancellation token

            var asyncError = gisCommand.DoGeoprocessingThing(cts.Token, worker);
            if (asyncError.Status.ToString() == "Canceled") e.Cancel = true;
            if (asyncError.Exception != null) throw new Exception(asyncError.Exception.InnerException.Message);
            //**************************************************************

        }

        private void BackgroundWorker1_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("An Exception occurred: " + e.Error.ToString());
            }
            else if (e.Cancelled == true)
            {
                MessageBox.Show(this, "Canceled!");
            }
            else
            {
                MessageBox.Show(this, "Complete!");
            }
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                Console.WriteLine((string)e.UserState);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string newfile = Dialog.OpenShapeFileDialog2();  // Call a method to open a IGxDialog.
        }

        public static void GetArcGISLicense()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Desktop);
            IAoInitialize aoInitialize = new AoInitializeClass();
            esriLicenseStatus stat;
            // checks for Arcview License
            stat = aoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeBasic);
            // if not licensed - initialize license
            if (stat != esriLicenseStatus.esriLicenseCheckedOut)
            {
                stat = aoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeAdvanced);
            }
            if (stat != esriLicenseStatus.esriLicenseCheckedOut)
            {
                Console.WriteLine("Could not get an ArcGIS License");
                throw new Exception("Could not get an ArcGIS License");
            }
        }
    }
}
