# ArcObjectsIssue
arcObjects Background Geoprocessing Issue Sample

## Problem
Calling a custom python geoprocessing tool through the GeoProcessor Class in .NET in a background thread does not work when certain ArcObjects libraries have been initialized already in another thread. Instead it results in the supplied parameters appearing as empty to the called python script. This only occurs if the main thread (UI thread) implements some ArcObjects class instances. In the below sample, for example, the process will only fail if you click on the form button to open the GxDialog object (an ArcObjects class that starts an open file/folder dialog).
## Details: 
The sample is a stand-alone desktop app. It is a simple windows form front end .NET project and a GIS C# class in a console project that is run in a background thread. The design intent was to keep the UI responsive and “thin” as possible. 
The sample includes a call to normal ESRI geoprocessing tools to show that they will run without issue as will custom non-python (modelbuilder) tools. Additionally a Python tool in the new [python Geoprocessing Toolbox](http://pro.arcgis.com/en/pro-app/arcpy/geoprocessing_and_python/a-quick-tour-of-python-toolboxes.htm) seems also run fine (still testing). 
You can see that the Geoprocessing Tool itself is receiving the parameters by looking at the output window and by the fact that it does not throw an exception. Instead the empty values get passed to the python script which will crash wherever these empty values trigger an exception.

This behaviour has been tested in ArcGIS Desktop 10.5. 
