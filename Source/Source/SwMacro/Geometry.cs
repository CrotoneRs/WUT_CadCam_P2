using System;
using System.Collections.Generic;
using System.Text;

namespace LabCC
{
    /// <summary>
    /// Tuple Structure to storing two related values.
    /// </summary>
    public struct Tuple
    {
        public double item1;
        public double item2;

        public Tuple(double i1, double i2)
        {
            item1 = i1;
            item2 = i2;
        }
    }

    /// <summary>
    /// Point Structure.
    /// </summary>
    public struct Point
    {
        public double x;
        public double y;
        public double z;

        public static Point None = new Point(double.MaxValue, double.MaxValue, double.MaxValue);

        public Point(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public static Point operator - (Point p1, Point p2)
        {
            return new Point(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);
        }

        public static Point operator + (Point p1, Point p2)
        {
            return new Point(p1.x + p2.x, p1.y + p2.y, p1.z + p2.z);
        }

        public static explicit operator Point2D(Point p)
        {
            return new Point2D(p.x, p.z);
        }

        /// <summary>
        /// Calculates Euclidean distance between two points in 3D.
        /// </summary>
        /// <param name="p">Other point.</param>
        /// <returns>Calculated distance.</returns>
        public double Distance(Point p)
        {
            return Math.Sqrt((p.x - x) * (p.x - x) + (p.y - y) * (p.y - y) + (p.z - z) * (p.z - z));
        }
    }

    /// <summary>
    /// Point2D Structure.
    /// </summary>
    public struct Point2D
    {
        public double x;
        public double y;

        public static Point2D None = new Point2D(double.MaxValue, double.MaxValue);

        public Point2D(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

        public static Point2D operator -(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.x - p2.x, p1.y - p2.y);
        }

        public static Point2D operator +(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.x + p2.x, p1.y + p2.y);
        }
    }

    /// <summary>
    /// Vector3 Structure.
    /// </summary>
    public struct Vector3
    {
        public double x;
        public double y;
        public double z;

        public Vector3(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        /// <summary>
        /// Calculates DotProduct of two vectors in 3D.
        /// </summary>
        /// <param name="p1">Vector 1.</param>
        /// <param name="p2">Vector 2.</param>
        /// <returns>Calculated DotProduct.</returns>
        public static double DotProduct(Vector3 p1, Vector3 p2)
        {
            return p1.x * p2.x + p1.y * p2.y + p1.z * p2.z;
        }

        /// <summary>
        /// Calculates CrossProduct of two vectors in 3D.
        /// </summary>
        /// <param name="v1">Vector 1.</param>
        /// <param name="v2">Vector 2.</param>
        /// <returns>Calculated CrossProduct.</returns>
        public static Vector3 CrossProduct(Vector3 v1, Vector3 v2)
        {
            double x = v1.y * v2.z - v2.y * v1.z;
            double y = -1 * (v1.x * v2.z - v2.x * v1.z);
            double z = v1.x * v2.y - v2.x * v1.y;

            return new Vector3(x, y, z);
        }

        public static Vector3 operator *(double alfa, Vector3 v)
        {
            return new Vector3(alfa * v.x, alfa * v.y, alfa * v.z);
        }

        public static implicit operator Point(Vector3 v)
        {
            return new Point(v.x, v.y, v.z);
        }
    }

    /// <summary>
    /// Vector2 Structure.
    /// </summary>
    public struct Vector2
    { 
        public double x;
        public double y;

        public Vector2(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

        public static Vector2 operator * (double alfa, Vector2 v)
        {
            return new Vector2(alfa * v.x, alfa * v.y);
        }

        public static implicit operator Point2D(Vector2 v)
        {
            return new Point2D(v.x, v.y);
        }
    }

    /// <summary>
    /// Segment Structure.
    /// </summary>
    public struct Segment
    {
        private Point start;
        private Point end;

        /* Static value to represent NullSegment */
        public static Segment None = new Segment(Point.None, Point.None);

        public Segment(Point _start, Point _end)
        {
            start = _start;
            end = _end;
        }

        public Segment(double[] _start, double[] _end)
        {
            start = new Point(_start[0], _start[1], _start[2]);
            end = new Point(_end[0], _end[1], _end[2]);
        }

        public Point GetStart()
        {
            return start;
        }

        public Point GetEnd()
        {
            return end;
        }

        public void SetEndY(double y)
        {
            end.y = y;
        }

        /// <summary>
        /// Indicates that two segments are intersected.
        /// Function checks if two points of other segment are placed on different sides of current segment.
        /// </summary>
        /// <param name="segment">Other segment.</param>
        /// <returns>True if intersection exists. False if not.</returns>
        public bool IsIntersection(Segment segment)
        {
            Point p1 = GetStart();
            Point p2 = GetEnd();

            Point start = segment.GetStart();
            Point end = segment.GetEnd();

            /* Calculating values indicating (same) / (not same) side od segment */
            double p1Site = (end.x - start.x) * (p1.z - start.z) - (p1.x - start.x) * (end.z - start.z);
            double p2Site = (end.x - start.x) * (p2.z - start.z) - (p2.x - start.x) * (end.z - start.z);

            if (p1Site > 0 && p2Site > 0)
                return false;

            if (p1Site < 0 && p2Site < 0)
                return false;

            if (p1Site != 0 && p2Site == 0)
                return false;

            if (p1Site == 0 && p2Site != 0)
                return false;

            if (p1Site == 0 && p2Site == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Calculates the intersection point of two segments.
        /// </summary>
        /// <param name="segment">Segment to intersect.</param>
        /// <returns>Intersection Point.</returns>
        public Point GetIntersectionPoint(Segment segment)
        {
            Point2D d1 = new Point2D(GetEnd().x - GetStart().x, GetEnd().z - GetStart().z);
            Point2D d2 = new Point2D(segment.GetEnd().x - segment.GetStart().x, segment.GetEnd().z - segment.GetStart().z);

            double dot = (d1.x * d2.y) - (d1.y * d2.x);

            Point2D c = new Point2D(segment.GetStart().x - GetStart().x, segment.GetStart().z - GetStart().z);

            double t = (c.x * d2.y - c.y * d2.x) / dot;

            return new Point(GetStart().x + (t * d1.x), double.MaxValue, GetStart().z + (t * d1.y));
        }

        /// <summary>
        /// Calculates the perpendicular segment on given point.
        /// </summary>
        /// <param name="point">Point where created segment must intersect existing.</param>
        /// <returns>Perpendicular Segment.</returns>
        public Segment GetPerpendicular(Point point)
        {
            Point start = GetPerpendicularPart(GetStart(), GetEnd(), point);
            Point end = GetPerpendicularPart(GetEnd(), GetStart(), point);

            return new Segment(start, end);
        }

        /// <summary>
        /// Calculates one-sited perpendicular segment.
        /// </summary>
        /// <param name="start">Start point of segment.</param>
        /// <param name="end">End point of segment.</param>
        /// <param name="point">Point where created segment must intersect existing.</param>
        /// <returns>The other point of one-sided perpendicular segment. First is third parameter.</returns>
        private Point GetPerpendicularPart(Point start, Point end, Point point)
        {
            Vector2 vectorSegment = new Vector2(end.x - start.x, end.z - start.z);
            Vector2 vectorToPoint = new Vector2(-1 * vectorSegment.y, vectorSegment.x);

            Point2D second = (Point2D)point + (15 * vectorToPoint);

            return new Point(second.x, double.MaxValue, second.y);
        }
    }

    /// <summary>
    /// Level Structure.
    /// </summary>
    public struct Level
    {
        private double currentLevel;
        private List<Segment> segments;

        public Level(double _level, List<Segment> _segments)
        {
            currentLevel = _level;
            segments = _segments;
        }

        public double GetLevel()
        {
            return currentLevel;
        }

        public List<Segment> GetSegments()
        {
            return segments;
        }
    }

    /// <summary>
    /// LevelManager Class:
    /// 
    ///     * Manages the levels.
    /// </summary>
    public class LevelManager
    {
        private List<Level> levels;

        public LevelManager()
        {
            levels = new List<Level>();
        }

        public List<Level> GetLevels()
        {
            return levels;
        }

        /// <summary>
        /// Creates levels based on solid's triangle's segments.
        /// </summary>
        /// <param name="segments">Solid's segments.</param>
        public void CreateLevels(List<Segment> segments)
        {
            Tuple minMax = GetHeigthLimits(segments);
            List<Plane> planes = GetLevelsPlanes(minMax);

            foreach (Plane plane in planes)
            {
                List<Point> levelPoints = new List<Point>();

                foreach (Segment segment in segments)
                {
                    Point intersectionPoint = Point.None;

                    if(plane.GetIntersectionPoint(segment, out intersectionPoint) == Plane.IntersectionType.INTERSECT)
                        levelPoints.Add(intersectionPoint);
                }

                levels.Add(new Level(plane.p2.y, GetLevelSegments(levelPoints)));
            }

            levels.Reverse();
        }

        /// <summary>
        /// Calculates heigth's limits for levels.
        /// </summary>
        /// <param name="segments">Solid's segments.</param>
        /// <returns>Tuple with minimum and maximum value of level.</returns>
        private Tuple GetHeigthLimits(List<Segment> segments)
        {
            double minimum = double.MaxValue;
            double maximum = double.MinValue;

            foreach (Segment segment in segments)
            {
                if (segment.GetStart().y > maximum)
                    maximum = segment.GetStart().y;

                if (segment.GetStart().y < minimum)
                    minimum = segment.GetStart().y;

                if (segment.GetEnd().y > maximum)
                    maximum = segment.GetEnd().y;

                if (segment.GetEnd().y < minimum)
                    minimum = segment.GetEnd().y;
            }

            return new Tuple(minimum + 0.00005, maximum);
        }

        /// <summary>
        /// Calculates level's planes.
        /// </summary>
        /// <param name="minMax">Tuple with minimum and maximum value of level.</param>
        /// <returns>List of level's planes.</returns>
        private List<Plane> GetLevelsPlanes(Tuple minMax)
        {
            List<Plane> resultLevels = new List<Plane>();

            int minDecimalPart = (int)Math.Round(1000.0 * minMax.item1);
            int maxDecimalPart = (int)Math.Round(1000.0 * minMax.item2);

            double step = 0.0015;
            double levelsCount = (maxDecimalPart - minDecimalPart) / (1000 * step);

            for (int i = 0; i < levelsCount; i++)
            {
                Point p1 = new Point(-50, minMax.item1 + i * step, -50);
                Point p2 = new Point(-50, minMax.item1 + i * step, 50);
                Point p3 = new Point(50, minMax.item1 + i * step, 50);

                resultLevels.Add(new Plane(p1, p2, p3));
            }

            return resultLevels;
        }

        /// <summary>
        /// Calculates level's segments based on intersection points.
        /// Points are calculated by intersection solid's segments with levels.
        /// </summary>
        /// <param name="levelPoint">Points to create segments.</param>
        /// <returns>List of level segments.</returns>
        private List<Segment> GetLevelSegments(List<Point> levelPoint)
        {
            List<Segment> levelSegments = new List<Segment>();

            for (int i = 0; i < levelPoint.Count - 1; i += 2)
                levelSegments.Add(new Segment(levelPoint[i], levelPoint[i + 1]));

            return levelSegments;
        }
    }

    /// <summary>
    /// Plane Structure.
    /// </summary>
    public struct Plane
    {
        public Point p1;
        public Point p2;
        public Point p3;

        public enum IntersectionType
        {
            PARALLEL, INTERSECT, NOINTERSECTION
        }

        public Plane(Point _p1, Point _p2, Point _p3)
        {
            p1 = _p1;
            p2 = _p2;
            p3 = _p3;
        }

        /// <summary>
        /// Indicates that current plane intersect given segment.
        /// </summary>
        /// <param name="segment">Givem segment.</param>
        /// <returns>True if intersection exists. False if not.</returns>
        public bool IsIntersection(Segment segment)
        {
            if(segment.GetStart().y > p2.y && segment.GetEnd().y < p2.y)
                return true;

            if (segment.GetStart().y < p2.y && segment.GetEnd().y > p2.y)
                return true;

            return false;
        }

        /// <summary>
        ///  Calculates the intersection point of current plane and given segment. 
        /// </summary>
        /// <param name="segment">Given segment.</param>
        /// <param name="intersectionPoint">(OUT) Intersection point.</param>
        /// <returns>IntersectionType.</returns>
        public IntersectionType GetIntersectionPoint(Segment segment, out Point intersectionPoint)
        {
            intersectionPoint = Point.None;

            Vector3 v1 = new Vector3(p2.x - p1.x, p2.y - p1.y, p2.z - p1.z);
            Vector3 v2 = new Vector3(p2.x - p3.x, p2.y - p3.y, p2.z - p3.z);

            Vector3 planeNormal = Vector3.CrossProduct(v1, v2);

            Point s = segment.GetStart();
            Point e = segment.GetEnd();

            Vector3 u = new Vector3(e.x - s.x, e.y - s.y, e.z - s.z);
            Vector3 w = new Vector3(s.x - p2.x, s.y - p2.y, s.z - p2.z);

            double D = Vector3.DotProduct(planeNormal, u);
            double N = -1 * Vector3.DotProduct(planeNormal, w);

            if (Math.Abs(D) < 10e-6)
                if (N == 0) return IntersectionType.PARALLEL;
                else return IntersectionType.NOINTERSECTION;

            double sI = N / D;

            if (sI < 0 || sI > 1)
                return IntersectionType.NOINTERSECTION;

            intersectionPoint = s + sI * u;

            return IntersectionType.INTERSECT;
        }
    }
}
