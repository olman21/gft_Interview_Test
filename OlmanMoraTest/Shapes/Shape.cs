using System;
using System.Collections.Generic;
using System.Drawing;

namespace OlmanMoraTest.Shapes
{
    public abstract class Shape
    {

        public double X { get; set; }
        public double Y { get; set; }
        public int SideCount { get; set; }

        public SolidBrush FillColor { get; set; }

        public string Id { get; set; }
        public ShapeTypeEnum Type { get; set; }

        public List<Shape> OverlapShapes { get; set; }

        public string Name
        {
            get {
                return Type.ToString();
            }
        }

        public Shape(double X, double Y, ShapeTypeEnum Type)
        {
            this.Id = Extensions.UnixTimeNow().ToString();
            this.X = X;
            this.Y = Y;
            this.Type = Type;
            this.FillColor = this.GetRandomColor();
            this.OverlapShapes = new List<Shape>();
        }

        private SolidBrush GetRandomColor()
        {
            var r = new Random(DateTime.Now.Ticks.GetHashCode());
            var red = r.Next(0, byte.MaxValue + 1);
            var green = r.Next(0, byte.MaxValue + 1);
            var blue = r.Next(0, byte.MaxValue + 1);
            return new SolidBrush(Color.FromArgb(red, green, blue));
        }

        public string GetId()
        {
            return this.Id;
        }

        public ShapeTypeEnum GetShapeType()
        {
            return this.Type;
        }

        public List<Shape> GetOverlapShapes()
        {
            return this.OverlapShapes;
        }

        public void AddOverlapShape(Shape shape)
        {
            this.OverlapShapes.Add(shape);
        }



    }
}
