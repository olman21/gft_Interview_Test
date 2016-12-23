using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlmanMoraTest.Shapes
{
    public class Ellipse : Shape, IShape
    {
        public double Width { get; set; }
        public double Height { get; set; }


        public Ellipse(double X, double Y, double Width, double Height) : base(X, Y, ShapeTypeEnum.Ellipse)
        {
            this.Width = Width;
            this.Height = Height;
        }
        public double GetArea()
        {
            return (double)(Math.PI * Width / 2 * Height / 2);
        }

        public string GetData()
        {
            return string.Format("Shape Id:{0} | Name: {1} | Area: {2} | Coords: ({3},{4}) | Width: {5} | Height: {6}", this.Id, this.Name, GetArea(), this.X, this.Y, this.Width, this.Height);
        }

        public void Draw(Graphics gx)
        {
            gx.FillEllipse(this.FillColor,
                            (float)((this.X + Canvas.RefX) - (this.Width / 2)),
                            (float)(((this.Y * -1) + Canvas.RefY) - (this.Height / 2)),
                            (float)this.Width, (float)this.Height);
        }

        public bool ShapeOverlaps(Point location)
        {
            var contains = false;
            using (var gp = new System.Drawing.Drawing2D.GraphicsPath())
            {
                gp.AddEllipse((float)((this.X + Canvas.RefX) - (this.Width / 2)),
                            (float)(((this.Y * -1) + Canvas.RefY) - (this.Height / 2)),
                            (float)this.Width, (float)this.Height);
                contains = gp.IsVisible(location);
            }
            return contains;
        }
    }
}
