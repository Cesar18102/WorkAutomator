using System;

namespace WorkAutomatorLogic.Services
{
    internal class IntersectionService
    {
        private const double TRESHHOLD = 0.01f;

        private double GetLength((double x, double y) point1, (double x, double y) point2)
        {
            return (double)Math.Sqrt(Math.Pow(point1.x - point2.x, 2.0) + Math.Pow(point1.y - point2.y, 2.0));
        }

        private double GetScope((double x, double y) point1, (double x, double y) point2, (double x, double y) point3)
        {
            double len12 = GetLength(point1, point2);
            double len23 = GetLength(point2, point3);
            double len13 = GetLength(point1, point3);

            double p = (len12 + len13 + len23) / 2;

            return (double)Math.Sqrt(p * (p - len12) * (p - len13) * (p - len23));
        }

        private bool CheckInside((double x, double y) point, (double x, double y) point1, (double x, double y) point2, (double x, double y) point3)
        {
            double scope120 = GetScope(point, point1, point2);
            double scope130 = GetScope(point, point1, point3);
            double scope230 = GetScope(point, point2, point3);

            double totalScope = scope120 + scope130 + scope230;
            double scope123 = GetScope(point1, point2, point3);

            return Math.Abs(scope123 - totalScope) < TRESHHOLD;
        }

        public bool CheckInside((double x, double y) point, (double x, double y)[] points)
        {
            for(int i = 0; i < points.Length; ++i)
                for(int j = 0; j < points.Length; ++j)
                    for(int k = 0; k < points.Length; ++k)
                    {
                        if (points[i].x == points[j].x && points[j].x == points[k].x)
                            continue;

                        if (points[i].y == points[j].y && points[j].y == points[k].y)
                            continue;

                        if (CheckInside(point, points[i], points[j], points[k]))
                            return true;
                    }

            return false;
        }
    }
}
