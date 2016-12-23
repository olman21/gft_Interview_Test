using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlmanMoraTest.Shapes
{
    public class Triangle : Shape, IShape
    {
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double X3 { get; set; }
        public double Y3 { get; set; }

        public Point[] Points
        {
            get {
                return new Point[] { new Point((int)X + Canvas.RefX, (int)(Y * -1) + Canvas.RefY), new Point((int)X2 + Canvas.RefX, (int)(Y2 * -1) + Canvas.RefY), new Point((int)X3+Canvas.RefX, (int)(Y3 * -1) + Canvas.RefY) };
            }
        }

        public Triangle(double X1, double Y1, double X2, double Y2, double X3, double Y3) : base(X1,Y1,ShapeTypeEnum.Triangle)
        {
            this.X2 = X2;
            this.Y2 = Y2;
            this.X3 = X3;
            this.Y3 = Y3;
            this.SideCount = 3;
        }
        public double GetArea()
        {
            return Math.Abs((X * (Y2 - Y3) + X2 * (Y3 - Y) + X3 * (Y - Y2)) / 2);
        }

        public string GetData()
        {
            return string.Format("Shape Id:{0} | Name: {1} | Area: {2} | Coords: ({3},{4}) ({5}.{6}) ({7}{8})", this.Id, this.Name, GetArea(), this.X,this.Y,this.X2,this.Y2,this.X3, this.Y3);
        }

        public void Draw(Graphics gx)
        {
            gx.FillPolygon(this.FillColor, this.Points);
        }

        public bool ShapeOverlaps(Point location)
        {
            var contains = false;
            using (var gp = new System.Drawing.Drawing2D.GraphicsPath())
            {
                gp.AddPolygon(this.Points);
                contains = gp.IsVisible(location);
            }
            return contains;
        }
    }
}
