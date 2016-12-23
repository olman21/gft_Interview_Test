using OlmanMoraTest.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlmanMoraTest
{
    public class ShapeManager
    {
        public List<IShape> CurrentShapes;
        private string[] ArgsNames;
        private string[] CommandNames;
        const string ArgsSymbol = "--";

        public ShapeManager()
        {
            this.CurrentShapes = new List<IShape>();
            this.ArgsNames = new string[] { ArgsSymbol + "X",
                                            ArgsSymbol + "Y",
                                            ArgsSymbol + "SLength",
                                            ArgsSymbol + "Side",
                                            ArgsSymbol + "Side1",
                                            ArgsSymbol + "Side2",
                                            ArgsSymbol + "Radius",
                                            ArgsSymbol + "r",
                                            ArgsSymbol + "Radius1",
                                            ArgsSymbol + "R1",
                                            ArgsSymbol + "Radius2",
                                            ArgsSymbol + "R2",
                                            ArgsSymbol + "X1",
                                            ArgsSymbol + "Y1",
                                            ArgsSymbol + "X2",
                                            ArgsSymbol + "Y2",
                                            ArgsSymbol + "X3",
                                            ArgsSymbol + "Y3",
                                            ArgsSymbol + "Shape",
                                            ArgsSymbol + "Width",
                                            ArgsSymbol + "W",
                                            ArgsSymbol + "Height",
                                            ArgsSymbol + "H",
                                            ArgsSymbol + "Id",
                                            "Circle",
                                            "Square",
                                            "Triangle",
                                            "Rectangle",
                                            "Donut",
                                            "Ellipse",
                                            "Image",
                                            "All"};
            this.CommandNames = new string[] { "ADD", "REMOVE", "SHOW", "FROMFILE", "EXIT", "HELP", "RESET" };
        }

        public void Start()
        {
            Console.Clear();

            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Type a new command, use \"Help\" for get the complete list of commands\n\n");
                Console.ForegroundColor = ConsoleColor.White;
            } while (WaitForCommand());
        }

        private bool WaitForCommand()
        {
            string FullCommand = Console.ReadLine();
            return ExecuteCommand(FullCommand);
        }

        private bool ExecuteCommand(string FullCommand)
        {
            string[] SplitedCommand = FullCommand.Split(' ');
            string Command = SplitedCommand[0];
            if (!IsSystemCommand(Command))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Unknown command (Use \"Help\" for get the complete List of commands)\n\n");
                Console.ForegroundColor = ConsoleColor.White;
                return true;
            }

            Dictionary<string, string[]> Arguments = GetCommandArguments(SplitedCommand);

            switch (Command.ToUpper())
            {
                case "EXIT":
                    return false;
                case "HELP":
                    PrintHelp();
                    break;
                case "FROMFILE":
                    LoadFromFile(SplitedCommand);
                    break;
                case "SHOW":
                    Show(Arguments);
                    break;
                case "REMOVE":
                    Remove(Arguments);
                    break;
                case "RESET":
                    Console.Clear();
                    CurrentShapes.Clear();
                    break;
                default:
                    AddShape(Command, Arguments);
                    break;
            }

            return true;
        }

        private void Remove(Dictionary<string, string[]> Args)
        {
            string Id = Args.GetFirstOrDefaultValue(ArgsSymbol+"id");
            if (string.IsNullOrEmpty(Id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("--id argument is required");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            CurrentShapes.RemoveAll(s => s.GetId() == Id);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Item Removed");
            Console.ForegroundColor = ConsoleColor.White;

        }

        private void Show(Dictionary<string, string[]> Args)
        {
            var SubCommand = Args.FirstOrDefault(c => !c.Key.StartsWith(ArgsSymbol));
            if (string.IsNullOrEmpty(SubCommand.Key) && string.IsNullOrEmpty(Args.GetFirstOrDefaultValue(ArgsSymbol + "id")))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unknown SubCommand");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            else if (!string.IsNullOrEmpty(Args.GetFirstOrDefaultValue(ArgsSymbol + "id")))
            {
                SubCommand = Args.FirstOrDefault(c => c.Key.EqualIgnoreCase(ArgsSymbol + "id"));
            }

            switch (SubCommand.Key.ToUpper())
            {
                case "IMAGE":
                    GenerateImage();
                    break;
                case "ALL":
                    PrintAllShapes();
                    break;
                case "CIRCLE":
                case "RECTANGLE":
                case "SQUARE":
                case "TRIANGLE":
                case "DONUT":
                case "ELLIPSE":
                    PrintShapesByType(SubCommand.Key);
                    break;
                case ArgsSymbol + "ID":
                    PrintShape(SubCommand.Value[0]);
                    break;

                default:

                    break;
            }

        }

        private void PrintAllShapes()
        {
            foreach (IShape shape in CurrentShapes)
            {
                try
                {
                    Console.WriteLine(shape.GetData());
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine(e.Message);
                }

                ShowOverlapShapeList(shape);
            }
        }

        private void PrintShape(string Id)
        {
            IShape shape = CurrentShapes.FirstOrDefault(s => s.GetId() == Id);
            try
            {
                Console.WriteLine(shape.GetData());
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }

            ShowOverlapShapeList(shape);
        }

        private void PrintShapesByType(string Type)
        {
            foreach (IShape shape in CurrentShapes.Where(s=>s.GetShapeType().ToString().EqualIgnoreCase(Type)))
            {
                try
                {
                    Console.WriteLine(shape.GetData());
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine(e.Message);
                }

                ShowOverlapShapeList(shape);
            }
        }

        private void GenerateImage()
        {
            using (var bmp = new Bitmap(Canvas.Width, Canvas.Height))
            {
                using (var gx = Graphics.FromImage(bmp))
                {
                    gx.FillRectangle(Brushes.Azure, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height));
                    foreach (IShape shape in CurrentShapes)
                    {
                        shape.Draw(gx);
                    }
                }

                string Dir = System.IO.Path.Combine(Directory.GetCurrentDirectory() + @"\image_preview\");
                string path = Path.Combine(Dir, "shape-output.png");
                if (!Directory.Exists(Dir))
                {
                    Directory.CreateDirectory(Dir);
                }
                bmp.Save(path);
                Process.Start(path);
            }
        }

        private void LoadFromFile(string[] Args)
        {
            string Path = "";
            if (Args.Length >= 2)
                Path = Args[1];
            if (!File.Exists(Path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File not found");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }


            string[] lines = System.IO.File.ReadAllLines(Path);
            foreach (string line in lines)
            {
                ExecuteCommand(line);
            }
        }

        private void PrintHelp()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("====================================HELP============================================");
            Console.WriteLine("Commands:");
            Console.WriteLine("ADD [ShapeName] [--named arguments]... : Adds a new shape to the list.");
            Console.WriteLine("REMOVE [ShapeName] [-id] OR id : Removes a shape to the list.");
            Console.WriteLine("SHOW Image: Show a graphic representation of the canvas");
            Console.WriteLine("SHOW [ShapeId]: Show Shape Information");
            Console.WriteLine("SHOW All: Show all Shapes Information");
            Console.WriteLine("SHOW [ShapeName]: Show all shapes for the specified type");
            Console.WriteLine("FROMFILE [FilePath]: Load all the shapes from the specified File path.");
            Console.WriteLine("[ShapeName] Circle | Triangle | Donut | Square | Rectangle | Ellipse \n\n");
            Console.WriteLine("Circle (Arguments)");
            Console.WriteLine("Named (order is not important, the values are specified space after from the arg name)");
            Console.WriteLine("{0}X: It corresponds to X coord from the center of the circle", ArgsSymbol);
            Console.WriteLine("{0}Y: It corresponds to Y coord from the center of the circle", ArgsSymbol);
            Console.WriteLine("{0}R OR {0}Radius: It corresponds to Radius of the circle", ArgsSymbol);
            Console.WriteLine("Unnamed (order is IMPORTANT)");
            Console.WriteLine("1:X | 2:Y | 3:Radius \n\n");
            Console.WriteLine("Triangule (Arguments)");
            Console.WriteLine("Named (order is not important, the values are specified space after from the arg name)");
            Console.WriteLine("{0}X1: It corresponds to X coord from the first triangle corner", ArgsSymbol);
            Console.WriteLine("{0}Y1: It corresponds to Y coord from the first triangle corner", ArgsSymbol);
            Console.WriteLine("{0}X2: It corresponds to X coord from the second triangle corner", ArgsSymbol);
            Console.WriteLine("{0}Y2: It corresponds to Y coord from the second triangle corner", ArgsSymbol);
            Console.WriteLine("{0}X3: It corresponds to X coord from the third triangle corner", ArgsSymbol);
            Console.WriteLine("{0}Y3: It corresponds to Y coord from the third triangle corner", ArgsSymbol);
            Console.WriteLine("Unnamed (order is IMPORTANT)");
            Console.WriteLine("1:X1 | 2:Y1 | 3:X2 | 4:Y2 | 5:X3 | 6:Y3 \n\n");
            Console.WriteLine("Square (Arguments)");
            Console.WriteLine("Named (order is not important, the values are specified space after from the arg name)");
            Console.WriteLine("{0}X: It corresponds to X coord from the Square corner", ArgsSymbol);
            Console.WriteLine("{0}Y: It corresponds to Y coord from the Square corner", ArgsSymbol);
            Console.WriteLine("{0}Side: It corresponds to length of the Side", ArgsSymbol);
            Console.WriteLine("Unnamed (order is IMPORTANT)");
            Console.WriteLine("1:X | 2:Y | 3:SideLength \n\n");
            Console.WriteLine("Rectangle (Arguments)");
            Console.WriteLine("Named (order is not important, the values are specified space after from the arg name)");
            Console.WriteLine("{0}X: It corresponds to X coord from the Square corner", ArgsSymbol);
            Console.WriteLine("{0}Y: It corresponds to Y coord from the Square corner", ArgsSymbol);
            Console.WriteLine("{0}Width: It corresponds to length of the Width", ArgsSymbol);
            Console.WriteLine("{0}Height: It corresponds to length of the Height", ArgsSymbol);
            Console.WriteLine("Unnamed (order is IMPORTANT)");
            Console.WriteLine("1:X | 2:Y | 3:Width | 4:Width \n\n");
            Console.WriteLine("Donut (Arguments)");
            Console.WriteLine("Named (order is not important, the values are specified space after from the arg name)");
            Console.WriteLine("{0}X: It corresponds to X coord from the Donut Centre", ArgsSymbol);
            Console.WriteLine("{0}Y: It corresponds to Y coord from the Donut Centre", ArgsSymbol);
            Console.WriteLine("{0}Radius1: It corresponds to length of the Outer Radius", ArgsSymbol);
            Console.WriteLine("{0}Radius2: It corresponds to length of the Inner Radius", ArgsSymbol);
            Console.WriteLine("Unnamed (order is IMPORTANT)");
            Console.WriteLine("1:X | 2:Y | 3:Radius1 | 4:Radius2 \n\n");
            Console.WriteLine("Ellipse (Arguments)");
            Console.WriteLine("Named (order is not important, the values are specified space after from the arg name)");
            Console.WriteLine("{0}X: It corresponds to X coord from the center of the Ellipse", ArgsSymbol);
            Console.WriteLine("{0}Y: It corresponds to Y coord from the center of the Ellipse", ArgsSymbol);
            Console.WriteLine("{0}W OR {0}Width: It corresponds to Width of the Ellipse", ArgsSymbol);
            Console.WriteLine("{0}H OR {0}Height: It corresponds to Height of the Ellipse", ArgsSymbol);
            Console.WriteLine("Unnamed (order is IMPORTANT)");
            Console.WriteLine("1:X | 2:Y | 3:Width | 4:Height \n\n");

            Console.ForegroundColor = ConsoleColor.White;
        }

        private bool IsSystemArgument(string value)
        {
            return this.ArgsNames.Any(a => a.EqualIgnoreCase(value));
        }

        private bool IsSystemCommand(string value)
        {
            return this.CommandNames.Any(c => c.EqualIgnoreCase(value));
        }

        private Dictionary<string, string[]> GetCommandArguments(string[] SplitedCommand)
        {
            Dictionary<string, string[]> Args = new Dictionary<string, string[]>(StringComparer.InvariantCultureIgnoreCase);
            for (int i = 1; i < SplitedCommand.Length; i++)
            {
                string arg = SplitedCommand[i];
                if (SplitedCommand.Any(a => a.StartsWith(ArgsSymbol)))
                {
                    if (IsSystemArgument(arg))
                    {
                        var ArgValue = SplitedCommand[i + 1];
                        if (!IsSystemArgument(ArgValue))
                            Args.Add(arg, new string[] { ArgValue });
                    }
                }
                else
                {
                    if (IsSystemArgument(arg))
                    {
                        Args.Add(arg, SplitedCommand.Where((c, index) => index > i).ToArray());
                        break;
                    }
                }

            }

            return Args;
        }

        private void AddShape(string Command, Dictionary<string, string[]> Args)
        {
            if (Command.EqualIgnoreCase("ADD"))
            {
                var SubCommand = Args.FirstOrDefault(c => !c.Key.StartsWith(ArgsSymbol));
                if (SubCommand.Key == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ADD command has some missing arguments");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

                switch (SubCommand.Key.ToUpper())
                {
                    case "CIRCLE":
                        AddCircle(Args);
                        break;
                    case "TRIANGLE":
                        AddTriangle(Args);
                        break;
                    case "SQUARE":
                        AddSquare(Args);
                        break;
                    case "RECTANGLE":
                        AddRectangle(Args);
                        break;
                    case "DONUT":
                        AddDonut(Args);
                        break;
                    case "ELLIPSE":
                        AddEllipse(Args);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Invalid Command, use \"Help\" if you need the complete list of commands");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }

            }
        }

        private void AddEllipse(Dictionary<string, string[]> Args)
        {
            double Width = 0, Height = 0, X = 0, Y = 0;
            bool ValidArgs = true;
            if ((!double.TryParse(Args.GetFirstOrDefaultValue("-Width"), out Width) && !double.TryParse(Args.GetFirstOrDefaultValue("-W"), out Width))
             || (!double.TryParse(Args.GetFirstOrDefaultValue("-Height"), out Width) && !double.TryParse(Args.GetFirstOrDefaultValue("-H"), out Height))
             || !double.TryParse(Args.GetFirstOrDefaultValue("-X"), out X)
             || !double.TryParse(Args.GetFirstOrDefaultValue("-Y"), out Y))
            {
                ValidArgs = false;
                string[] SequencialArgs = Args.GetValueOrDefault("Ellipse");
                if (SequencialArgs != null && SequencialArgs.Length >= 4)
                {
                    ValidArgs = true;
                    for (int i = 0; i < SequencialArgs.Length; i++)
                    {
                        if (i == 0)
                        {
                            if (!double.TryParse(SequencialArgs[i], out X))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 1)
                        {
                            if (!double.TryParse(SequencialArgs[i], out Y))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 2)
                        {
                            if (!double.TryParse(SequencialArgs[i], out Width))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 3)
                        {
                            if (!double.TryParse(SequencialArgs[i], out Height))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                    }
                }

                if (!ValidArgs)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ellipse requires: [X] | [Y] | [Width] | [Height] as number arguments");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

            }

            Ellipse ellipse = new Ellipse(X, Y, Width, Height);
            ProcessShape(ellipse);
        }

        private void AddDonut(Dictionary<string, string[]> Args)
        {
            double Radius = 0, Radius2 = 0, X = 0, Y = 0;
            bool ValidArgs = true;
            if ((!double.TryParse(Args.GetFirstOrDefaultValue("-Radius1"), out Radius) && !double.TryParse(Args.GetFirstOrDefaultValue("-R1"), out Radius))
             || (!double.TryParse(Args.GetFirstOrDefaultValue("-Radius2"), out Radius2) && !double.TryParse(Args.GetFirstOrDefaultValue("-R2"), out Radius2))
             || !double.TryParse(Args.GetFirstOrDefaultValue("-X"), out X)
             || !double.TryParse(Args.GetFirstOrDefaultValue("-Y"), out Y))
            {
                ValidArgs = false;
                string[] SequencialArgs = Args.GetValueOrDefault("Donut");
                if (SequencialArgs != null && SequencialArgs.Length >= 4)
                {
                    ValidArgs = true;
                    for (int i = 0; i < SequencialArgs.Length; i++)
                    {
                        if (i == 0)
                        {
                            if (!double.TryParse(SequencialArgs[i], out X))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 1)
                        {
                            if (!double.TryParse(SequencialArgs[i], out Y))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 2)
                        {
                            if (!double.TryParse(SequencialArgs[i], out Radius))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 3)
                        {
                            if (!double.TryParse(SequencialArgs[i], out Radius2))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                    }
                }

                if (!ValidArgs)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Donut requires: [Radius1] | [Radius2] | [X] | [Y] as number arguments");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

            }

            Donut donut = new Donut(X, Y, Radius, Radius2);
            ProcessShape(donut);
        }

        private void AddCircle(Dictionary<string, string[]> Args)
        {
            double Radius = 0, X = 0, Y = 0;
            bool ValidArgs = true;
            if ((!double.TryParse(Args.GetFirstOrDefaultValue("-Radius"), out Radius) && !double.TryParse(Args.GetFirstOrDefaultValue("-R"), out Radius))
             || !double.TryParse(Args.GetFirstOrDefaultValue("-X"), out X)
             || !double.TryParse(Args.GetFirstOrDefaultValue("-Y"), out Y))
            {
                ValidArgs = false;
                string[] SequencialArgs = Args.GetValueOrDefault("Circle");
                if (SequencialArgs != null && SequencialArgs.Length >= 3)
                {
                    ValidArgs = true;
                    for (int i = 0; i < SequencialArgs.Length; i++)
                    {
                        if (i == 0)
                        {
                            if (!double.TryParse(SequencialArgs[i], out X))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 1)
                        {
                            if (!double.TryParse(SequencialArgs[i], out Y))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 2)
                        {
                            if (!double.TryParse(SequencialArgs[i], out Radius))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                    }
                }

                if (!ValidArgs)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Circle requires: [Radius] | [X] | [Y] as number arguments");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

            }

            Circle circle = new Circle(Radius, X, Y);
            ProcessShape(circle);
        }

        private void AddTriangle(Dictionary<string, string[]> Args)
        {
            double X1 = 0, Y1 = 0, X2 = 0, Y2 = 0, X3 = 0, Y3 = 0;
            bool ValidArgs = true;
            if (!double.TryParse(Args.GetFirstOrDefaultValue("-X1"), out X1)
             || !double.TryParse(Args.GetFirstOrDefaultValue("-Y1"), out Y1)
             || !double.TryParse(Args.GetFirstOrDefaultValue("-X2"), out X2)
             || !double.TryParse(Args.GetFirstOrDefaultValue("-Y2"), out Y2)
             || !double.TryParse(Args.GetFirstOrDefaultValue("-X3"), out X3)
             || !double.TryParse(Args.GetFirstOrDefaultValue("-Y3"), out Y3))
            {
                ValidArgs = false;
                string[] SequencialArgs = Args.GetValueOrDefault("Triangle");
                if (SequencialArgs != null && SequencialArgs.Length >= 6)
                {
                    ValidArgs = true;
                    for (int i = 0; i < SequencialArgs.Length; i++)
                    {

                        if (i == 0)
                        {
                            if (!double.TryParse(SequencialArgs[i], out X1))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 1)
                        {
                            if (!double.TryParse(SequencialArgs[i], out Y1))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 2)
                        {
                            if (!double.TryParse(SequencialArgs[i], out X2))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 3)
                        {
                            if (!double.TryParse(SequencialArgs[i], out Y2))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 4)
                        {
                            if (!double.TryParse(SequencialArgs[i], out X3))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 5)
                        {
                            if (!double.TryParse(SequencialArgs[i], out Y3))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                    }
                }

                if (!ValidArgs)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Triangle requires: [X1] | [Y1] | [X2] | [Y2] | [X3] | [Y3] as number arguments");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

            }

            Triangle triangle = new Triangle(X1, Y1, X2, Y2, X3, Y3);
            ProcessShape(triangle);
        }

        private void AddSquare(Dictionary<string, string[]> Args)
        {
            double SideLength = 0, X = 0, Y = 0;
            bool ValidArgs = true;
            if ((!double.TryParse(Args.GetFirstOrDefaultValue("-Side"), out SideLength) && !double.TryParse(Args.GetFirstOrDefaultValue("-SLength"), out SideLength))
             || !double.TryParse(Args.GetFirstOrDefaultValue("-X"), out X)
             || !double.TryParse(Args.GetFirstOrDefaultValue("-Y"), out Y))
            {
                ValidArgs = false;
                string[] SequencialArgs = Args.GetValueOrDefault("Square");
                if (SequencialArgs != null && SequencialArgs.Length >= 3)
                {
                    ValidArgs = true;
                    for (int i = 0; i < SequencialArgs.Length; i++)
                    {
                        if (i == 0)
                        {
                            if (!double.TryParse(SequencialArgs[i], out X))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 1)
                        {
                            if (!double.TryParse(SequencialArgs[i], out Y))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 2)
                        {
                            if (!double.TryParse(SequencialArgs[i], out SideLength))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                    }
                }

                if (!ValidArgs)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Square requires: [SideLength OR Slength] | [X] | [Y] as number arguments");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

            }

            Square square = new Square(SideLength, X, Y);
            ProcessShape(square);
        }

        private void AddRectangle(Dictionary<string, string[]> Args)
        {
            double Side1 = 0, Side2 = 0, X = 0, Y = 0;
            bool ValidArgs = true;
            if ((!double.TryParse(Args.GetFirstOrDefaultValue("-Side"), out Side1) && !double.TryParse(Args.GetFirstOrDefaultValue("-Side1"), out Side1))
             || !double.TryParse(Args.GetFirstOrDefaultValue("-Side2"), out Side2)
             || !double.TryParse(Args.GetFirstOrDefaultValue("-X"), out X)
             || !double.TryParse(Args.GetFirstOrDefaultValue("-Y"), out Y))
            {
                ValidArgs = false;
                string[] SequencialArgs = Args.GetValueOrDefault("Rectangle");
                if (SequencialArgs != null && SequencialArgs.Length >= 4)
                {
                    ValidArgs = true;
                    for (int i = 0; i < SequencialArgs.Length; i++)
                    {
                        if (i == 0)
                        {
                            if (!double.TryParse(SequencialArgs[i], out X))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 1)
                        {
                            if (!double.TryParse(SequencialArgs[i], out Y))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 2)
                        {
                            if (!double.TryParse(SequencialArgs[i], out Side1))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                        else if (i == 3)
                        {
                            if (!double.TryParse(SequencialArgs[i], out Side2))
                            {
                                ValidArgs = false;
                                break;
                            }

                        }
                    }
                }

                if (!ValidArgs)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Rectangle requires: [Side OR Side1] | [Side2] | [X] | [Y] as number arguments");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }
            }

            Shapes.Rectangle rectangle = new Shapes.Rectangle(Side1, Side2, X, Y);
            ProcessShape(rectangle);
        }



        private void ProcessShape(IShape shape)
        {
            CurrentShapes.Add(shape);
            VerifyShapeOverlap(shape);

            try
            {
                Console.WriteLine(shape.GetData());
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }

            ShowOverlapShapeList(shape);
        }

        private void VerifyShapeOverlap(IShape shape)
        {
            IEnumerable<IShape> OtherShapes = this.CurrentShapes.Where(s => s.GetId() != shape.GetId());

            foreach (IShape Other in OtherShapes)
            {
                if (shape.GetShapeType() == ShapeTypeEnum.Triangle)
                {
                    Triangle triangle = shape as Triangle;
                    Point point1 = triangle.Points[0];
                    Point point2 = triangle.Points[1];
                    Point point3 = triangle.Points[2];

                    if (Other.ShapeOverlaps(point1) || Other.ShapeOverlaps(point2) || Other.ShapeOverlaps(point3))
                    {
                        shape.AddOverlapShape(Other as Shape);
                    }
                }
                else
                {
                    Shape otherShapeRef = shape as Shape;
                    Point point = new Point((int)otherShapeRef.X + Canvas.RefX, (int)otherShapeRef.Y + Canvas.RefY);
                    if (Other.ShapeOverlaps(point))
                    {
                        shape.AddOverlapShape(Other as Shape);
                    }
                }
            }
        }

        private void ShowOverlapShapeList(IShape shape)
        {
            if (shape.GetOverlapShapes().Count() > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("=================================================");
                Console.WriteLine("The Following Shapes Overlap this {0} \n\n", shape.GetShapeType());
                double TotalArea = shape.GetArea();
                foreach (IShape OverlapShape in shape.GetOverlapShapes())
                {
                    try
                    {
                        TotalArea += OverlapShape.GetArea();
                        Console.WriteLine(OverlapShape.GetData());
                    }
                    catch (ArgumentNullException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                Console.WriteLine("\n\n TOTAL AREA: {0}", TotalArea);
                Console.WriteLine("=================================================");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }


    }
}
