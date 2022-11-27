using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPRP.Models.Maps
{
    /// <summary>
    /// Simple representation of a x/y coordinate
    /// </summary>
    public class PointD
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointD"/> class.
        /// </summary>
        /// <param name="x">The X point.</param>
        /// <param name="y">The Y point.</param>
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets or sets the X point.
        /// </summary>
        /// <value>The X point.</value>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y point.
        /// </summary>
        /// <value>The Y point.</value>
        public double Y { get; set; }
        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is PointD)
            {
                PointD p = (PointD)obj;
                return X == p.X && Y == p.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            string code = string.Format("{0}_{1}", X, Y);
            return code.GetHashCode();
        }
    }

    public class Utility
    {

        /// <summary>
        /// Uses the Douglas Peucker algorithim to reduce the number of points.
        /// </summary>
        /// <param name="Points">The points.</param>
        /// <param name="Tolerance">The tolerance.</param>
        /// <returns></returns>
        public static List<PointD> DouglasPeuckerReduction(List<PointD> Points, double Tolerance)
        {

            if (Points == null || Points.Count < 3)
                return Points;

            int firstPoint = 0;
            int lastPoint = Points.Count - 1;
            List<int> pointIndexsToKeep = new List<int>();

            //Add the first and last index to the keepers
            pointIndexsToKeep.Add(firstPoint);
            pointIndexsToKeep.Add(lastPoint);


            //The first and the last point can not be the same
            while (Points[firstPoint].Equals(Points[lastPoint]))
            {
                lastPoint--;
            }

            DouglasPeuckerReduction(Points, firstPoint, lastPoint, Tolerance, ref pointIndexsToKeep);

            List<PointD> returnPoints = new List<PointD>();
            pointIndexsToKeep.Sort();
            foreach (Int32 index in pointIndexsToKeep)
            {
                returnPoints.Add(Points[index]);
            }

            return returnPoints;
        }

        /// <summary>
        /// Douglases the peucker reduction.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="firstPoint">The first point.</param>
        /// <param name="lastPoint">The last point.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <param name="pointIndexsToKeep">The point indexs to keep.</param>
        private static void DouglasPeuckerReduction(List<PointD> points, 
            int firstPoint, int lastPoint, double tolerance, 
            ref List<int> pointIndexsToKeep)
        {
            double maxDistance = 0;
            int indexFarthest = 0;

            for (var index = firstPoint; index < lastPoint; index++)
            {
                double distance = PerpendicularDistance(points[firstPoint], points[lastPoint], points[index]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    indexFarthest = index;
                }
            }

            if (maxDistance > tolerance && indexFarthest != 0)
            {
                //Add the largest point that exceeds the tolerance
                pointIndexsToKeep.Add(indexFarthest);

                DouglasPeuckerReduction(points, firstPoint, indexFarthest, tolerance, ref pointIndexsToKeep);
                DouglasPeuckerReduction(points, indexFarthest, lastPoint, tolerance, ref pointIndexsToKeep);
            }
        }

        /// <summary>
        /// The distance of a point from a line made from point1 and point2.
        /// </summary>
        /// <param name="pt1">The PT1.</param>
        /// <param name="pt2">The PT2.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public static Double PerpendicularDistance(PointD Point1, PointD Point2, PointD Point)
        {
            //Area = |(1/2)(x1y2 + x2y3 + x3y1 - x2y1 - x3y2 - x1y3)|   *Area of triangle
            //Base = √((x1-x2)²+(x1-x2)²)                               *Base of Triangle*
            //Area = .5*Base*H                                          *Solve for height
            //Height = Area/.5/Base

            double area = Math.Abs(.5 * (Point1.X * Point2.Y + Point2.X * Point.Y + Point.X * Point1.Y - Point2.X * Point1.Y - Point.X * Point2.Y - Point1.X * Point.Y));
            double bottom = Math.Sqrt(Math.Pow(Point1.X - Point2.X, 2) + Math.Pow(Point1.Y - Point2.Y, 2));
            double height = area / bottom * 2;

            return height;

            //Another option
            //Double A = Point.X - Point1.X;
            //Double B = Point.Y - Point1.Y;
            //Double C = Point2.X - Point1.X;
            //Double D = Point2.Y - Point1.Y;

            //Double dot = A * C + B * D;
            //Double len_sq = C * C + D * D;
            //Double param = dot / len_sq;

            //Double xx, yy;

            //if (param < 0)
            //{
            //    xx = Point1.X;
            //    yy = Point1.Y;
            //}
            //else if (param > 1)
            //{
            //    xx = Point2.X;
            //    yy = Point2.Y;
            //}
            //else
            //{
            //    xx = Point1.X + param * C;
            //    yy = Point1.Y + param * D;
            //}

            //Double d = DistanceBetweenOn2DPlane(Point, new Point(xx, yy));

        }
    }
}
