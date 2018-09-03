using System.Drawing;
using System;

namespace BallCollisionStationary
	{
	public enum Type {horizontal,vertical,standart};
	//конструктор по двум точкам
	public class Line
		{
		public double k { get; private set; }
		public double b { get; private set; }
		public Type type { get; private set; }
		//конструктор по двум точкам
		public Line(Point p1, Point p2)
			{
			double dx, dy;
			dx = p1.X - p2.X;
			dy = p1.Y - p2.Y;

			if (Math.Abs(dy) < 0.1)
				this.type = Type.horizontal;
			if (Math.Abs(dx) < 0.1)
				this.type = Type.vertical;
			else
				{
				this.type = Type.standart;
				k = (p1.Y - p2.Y) / (p1.X - p2.X);
				b = p1.Y - k * p1.X;
				}
			}
		//конструктор по двум точкам
		public Line(PointF p1, PointF p2)
			{
			double dx, dy;
			dx=p1.X-p2.X;
			dy=p1.Y-p2.Y;

			if (Math.Abs(dy)<0.01)
				this.type=Type.horizontal;
			if (Math.Abs(dx) < 0.01)
				this.type=Type.vertical;
			else
			{
				this.type=Type.standart;
				k = (p1.Y - p2.Y) / (p1.X - p2.X);
				b = p1.Y - k * p1.X;
			}
			}
		//конструктор по коэффициентам k, b
		public Line(double k, double b)
			{
			this.type = Type.standart;
			this.k = k;
			this.b = b;
			}
		//вычислить сегмент линии по известной точке на линии и длине сегмента
		public void CalculateLineSegment(PointF p, int length, out PointF pa, out PointF pb)
			{
			float dx, dy;
			PointF a = new PointF();
			PointF b = new PointF();
			switch (this.type)
				{
				case Type.horizontal:
					dx = length / 2;
					dy = 0;
					a.X = p.X + dx;
					a.Y = p.Y + dy;//dy=0
					b.X = p.X - dx;
					b.Y = p.Y - dy;//dy=0
					break;
				case Type.vertical:
					dx = 0;
					dy = length / 2;
					a.X = p.X + dx;//dx=0
					a.Y = p.Y + dy;
					b.X = p.X - dx;//dx=0
					b.Y = p.Y - dy;
					break;
				case Type.standart:
					double alpha;
					alpha = Math.Atan(k);//угол наклона прямой к ОХ в радианах
					dx = (float)Math.Cos(alpha) * length / 2;
					dy = (float)Math.Sin(alpha) * length / 2;
                    //if(k>0)
						a.X = p.X + dx;
						a.Y = p.Y + dy;
						b.X = p.X - dx;
						b.Y = p.Y - dy;
                    //if(k<0)
                    //    a.X = p.X - dx;
                    //    a.Y = p.Y - dy;
                    //    b.X = p.X + dx;
                    //    b.Y = p.Y + dy;
                    break;
				}
			pa = new PointF(a.X, a.Y);
			pb = new PointF(b.X, b.Y);
			}
        public void DrawSegment(Graphics graph, Pen pen,PointF p, int length)
        {
            PointF pa, pb;
            CalculateLineSegment(p, length, out pa, out pb);
            graph.DrawLine(pen, pa, pb);
        }
		public PointF OxIntersection()
			{
			double x; 
			double y = 0;
			x=-this.b/this.k;
			PointF p = new PointF((float)x, (float)y);
			return p;
			}
		public PointF OyIntersection()
			{
			double x=0;
			double y;
			y = this.b;
			PointF p = new PointF((float)x, (float)y);
			return p;
			}
		}
	}
