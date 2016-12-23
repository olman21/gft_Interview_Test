using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlmanMoraTest.Shapes
{
    public class Rectangle : Shape, IShape
    {
        private double _Side1;
        private double _Side2;
        public double Side1
        {
            get
            {
                return Math.Abs(_Side1);
            }
            set
            {
                _Side1 = value;
            }
        }
        public double Side2
        {
            get
            {
                return Math.Abs(_Side2);
            }
            set
            {
                _Side2 = value;
            }
        }
        public Rectangle(double Side1,double Side2, double X, double Y) : base(X,Y,ShapeTypeEnum.Rectangle)
        {
            this._Side1 = Side1;
            this._Side2 = Side2;
            this.SideCount = 4;
        }
        public double GetArea()
        {
            if (_Side2 == default(double) || _Side1 == default(double))
                throw new ArgumentNullException("Side1 and Side2 are neccesary for calculate rectangle's area");

            return this._Side1 * this._Side2;
        }

        public string GetData()
        {
            return string.Format("Shape Id:{0} | Name: {1} | Area: {2} | Coords: ({3},{4}) | Width: {5} | Height: {6}", this.Id, this.Name, GetArea(), this.X, this.Y, this.Side1, this.Side2);
        }

        public void Draw(Graphics gx)
        {
            gx.FillRectangle(this.FillColor, new System.Drawing.Rectangle((int)X + Canvas.RefX,(int)(Y * -1) + Canvas.RefY, (int)Side1, (int)Side2));
        }

        public bool ShapeOverlaps(Point location)
        {
            var contains = false;
            using (var gp = new System.Drawing.Drawing2D.GraphicsPath())
            {
                gp.AddRectangle(new System.Drawing.Rectangle((int)X + Canvas.RefX, (int)(Y * -1) + Canvas.RefY, (int)Side1, (int)Side2));
                contains = gp.IsVisible(location);
            }
            return contains;
        }

       
    }
}
