using System;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Text;

namespace LabCC
{
    /// <summary>
    /// GradientCurve Class:
    /// 
    ///     * Manages the curve.
    /// </summary>
    public class GradientCurve
    {
        private List<Segment> curveSegments;

        public enum PointType
        {
            RANDOM, SPECIFIC
        }

        public GradientCurve()
        {
            curveSegments = new List<Segment>();
        }

        /// <summary>
        /// Creates the gradient curve.
        /// </summary>
        /// <param name="levels">All solid's levels.</param>
        /// <param name="swModel">Solid's model to drowing.</param>
        public void Make(List<Level> levels, ModelDoc2 swModel)
        {
            /* Getting starting point and segment */
            Segment segment = new Segment();
            Point point = GetStartingPoint(PointType.RANDOM, levels[0], out segment);

            /* Iterating through levels */
            for (int i = 0; i < levels.Count - 1; i++)
            {
                double distance = double.MaxValue;

                /* Getting perpendicular segment to one given */
                Segment perpendicular = segment.GetPerpendicular(point);
                perpendicular.SetEndY(levels[i + 1].GetLevel());

                /* Iterating through next level's segments */
                foreach (Segment nextSegment in levels[i + 1].GetSegments())
                    /* Determining if segments intersect - there's always only one proper segment (max two of them at all) */
                    if (perpendicular.IsIntersection(nextSegment) && nextSegment.IsIntersection(perpendicular))
                    {
                        /* Getting intersection point of segments */
                        Point p = perpendicular.GetIntersectionPoint(nextSegment);
                        p.y = levels[i + 1].GetLevel();

                        double currentDistance = point.Distance(p);

                        /* Determining if proper segment has been found */
                        if (currentDistance < distance)
                        {
                            distance = currentDistance;

                            /* Adding segment to gradient curve */
                            curveSegments.Add(new Segment(point, p));

                            segment = nextSegment;
                            point = p;
                        }
                    }
            }
        }

        /// <summary>
        /// Finds starting point for gradient curve.
        /// </summary>
        /// <param name="type">Random and Specific types allowed.</param>
        /// <param name="level">Level to search.</param>
        /// <param name="segment">(OUT) Found segment.</param>
        /// <returns>Found point.</returns>
        private Point GetStartingPoint(PointType type, Level level, out Segment segment)
        {
            switch (type)
            {
                case PointType.RANDOM:
                    {
                        Random random = new Random();
                        List<Segment> segments = level.GetSegments();
                        
                        segment = segments[random.Next(segments.Count)];

                        Point start = segment.GetStart();
                        Point end = segment.GetEnd();

                        double x = (start.x + end.x) / 2.0;
                        double y = (start.y + end.y) / 2.0;
                        double z = (start.z + end.z) / 2.0;

                        return new Point(x, y, z);
                    }
                case PointType.SPECIFIC:
                    {
                        /* Values taken from SolidWorks */
                        Point start = new Point(-0.02248, 0.02750, 0.00598);
                        Point end = new Point(-0.02177, 0.02750, 0.00826);

                        segment = new Segment(start, end);

                        double x = (start.x + end.x) / 2.0;
                        double y = (start.y + end.y) / 2.0;
                        double z = (start.z + end.z) / 2.0;

                        return new Point(x, y, z);
                    }
            }

            segment = Segment.None;
            return Point.None;
        }

        /// <summary>
        /// Drawing gradient curve.
        /// </summary>
        /// <param name="swModel">Solid's model.</param>
        public void Draw(ModelDoc2 swModel)
        {
            swModel.SetAddToDB(true);
            swModel.SetDisplayWhenAdded(false);
            swModel.Insert3DSketch2(false);

            for (int i = 0; i < curveSegments.Count - 1; i++)
            {
                Point start = curveSegments[i].GetStart();
                Point end = curveSegments[i].GetEnd();

                swModel.CreateLine2(start.x, start.y, start.z, end.x, end.y, end.z);
            }

            swModel.Insert3DSketch2(true);
            swModel.ClearSelection2(true);

            swModel.SetAddToDB(false);
            swModel.SetDisplayWhenAdded(true);
        }

        /// <summary>
        /// Drawing levels.
        /// </summary>
        /// <param name="swModel">Solid's model.</param>
        /// <param name="levels">Levels to draw.</param>
        public void DrawLevels(ModelDoc2 swModel, List<Level> levels)
        {
            swModel.SetAddToDB(true);
            swModel.SetDisplayWhenAdded(false);
            swModel.Insert3DSketch2(false);

            foreach (Level level in levels)
                foreach (Segment segment in level.GetSegments())
                    swModel.CreateLine2
                        (
                            segment.GetStart().x, segment.GetStart().y, segment.GetStart().z,
                            segment.GetEnd().x, segment.GetEnd().y, segment.GetEnd().z
                        );

            swModel.Insert3DSketch2(true);
            swModel.ClearSelection2(true);
            swModel.SetAddToDB(false);
            swModel.SetDisplayWhenAdded(true);
        }

        /// <summary>
        /// Drawing segments.
        /// </summary>
        /// <param name="swModel">Solid's model.</param>
        /// <param name="segment">Segment to draw.</param>
        private void DrawSegment(ModelDoc2 swModel, Segment segment)
        {
            swModel.SetAddToDB(true);
            swModel.SetDisplayWhenAdded(false);
            swModel.Insert3DSketch2(false);

            swModel.CreateLine2
                (
                segment.GetStart().x, segment.GetStart().y, segment.GetStart().z,
                segment.GetEnd().x, segment.GetEnd().y, segment.GetEnd().z
                );

            swModel.Insert3DSketch2(true);
            swModel.ClearSelection2(true);

            swModel.SetAddToDB(false);
            swModel.SetDisplayWhenAdded(true);
        }
    }
}
