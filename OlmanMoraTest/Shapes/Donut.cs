using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlmanMoraTest.Shapes
{
    public class Donut : Circle, IShape
    {
        public double Radius2 { get; set; }

        public Donut(double X, double Y, double Radius, double Radius2) : base(Radius, X, Y,ShapeTypeEnum.Donut)
        {
            this.Radius2 = Radius2;
        }

        public new void Draw(Graphics gx)
        {
            gx.FillEllipse(this.FillColor, (float)((X + Canvas.RefX) - (Radius / 2)), (float)((Y * -1) + Canvas.RefY - (Radius / 2)), (float)this.Radius, (float)Radius);
            gx.FillEllipse(Brushes.Azure, (float)(X + Canvas.RefX - (Radius2 / 2)), (float)((Y * -1) + Canvas.RefY - (Radius2 / 2)), (float)Radius2, (float)Radius2);
        }

        public new double GetArea()
        {
            return ((Math.PI * Radius2 * Radius2) - (Math.PI * Radius * Radius));
        }

        public new string GetData()
        {
            return string.Format("Shape Id:{0} | Name: {1} | Area: {2} | Coords: ({3},{4}) | Radius1: {5} | Radius2 {6}", this.Id, this.Name, GetArea(), this.X, this.Y, this.Radius, this.Radius2);
        }

        public new bool ShapeOverlaps(Point location)
        {
            var contains = false;
            using (var gp = new System.Drawing.Drawing2D.GraphicsPath())
            {
                gp.AddEllipse((float)((X + Canvas.RefX) - (Radius / 2)), (float)((Y * -1) + Canvas.RefY - (Radius / 2)), (float)this.Radius, (float)Radius);
                gp.AddEllipse((float)(X + Canvas.RefX - (Radius2 / 2)), (float)((Y * -1) + Canvas.RefY - (Radius2 / 2)), (float)Radius2, (float)Radius2);
                contains = gp.IsVisible(location);
            }
            return contains;
        }
    }
}
