using System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using SFML.System;

namespace ArcanoidSfml
{
    internal class Program
    {
        private static RenderWindow _window;

        private static CircleShape _circle;

        static void Main(string[] args)
        {
            _window = new RenderWindow(new VideoMode(800, 600), "Арканоид");
            _window.SetFramerateLimit(60);
            _window.Closed += _window_Closed;

            _circle = new CircleShape(10);
            _circle.FillColor = Color.Magenta;
            _circle.Position = new Vector2f(100, 100);

            Console.WriteLine($"x = 10, y = 10, alphaX = {180.0/Math.PI * Math.Acos(10.0/Math.Sqrt(200.0))}, alphaY = {180.0/Math.PI * Math.Asin(10.0/Math.Sqrt(200.0))}");
            Console.WriteLine($"x = -10, y = 10, alphaX = {180.0/Math.PI * Math.Acos(-10.0/Math.Sqrt(200.0))}, alphaY = {180.0/Math.PI * Math.Asin(10.0/Math.Sqrt(200.0))}");
            Console.WriteLine($"x = -10, y = -10, alphaX = {180.0/Math.PI * Math.Acos(-10.0/Math.Sqrt(200.0))}, alphaY = {180.0/Math.PI * Math.Asin(-10.0/Math.Sqrt(200.0))}");
            Console.WriteLine($"x = 10, y = -10, alphaX = {180.0/Math.PI * Math.Acos(10.0/Math.Sqrt(200.0))}, alphaY = {180.0/Math.PI * Math.Asin(-10.0/Math.Sqrt(200.0))}");

            while(_window.IsOpen)
            {
                _window.Clear(Color.Cyan);

                _window.DispatchEvents();

                _circle.Draw(_window, RenderStates.Default);

                _window.Display();
            }
        }

        private static void _window_Closed(object? sender, EventArgs e)
        {
            _window.Close();
        }
    }
}