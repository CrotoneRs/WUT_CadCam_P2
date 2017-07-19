using System;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Text;

namespace LabCC
{
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

        public void Make(List<Level> levels, ModelDoc2 swModel)
        {
            Segment segment = new Segment();
            Point point = GetStartingPoint(PointType.RANDOM, levels[0], out segment);

            for (int i = 0; i < levels.Count - 1; i++)
            {
                double distance = double.MaxValue;

                Segment perpendicular = segment.GetPerpendicular(point);
                perpendicular.SetEndY(levels[i + 1].GetLevel());

                foreach (Segment nextSegment in levels[i + 1].GetSegments())
                    if (perpendicular.IsIntersection(nextSegment) && nextSegment.IsIntersection(perpendicular))
                    {
                        Point p = perpendicular.GetIntersectionPoint(nextSegment);
                        p.y = levels[i + 1].GetLevel();

                        double currentDistance = point.Distance(p);

                        if (currentDistance < distance)
                        {
                            distance = currentDistance;
                            curveSegments.Add(new Segment(point, p));

                            segment = nextSegment;
                            point = p;
                        }
                    }
            }
        }

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
