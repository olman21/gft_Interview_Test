using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlmanMoraTest.Shapes
{
    public interface IShape
    {
        double GetArea();
        string GetData();
        void Draw(Graphics gx);
        bool ShapeOverlaps(Point location);
        string GetId();
        ShapeTypeEnum GetShapeType();
        List<Shape> GetOverlapShapes();
        void AddOverlapShape(Shape shape);

    }
}
