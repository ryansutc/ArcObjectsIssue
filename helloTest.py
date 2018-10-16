'''
Simple test script for development
'''

import arcpy

msg = arcpy.GetParameterAsText(0)

print msg
arcpy.AddMessage("so the msg is: " + msg)

if msg == "":
    raise Exception("the parameter was lost in arcpy.GetParameterAsText")
else:
    arcpy.AddMessage("done!")