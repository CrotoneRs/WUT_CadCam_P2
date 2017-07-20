using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace LabCC
{
    /// <summary>
    /// Warsaw University of Technology:
    /// 
    ///     * CAD/CAM Laboratories
    ///         * Calculating a gradient curve of given solid
    /// </summary>
    public partial class SolidWorksMacro
    {
        public SldWorks swApp;

        /// <summary>
        /// The entry point of the program.
        /// </summary>
        public void Main()
        {
            /* Acquiring current document (with solid) */
            ModelDoc2 swModel = (ModelDoc2) swApp.ActivateDoc("Cone.SLDPRT");

            /* Tessetalion (getting all solid's triangles) */
            TessellationHelper tesselation = new TessellationHelper(swApp, swModel);
            List<Segment> segments = tesselation.Tessellate();

            /* Creating levels */
            LevelManager levelManager = new LevelManager();
            levelManager.CreateLevels(segments);

            /* Calculating gradient curve */
            GradientCurve gradientCurve = new GradientCurve();
            gradientCurve.Make(levelManager.GetLevels(), swModel);

            /* Drawing calculated curve and levels */
            gradientCurve.Draw(swModel);
            gradientCurve.DrawLevels(swModel, levelManager.GetLevels());
        }
    }
}


