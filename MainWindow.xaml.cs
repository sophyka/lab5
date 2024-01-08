using System.Collections.ObjectModel;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TurtleGraphics;
using Color = System.Windows.Media.Color;
using Line = System.Windows.Shapes.Line;

namespace _5labWPF
{
    public partial class MainWindow : Window
    {
        public Turtle TurtleView { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            TurtleView = new Turtle();
            DataContext = this;

        }

        private void ExecuteButton_OnClick(object sender, RoutedEventArgs e)
        {
            string command = commandTextBox.Text;
            ExecuteCommand(command);
            commandTextBox.Text = string.Empty;
        }

        private void ExecuteCommand(string command)
        {
            try
            {
                TurtleView.ProcessCommand(command);
                DrawCanvas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения команды: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DrawCanvas()
        {
            drawingCanvas.Children.Clear();
            double offsetX = drawingCanvas.ActualWidth / 2;
            double offsetY = drawingCanvas.ActualHeight / 2;
            foreach (var temp in TurtleView.getLines())
            {
                Color color;
                switch (temp.Color)
                {
                    case "White":
                        color = Colors.White;
                        break;
                    case "Red":
                        color = Colors.Red;
                        break;
                    case "Green":
                        color= Colors.Green;
                        break;
                    case "Blue":
                        color= Colors.Blue;
                        break;
                    default:
                        color = Colors.Black;
                        break;
                }
                Line line = new Line
                {
                    X1 = temp.StartDot.X+offsetX,
                    Y1 = temp.StartDot.Y*(-1)+offsetY,
                    X2 = temp.EndDot.X+offsetX,
                    Y2 = temp.EndDot.Y*(-1)+offsetY,
                    Stroke = new SolidColorBrush(color),
                    StrokeThickness = 2
                };
                drawingCanvas.Children.Add(line);
            }
            Ellipse brush = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Black
            };

            Canvas.SetLeft(brush, TurtleView.x - brush.Width / 2 + offsetX);
            Canvas.SetTop(brush, TurtleView.y * (-1) - brush.Height / 2 + offsetY);

            drawingCanvas.Children.Add(brush);

            TextBlock textBlock = new TextBlock
            {
                Text = $"Угол: {TurtleView.angle}\n(X:Y): ({TurtleView.x}, {TurtleView.y})",
                Foreground = Brushes.Black,
                FontSize = 12
            };

            Canvas.SetLeft(textBlock, TurtleView.x + 15 + offsetX);
            Canvas.SetTop(textBlock, TurtleView.y * (-1) - 15 + offsetY);

            drawingCanvas.Children.Add(textBlock);
        }
    }
}