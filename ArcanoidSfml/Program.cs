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

        static void Main(string[] args)
        {
            _window = new RenderWindow(new VideoMode(800, 600), "Арканоид");
            _window.SetFramerateLimit(60);
            _window.Closed += _window_Closed;

            while(_window.IsOpen)
            {
                _window.Clear(Color.Cyan);

                _window.DispatchEvents();

                _window.Display();
            }
        }

        private static void _window_Closed(object? sender, EventArgs e)
        {
            _window.Close();
        }
    }
}