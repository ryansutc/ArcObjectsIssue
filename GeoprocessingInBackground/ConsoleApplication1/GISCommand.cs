using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geoprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.DataManagementTools;
using System.Threading;
using System.ComponentModel;
using System.IO;

namespace ConsoleApplication1
{
    public class GISCommand
    {

        public Task<string> DoGeoprocessingThing(CancellationToken cts, BackgroundWorker worker)
        {

            System.Object obj = "";
            Geoprocessor gp1 = new Geoprocessor();
            GeoProcessor gp = new GeoProcessor();
            gp1.OverwriteOutput = true;
            try
            {
                // ################################################
                // Working call to an existing ArcGIS Toolbox Tool:
                CreateFileGDB createFileGdb = new CreateFileGDB();
                createFileGdb.out_folder_path = "c:/temp";
                createFileGdb.out_name = "tmp.gdb";
                gp1.Execute(createFileGdb, null);
                // ###############################################

                // ###########################################
                // This process will fail if you've already started ArcObjects by opening the form UI dialog
                gp.AddToolbox(@"C:\temp\ArcObjectsIssue\HelloTest.tbx"); //path to your toolbox here!!!!!!!!!!!!!!!
                IVariantArray parameters = new VarArrayClass();
                parameters = new VarArrayClass();
                parameters.Add("Hello Geoprocessing Tool with Python Script!");
                gp.Execute("helloPython", parameters, null);
                cts.ThrowIfCancellationRequested();
                // ###########################################
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine(ReturnGpMessages(gp, obj)); //from a support library not shared here.

                throw new Exception("yeah things went south");
            }
            finally
            {
                gp = null;
                gp1 = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return null;
        }

        /// <summary>
        /// Provides convenience methods for dealing
        /// with Geoprocessor issues. (Override: using IGeoProcessor)
        /// </summary>
        public static string ReturnGpMessages(GeoProcessor gp, System.Object obj)
        {
            string message = "";
            if (gp.MessageCount > 0)
            {
                for (int i = 0; i <= gp.MessageCount - 1; i++)
                {
                    message += gp.GetMessage(i) + "\n";
                }
            }
            return message;
        }
    }
}
