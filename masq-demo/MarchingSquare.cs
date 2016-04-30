using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace oprCourseSoloviev
{
    public class MarchingSquare
    {
        bool border = false;
        int Z;
        int N;
        double[,] arr;
        int startx = 0, starty = 0;
        List<PointF> res;
        enum dir
        {
            None,
            Up,
            Left,
            Down,
            Right
        }
        dir prevStep;
        dir nextStep;
        public MarchingSquare(int n)
        {
            N = n;
        }
        void findstartpos()
        {
            for (int y = 0; y < N; y++)
                for (int x = 0; x < N; x++)
                    if (arr[x, y] < Z)
                    {
                        startx = x;
                        starty = y;
                        return;
                    }
        }
        bool findstartpos(int x0, int y0)
        {
            bool contain = false;
            for (int y = y0; y < N; y++)
                for (int x = x0; x < N; x++)
                    if (arr[x, y] < Z)
                    {
                        for (int i = 0; i < res.Count; i++)
                            if (res[i].X == x && res[i].Y == y)
                            {
                                contain = true;
                                break;
                            }
                        if (!contain)
                        {
                            startx = x;
                            starty = y;
                            return true;
                        }
                    }
            return false;
        }
        bool check(int x, int y)
        {
            if (x == N - 1 || y == N - 1 || x == 0 || y == 0) border = true;
            if (x < 0 || y < 0 || x >= N || y >= N) return false;
            if (arr[x, y] < Z) return true;
            return false;
        }
        void step(int x, int y)
        {
            bool ul = check(x - 1, y - 1);
            bool ur = check(x, y - 1);
            bool dl = check(x - 1, y);
            bool dr = check(x, y);
            prevStep = nextStep;
            int state = 0;
            if (ul) state |= 1;
            if (ur) state |= 2;
            if (dl) state |= 4;
            if (dr) state |= 8;
            switch (state)
            {
                case 1: nextStep = dir.Down; break;
                case 2: nextStep = dir.Right; break;
                case 3: nextStep = dir.Right; break;
                case 4: nextStep = dir.Left; break;
                case 5: nextStep = dir.Down; break;
                case 6:
                    if (prevStep == dir.Down)
                    {
                        nextStep = dir.Left;
                    }
                    else
                    {
                        nextStep = dir.Right;
                    }
                    break;
                case 7: nextStep = dir.Right; break;
                case 8: nextStep = dir.Up; break;
                case 9:
                    if (prevStep == dir.Right)
                    {
                        nextStep = dir.Down;
                    }
                    else
                    {
                        nextStep = dir.Up;
                    }
                    break;
                case 10: nextStep = dir.Up; break;
                case 11: nextStep = dir.Up; break;
                case 12: nextStep = dir.Left; break;
                case 13: nextStep = dir.Down; break;
                case 14: nextStep = dir.Left; break;
                default:
                    nextStep = dir.None;
                    break;
            }

        }
        public PointF[] get(double[,] a, int z, bool ip)
        {
            border = false;
            arr = a;
            Z = z;
            startx = starty = 0;

            nextStep = prevStep = dir.Left;
            res = new List<PointF>();

            bool first = true;
            PointF dp1 = new PointF();
            double x0 = -N / 2;
            double y0 = -N / 2;

            findstartpos();
            int x = startx;
            int y = starty;

            do
            {
                step(x, y);

                if (x > 0 && x < N && y > 0 && y < N)
                {
                    int dx = 0, dy = 0;
                    switch (prevStep)
                    {
                        case dir.Down: dy = 1; break;
                        case dir.Left: dx = 1; break;
                        case dir.Up: dy = -1; break;
                        case dir.Right: dx = -1; break;
                        default: break;
                    }

                    double X = x0 + x;
                    double Y = y0 + y;
                    if (first)
                    {
                        double tx = X, ty = Y;
                        if (ip) ty = y0 + y + (Z - a[x, y - 1]) / (a[x, y] - a[x, y - 1]) - 1;
                        dp1 = new PointF((float)tx, (float)ty);
                        first = false;
                    }

                    if (ip) //ip - interpolation
                    {
                        if (dx != 0 && prevStep == nextStep) Y = y0 + y + (Z - a[x, y - 1]) / (a[x, y] - a[x, y - 1]) - 1;
                        if (dy != 0 && prevStep == nextStep) X = x0 + x + (Z - a[x - 1, y]) / (a[x, y] - a[x - 1, y]) - 1;

                        if (nextStep == dir.Down && prevStep == dir.Left) Y = y0 + y + (Z - a[x, y - 1]) / (a[x, y] - a[x, y - 1]) - 1;
                        if (nextStep == dir.Down && prevStep == dir.Right) X = x0 + x + (Z - a[x - 1, y]) / (a[x, y] - a[x - 1, y]) - 1;
                        if (nextStep == dir.Left && prevStep == dir.Down) X = x0 + x + (Z - a[x - 1, y]) / (a[x, y] - a[x - 1, y]) - 1;
                        if (nextStep == dir.Left && prevStep == dir.Up) X = x0 + x + (Z - a[x - 1, y]) / (a[x, y] - a[x - 1, y]) - 1;
                        if (nextStep == dir.Up && prevStep == dir.Right) X = x0 + x + (Z - a[x - 1, y]) / (a[x, y] - a[x - 1, y]) - 1;
                        if (nextStep == dir.Up && prevStep == dir.Left) X = x0 + x + (Z - a[x - 1, y]) / (a[x, y] - a[x - 1, y]) - 1;
                        if (nextStep == dir.Right && prevStep == dir.Up) Y = y0 + y + (Z - a[x, y - 1]) / (a[x, y] - a[x, y - 1]) - 1;
                        if (nextStep == dir.Right && prevStep == dir.Down) X = x0 + x + (Z - a[x - 1, y]) / (a[x, y] - a[x - 1, y]) - 1;

                        if (!(nextStep == dir.Down && prevStep == dir.Right) && !(nextStep == dir.Left && prevStep == dir.Up))
                            res.Add(new PointF((float)X, (float)Y));

                    }
                    else res.Add(new PointF((float)X, (float)Y));
                }

                switch (nextStep)
                {
                    case dir.Down: y--; break;
                    case dir.Left: x--; break;
                    case dir.Up: y++; break;
                    case dir.Right: x++; break;
                    default: break;
                }
            } while (x != startx || y != starty);

            if (!border)
            {
                if (!first) res.Add(dp1);
                if (res.Capacity > 0) res.Add(res[0]);
            }

            return res.ToArray();
        }
    }
}
