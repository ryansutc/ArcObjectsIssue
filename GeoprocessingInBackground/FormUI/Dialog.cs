using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.CatalogUI;

namespace FormUI
{
    internal class Dialog
    {
        /// <summary>
        /// Opens an ArcGIS Style Dialog box instead of windows style one for shapefiles.
        /// </summary>
        public static string OpenShapeFileDialog2()
        {
            IGxDialog pGxDialog = new GxDialogClass();
            IGxObjectFilter pFilter = new GxFilterShapefilesClass();
            IGxObjectFilter pFilter2 = new GxFilterSDEFeatureClasses();
            IGxObjectFilterCollection pFilterCollection;
            IEnumGxObject pEnumGx;

            pFilterCollection = pGxDialog as IGxObjectFilterCollection;
            pFilterCollection.AddFilter(pFilter, true);
            pFilterCollection.AddFilter(pFilter2, true);
            pGxDialog.Title = "Browse Data";

            if ((bool)pGxDialog.DoModalOpen(0, out pEnumGx))
            {
                IGxObject pGxObject;
                pGxObject = pEnumGx.Next() as IGxObject;

                if (pGxObject is GxDataset)
                {
                    var path = pGxObject.FullName;
                    // Just in case releasing the com objects might help the issue:
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pGxObject);
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pGxDialog);
                    return path;
                }
            }
            return null;
        }
    }
}
