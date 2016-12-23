using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlmanMoraTest.Shapes
{
    public class Circle : Shape, IShape
    {
        private double _Radius;
        public double Radius
        {
            get
            {
                return Math.Abs(_Radius);
            }
            set
            {
                _Radius = value;
            }
        }

        public double Diameter
        {
            get
            {
                return _Radius * 2;
            }
        }
        public Circle(double Radius, double X, double Y, ShapeTypeEnum Type) : base(X, Y, Type)
        {
            this.Radius = Radius;
        }
        public Circle(double Radius, double X, double Y) : base(X, Y, ShapeTypeEnum.Circle)
        {
            this.Radius = Radius;
        }
        public double GetArea()
        {
            if (this.Radius == default(double))
                throw new ArgumentNullException("Radius is neccesary for calculate Circle's area");

            return (double)(Math.PI * Radius * Radius); ;
        }

        public string GetData()
        {
            return string.Format("Shape Id:{0} | Name: {1} | Area: {2} | Coords: ({3},{4}) | Radius: {5}", this.Id, this.Name, GetArea(), this.X, this.Y, this.Radius );
        }

        public void Draw(Graphics gx)
        {
            gx.FillEllipse(this.FillColor,
                            (float)((this.X + Canvas.RefX) - (this.Radius / 2)),
                            (float)(((this.Y * -1) + Canvas.RefY) - (this.Radius / 2)),
                            (float)this.Radius, (float)this.Radius);
        }

        public bool ShapeOverlaps(Point location)
        {
            var contains = false;
            using (var gp = new System.Drawing.Drawing2D.GraphicsPath())
            {
                gp.AddEllipse((float)((this.X + Canvas.RefX) - (this.Radius / 2)),
                            (float)(((this.Y * -1) + Canvas.RefY) - (this.Radius / 2)),
                            (float)this.Radius, (float)this.Radius);
                contains = gp.IsVisible(location);
            }
            return contains;
        }
    }
}
