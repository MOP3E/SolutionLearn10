using System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using SFML.System;

namespace ArcanoidSfml
{
    internal class Program
    {
        /// <summary>
        /// Окно отрисовки.
        /// </summary>
        private static RenderWindow _window;

        /// <summary>
        /// Красный кирпич.
        /// </summary>
        private static Sprite _redBrick;

        /// <summary>
        /// Зелёный кирпич.
        /// </summary>
        private static Sprite _greenBrick;
        
        /// <summary>
        /// Синий кирпич.
        /// </summary>
        private static Sprite _blueBrick;

        /// <summary>
        /// Спрайт мяча.
        /// </summary>
        private static Sprite _ballSprite;

        /// <summary>
        /// Спрайт биты.
        /// </summary>
        private static Sprite _batSprite;

        /// <summary>
        /// Параметры игрового поля.
        /// </summary>
        private static IntRect _field;

        /// <summary>
        /// Мяч.
        /// </summary>
        private static Ball _ball;

        /// <summary>
        /// Кирпичи.
        /// </summary>
        private static List<Brick> _bricks;

        static void Main(string[] args)
        {
            //инициализация окна
            _window = new RenderWindow(new VideoMode(800, 600), "Арканоид");
            _window.SetFramerateLimit(60);
            _window.Closed += _window_Closed;

            //загрузка спрайтов
            _redBrick = new Sprite(new Texture("red_brick.png"));
            _blueBrick = new Sprite(new Texture("blue_brick.png"));
            _greenBrick = new Sprite(new Texture("green_brick.png"));
            _ballSprite = new Sprite(new Texture("ball.png")) { Position = new Vector2f(300, 270) };
            _batSprite = new Sprite(new Texture("bat.png"));

            //создание игрового поля
            _field = new IntRect(0, 0, 800, 600);

            //создание мяча
            _ball = new Ball(_ballSprite, 5, (float)(Math.PI / 4 * 1));

            //создание кирпичей
            _bricks = new List<Brick>
            {
                new(_blueBrick, new Vector2f(300, 300))
            };


            //игровые часы
            Clock clock = new Clock();
            clock.Restart();
            
            while (_window.IsOpen)
            {
                float deltaTime = clock.ElapsedTime.AsSeconds();
                clock.Restart();

                _window.Clear(Color.Black);

                _window.DispatchEvents();

                _ball.Move(deltaTime);

                Brick? crackedBrick = null;
                _ball.CollisionTest(_field, null, _bricks, out crackedBrick);

                _ball.Draw(_window, RenderStates.Default);
                foreach (Brick brick in _bricks) brick.Draw(_window, RenderStates.Default);

                _window.Display();
            }
        }

        private static void _window_Closed(object? sender, EventArgs e)
        {
            _window.Close();
        }
    }
}