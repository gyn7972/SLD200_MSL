using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace QMC.Common.VisionPart
{
    public class CirclePoints
    {
        private Dictionary<int, (int MinY, int MaxY)> pointsDictionary;

        public CirclePoints(List<Point> points)
        {
            pointsDictionary = new Dictionary<int, (int MinY, int MaxY)>();

            foreach (var point in points)
            {
                if (pointsDictionary.ContainsKey(point.X))
                {
                    var currentRange = pointsDictionary[point.X];
                    pointsDictionary[point.X] = (Math.Min(currentRange.MinY, point.Y), Math.Max(currentRange.MaxY, point.Y));
                }
                else
                {
                    pointsDictionary[point.X] = (point.Y, point.Y);
                }
            }
        }

        public bool IsPointInRange(Point point)
        {
            if (pointsDictionary.ContainsKey(point.X))
            {
                var range = pointsDictionary[point.X];
                return point.Y >= range.MinY && point.Y <= range.MaxY;
            }
            return false;
        }

        public static List<Point> GenerateCirclePoints(int radius)
        {
            List<Point> points = new List<Point>();

            for (int x = -radius; x <= radius; x++)
            {
                int y = (int)Math.Sqrt(radius * radius - x * x);
                points.Add(new Point(x, y));
                points.Add(new Point(x, -y));
            }

            return points;
        }
    }
}

