using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace LabCC
{
    public partial class SolidWorksMacro
    {
        public SldWorks swApp;

        public void Main()
        {
            ModelDoc2 swModel = (ModelDoc2) swApp.ActivateDoc("Cone.SLDPRT");

            TessellationHelper tesselation = new TessellationHelper(swApp, swModel);
            List<Segment> segments = tesselation.Tessellate();

            LevelManager levelManager = new LevelManager();
            levelManager.CreateLevels(segments);

            GradientCurve gradientCurve = new GradientCurve();
            gradientCurve.Make(levelManager.GetLevels(), swModel);

            gradientCurve.Draw(swModel);
            gradientCurve.DrawLevels(swModel, levelManager.GetLevels());
        }
    }
}


