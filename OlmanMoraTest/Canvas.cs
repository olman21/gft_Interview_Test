using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace OlmanMoraTest
{
    public static class Canvas
    {
        public static int Width
        {
            get
            {
                int Width;
                if (int.TryParse(ConfigurationManager.AppSettings["CanvasWidth"], out Width))
                {
                    return Width;
                }
                return 1024;
            }
        }
        public static int Height
        {
            get
            {
                int Height;
                if (int.TryParse(ConfigurationManager.AppSettings["CanvasHeight"], out Height))
                {
                    return Height;
                }
                return 768;
            }
        }

        public static int RefX
        {
            get
            {
                return Width / 2;
            }
        }

        public static int RefY
        {
            get
            {
                return Height / 2;
            }
        }
    }
}
