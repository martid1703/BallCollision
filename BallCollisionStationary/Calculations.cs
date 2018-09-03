using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallCollisionStationary
{
  public class Calculations
  {
    // NOT FINISHED!
    public  Array[] Jugged_to_OneDim_Array(Array [][] inp)
    {
      int size=0;
      for (int i = 0; i < inp.Length; i++)
      {
        for (int j = 0; j < inp[i].Length; j++)
        {
          size++;
        }
      }
      return null;
    }
    // NOT TESTED! WILL IT WORK FOR ANY GIVEN ARRAY, SAY OfType.'LABEL'?
    public  Array[] TwoDim_to_OneDim_Array(Array [,] inp)
    {
      int n=0;

      Array [] ans=new Array[n];
      for (int i = 0; i < inp.GetLength(0); i++)
			{
        for (int j = 0; j < inp.GetLength(1); j++)
			  {
			    ans[n]=inp[i,j];
          n++;
			  }
			}
      return ans;
    }
    //Compose line equation by two given points y=k*x+b (out double k, out double b)
		public void ComposeLineEquation(Point p1, Point p2, out Line line)
    {
		double k = (p1.Y - p2.Y) / (p1.X - p2.X);
		double b = p1.Y - k * p1.X;
		line = new Line(k, b);
    }
		//Compose line equation PERPENDICULAR to given line in a given point
		public void ComposePerpendicularLineEquation(Point p, Line line)
			{
			double k = -1 / line.k;
			double b = p.Y - k * p.X;
			Line perpendicular = new Line(k, b);
			}
    //Compose circle equation by given center point and radius (out double p, out double q)
		public void ComposeCircleEquation(Point center, int rad)
    {
		//не требуется, т.к. функция FindLineCircleIntersections - самодостаточна
    }
    //Find the points of line and circle intersection, if any (out Point ipnt1 , out Point ipnt2)
		public static int FindLineCircleIntersections(float cx, float cy, float radius, PointF point1, PointF point2, out PointF intersection1, out PointF intersection2)
    {
      float dx, dy, A, B, C, det, t;

      dx = point2.X - point1.X;
      dy = point2.Y - point1.Y;

      A = dx * dx + dy * dy;
      B = 2 * (dx * (point1.X - cx) + dy * (point1.Y - cy));
      C = (point1.X - cx) * (point1.X - cx) +
          (point1.Y - cy) * (point1.Y - cy) -
          radius * radius;

      det = B * B - 4 * A * C;
      if ((A <= 0.0000001) || (det < 0))
      {
        // No real solutions.
        intersection1 = new PointF(float.NaN, float.NaN);
        intersection2 = new PointF(float.NaN, float.NaN);
        return 0;
      }
      else if (det == 0)
      {
        // One solution.
        t = -B / (2 * A);
        intersection1 =
            new PointF(point1.X + t * dx, point1.Y + t * dy);
        intersection2 = new PointF(float.NaN, float.NaN);
        return 1;
      }
      else
      {
        // Two solutions.
        t = (float)((-B + Math.Sqrt(det)) / (2 * A));
        intersection1 =
            new PointF(point1.X + t * dx, point1.Y + t * dy);
        t = (float)((-B - Math.Sqrt(det)) / (2 * A));
        intersection2 =
            new PointF(point1.X + t * dx, point1.Y + t * dy);
        return 2;
      }
    }
		//-----------------------------------------------
		public static  int FindLineCircleIntersections(Line line, Circle circle, out PointF intersection1, out PointF intersection2)
			{
			float dx, dy, A, B, C, det, t;
			//---------------------------------------------
			//---------------------------------------------
			//---ЭТО ОШИБКА, ЛИНИЯ БЫВАЕТ И ПАРАЛЛЕЛЬНОЙ ОХ или ОУ-------
			PointF point1 = line.OxIntersection(); 
			PointF point2 = line.OyIntersection();
			//---------------------------------------------
			//---------------------------------------------
			//---------------------------------------------
			float cx = (float)circle.center.X;
			float cy = (float)circle.center.Y;
			float radius = (float)circle.radius;

			dx = point2.X - point1.X;
			dy = point2.Y - point1.Y;

			A = dx * dx + dy * dy;
			B = 2 * (dx * (point1.X - cx) + dy * (point1.Y - cy));
			C = (point1.X - cx) * (point1.X - cx) +
					(point1.Y - cy) * (point1.Y - cy) -
					radius * radius;

			det = B * B - 4 * A * C;
			if ((A <= 0.0000001) || (det < 0))
				{
				// No real solutions.
				intersection1 = new PointF(float.NaN, float.NaN);
				intersection2 = new PointF(float.NaN, float.NaN);
				return 0;
				}
			else if (det == 0)
				{
				// One solution.
				t = -B / (2 * A);
				intersection1 =
						new PointF(point1.X + t * dx, point1.Y + t * dy);
				intersection2 = new PointF(float.NaN, float.NaN);
				return 1;
				}
			else
				{
				// Two solutions.
				t = (float)((-B + Math.Sqrt(det)) / (2 * A));
				intersection1 =
						new PointF(point1.X + t * dx, point1.Y + t * dy);
				t = (float)((-B - Math.Sqrt(det)) / (2 * A));
				intersection2 =
						new PointF(point1.X + t * dx, point1.Y + t * dy);
				return 2;
				}
			}
		//-----------------------------------------------
		private  PointF FindNearestPoint(PointF p1, PointF p2, PointF center)
			{
			double r1=Math.Sqrt(Math.Pow((center.X-p1.X),2)+Math.Pow((center.Y-p1.Y),2));
			double r2=Math.Sqrt(Math.Pow((center.X-p2.X),2)+Math.Pow((center.Y-p2.Y),2));
			if (r1 > r2)
				return p2;
			else
				return p1;
			}
    // ----Find the points where the two circles intersect (out Point pnt1, out Point pnt2)------
    private int FindCircleCircleIntersections(
        float cx0, float cy0, float radius0,
        float cx1, float cy1, float radius1,
        out PointF intersection1, out PointF intersection2)
    {
      // Find the distance between the centers.
      float dx = cx0 - cx1;
      float dy = cy0 - cy1;
      double dist = Math.Sqrt(dx * dx + dy * dy);

      // See how many solutions there are.
      if (dist > radius0 + radius1)
      {
        // No solutions, the circles are too far apart.
        intersection1 = new PointF(float.NaN, float.NaN);
        intersection2 = new PointF(float.NaN, float.NaN);
        return 0;
      }
      else if (dist < Math.Abs(radius0 - radius1))
      {
        // No solutions, one circle contains the other.
        intersection1 = new PointF(float.NaN, float.NaN);
        intersection2 = new PointF(float.NaN, float.NaN);
        return 0;
      }
      else if ((dist == 0) && (radius0 == radius1))
      {
        // No solutions, the circles coincide.
        intersection1 = new PointF(float.NaN, float.NaN);
        intersection2 = new PointF(float.NaN, float.NaN);
        return 0;
      }
      else
      {
        // Find a and h.
        double a = (radius0 * radius0 -
            radius1 * radius1 + dist * dist) / (2 * dist);
        double h = Math.Sqrt(radius0 * radius0 - a * a);

        // Find P2.
        double cx2 = cx0 + a * (cx1 - cx0) / dist;
        double cy2 = cy0 + a * (cy1 - cy0) / dist;

        // Get the points P3.
        intersection1 = new PointF(
            (float)(cx2 + h * (cy1 - cy0) / dist),
            (float)(cy2 - h * (cx1 - cx0) / dist));
        intersection2 = new PointF(
            (float)(cx2 - h * (cy1 - cy0) / dist),
            (float)(cy2 + h * (cx1 - cx0) / dist));

        // See if we have 1 or 2 solutions.
        if (dist == radius0 + radius1) return 1;
        return 2;
      }
    }
		//--------------------------------------------------------------
		private PointF RotatePointByAngleDgr(PointF rotPoint, double rotAngle, PointF rotCenter)
			{
			PointF p=new PointF();
			double x0 = rotPoint.X;
			double y0 = rotPoint.Y;
			double x1 = rotCenter.X;
			double y1 = rotCenter.Y;
			p.X = (float)(Math.Abs(x0 - x1) * Math.Cos(rotAngle * Math.PI / 180) - Math.Abs(y0 - y1) * Math.Sin(rotAngle * Math.PI / 180));
			p.Y = (float)(Math.Abs(x0 - x1) * Math.Sin(rotAngle * Math.PI / 180) + Math.Abs(y0 - y1) * Math.Cos(rotAngle * Math.PI / 180));
			return p;
			}
		//if angle>0 - rotate clockwise, angle<0 - counterclockwise, angle=0 - no rotation
		//p0 - rotation center, p1 - point to rotate, angle - rotation angle in radians, p2 - point to return
        public static double DistanceBetweenTwoPoints(PointF a, PointF b)
        {
            double dist = Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
            return dist;
        }

  }
}
