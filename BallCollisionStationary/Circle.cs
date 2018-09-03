using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallCollisionStationary
	{
	public class Circle
		{
		public PointF center { get; private set; }
		public double radius { get; private set; }
		public Circle(PointF center, double radius)
			{
			this.center = center;
			this.radius = radius;
			}
		public Circle(double x, double y, double radius)
			{
            center = new PointF((float)x, (float)y);
			this.radius = radius;
			}
        public void Draw(Graphics graph,Pen pen)
        {
            float x, y, width, height;
            x = (float)(center.X - radius);
            y = (float)(center.Y - radius);
            width = (float)(radius * 2);
            height = (float)(radius * 2);
            graph.DrawEllipse(pen, x, y, width, height);
        }
		}
	}
