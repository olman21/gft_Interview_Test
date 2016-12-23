using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlmanMoraTest.Shapes
{
    public class Square : Shape, IShape
    {
        private double _Side;
        public double Side
        {
            get
            {
                return Math.Abs(_Side);
            }
            set
            {
                _Side = value;
            }
        }
        public Square(double SideLength, double X, double Y) : base(X,Y,ShapeTypeEnum.Square)
        {
            this.Side = SideLength;
            this.SideCount = 4;

        }
        public double GetArea()
        {
            if (Side == default(double))
                throw new ArgumentNullException("Side lenght is neccesary for calculate square area");
            return this.Side * this.Side;

        }

        public string GetData()
        {
            return string.Format("Shape Id:{0} | Name: {1} | Area: {2} | Coords: ({3},{4}) | Side: {5}", this.Id, this.Name, GetArea(), this.X, this.Y, this.Side);
        }

        public void Draw(Graphics gx)
        {
            gx.FillRectangle(this.FillColor, new System.Drawing.Rectangle((int)X + Canvas.RefX, (int)(Y * -1) + Canvas.RefY, (int)Side, (int)Side));
        }

        public bool ShapeOverlaps(Point location)
        {
            var contains = false;
            using (var gp = new System.Drawing.Drawing2D.GraphicsPath())
            {
                gp.AddRectangle(new System.Drawing.Rectangle((int)X + Canvas.RefX, (int)(Y * -1) + Canvas.RefY, (int)Side, (int)Side));
                contains = gp.IsVisible(location);
            }
            return contains;
        }
    }
}
