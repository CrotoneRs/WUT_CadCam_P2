using System;
using System.Collections.Generic;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Diagnostics;

namespace LabCC
{
    /// <summary>
    /// Tesselation Maker:
    /// 
    ///     * Downloaded from SolidWorks API Documentation and applied to the problem
    /// </summary>
    public class TessellationHelper
    {
        public SldWorks swApp;
        private ModelDoc2 swModel;

        public TessellationHelper(SldWorks _swApp, ModelDoc2 _swModel)
        {
            swApp = _swApp;
            swModel = _swModel;
        }

        public List<Segment> Tessellate()
        {
            List<Segment> segments = new List<Segment>();
            PartDoc swPart = (PartDoc)swModel;

            System.Object vBodies;
            System.Array aBodies;

            /* Acquiring solid's bodies */
            vBodies = swPart.GetBodies2((int)swBodyType_e.swAllBodies, false);
            aBodies = (System.Array)vBodies;

            int iNumBodies = aBodies.Length;
            int iNumSolidBodies = 0;
            int iNumSheetBodies = 0;

            Body2 swBody;

            for (int b = 0; b < iNumBodies; b++)
            {
                swBody = (Body2)aBodies.GetValue(b);

                int nBodyType = swBody.GetType();

                if (nBodyType == (int)swBodyType_e.swSheetBody)
                    iNumSheetBodies++;

                if (nBodyType == (int)swBodyType_e.swSolidBody)
                {
                    iNumSolidBodies++;

                    Face2 swFace = null;
                    Tessellation swTessellation = null;

                    bool bResult = false;
                    swTessellation = (Tessellation)swBody.GetTessellation(null);

                    swTessellation.NeedFaceFacetMap = true;
                    swTessellation.NeedVertexParams = true;
                    swTessellation.NeedVertexNormal = true;
                    swTessellation.ImprovedQuality = true;

                    /* Proper Tesselation */
                    swTessellation.MatchType = (int)swTesselationMatchType_e.swTesselationMatchFacetTopology;
                    bResult = swTessellation.Tessellate();

                    int[] aFacetIds;
                    int iNumFacetIds;
                    int[] aFinIds;
                    int[] aVertexIds;
                    double[] aVertexCoords1;
                    double[] aVertexCoords2;

                    swFace = (Face2)swBody.GetFirstFace();

                    /* Getting all solid's triangles */
                    while (swFace != null)
                    {
                        aFacetIds = (int[])swTessellation.GetFaceFacets(swFace);

                        iNumFacetIds = aFacetIds.Length;

                        for (int iFacetIdIdx = 0; iFacetIdIdx < iNumFacetIds; iFacetIdIdx++)
                        {
                            aFinIds = (int[])swTessellation.GetFacetFins(aFacetIds[iFacetIdIdx]);

                            for (int iFinIdx = 0; iFinIdx < 3; iFinIdx++)
                            {
                                aVertexIds = (int[])swTessellation.GetFinVertices(aFinIds[iFinIdx]);

                                aVertexCoords1 = (double[])swTessellation.GetVertexPoint(aVertexIds[0]);
                                aVertexCoords2 = (double[])swTessellation.GetVertexPoint(aVertexIds[1]);

                                /* Creating segments */
                                segments.Add(new Segment(aVertexCoords1, aVertexCoords2));
                            }
                        }

                        swFace = (Face2)swFace.GetNextFace();
                    }
                }
            }

            return segments;
        }
    }
}
