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
        /// Бита.
        /// </summary>
        private static Bat _bat;

        /// <summary>
        /// Кирпичи.
        /// </summary>
        private static List<Brick> _bricks;

        /// <summary>
        /// Текущие очки.
        /// </summary>
        private static int _score;

        /// <summary>
        /// Текущий уровень.
        /// </summary>
        private static int _level;

        /// <summary>
        /// Рекорд.
        /// </summary>
        private static int _record;

        /// <summary>
        /// Число запасных мячей.
        /// </summary>
        private static int _balls;

        /// <summary>
        /// Текущее состояние игры.
        /// </summary>
        private static GameState _state;

        /// <summary>
        /// Предыдущая позиция курсора мыши по оси Х.
        /// </summary>
        private static int _prevMousePos;

        /// <summary>
        /// Предыдущее состояние клавиши ECS.
        /// </summary>
        private static bool _prevEsc;
        
        /// <summary>
        /// Текст для вывода текста на экран.
        /// </summary>
        private static Text _text;

        static void Main(string[] args)
        {
            //инициализация окна
            _window = new RenderWindow(new VideoMode(800, 600), "Арканоид");
            _window.SetFramerateLimit(60);
            _window.Closed += _window_Closed;
            _window.SetMouseCursorVisible(false);
            _window.SetMouseCursorGrabbed(true);

            //загрузка спрайтов
            _redBrick = new Sprite(new Texture("red_brick.png"));
            _blueBrick = new Sprite(new Texture("blue_brick.png"));
            _greenBrick = new Sprite(new Texture("green_brick.png"));
            _ballSprite = new Sprite(new Texture("ball.png"));
            _batSprite = new Sprite(new Texture("bat.png"));

            //настройка игрового текста
            _text = new Text();
            _text.Font = new Font("comic.ttf");

            //настройка игрового поля
            _field = new IntRect(0, 30, 800, 570);
            
            //создание часов
            Clock clock = new();
            clock.Restart();
            _prevMousePos = Mouse.GetPosition(_window).X;

            _state = GameState.MainMenu;
            _record = 0;

            while (_window.IsOpen)
            {
                float deltaTime = clock.ElapsedTime.AsSeconds();
                clock.Restart();

                _window.Clear(Color.Black);

                _window.DispatchEvents();

                //проверка нажатий на кнопки мыши
                MouseState.ButtonsTest();

                //обнаружение нажатия на кнопку ESC
                bool esc = Keyboard.IsKeyPressed(Keyboard.Key.Escape);
                bool escPressed = _prevEsc && _prevEsc != esc;
                _prevEsc = esc;

                //быстрый выход из программы
                if (_state != GameState.MainMenu)
                {
                    //из любого режима игры по нажатию кнопки ECS всегда возврат в главное меню
                    if (escPressed)
                    {
                        _state = GameState.MainMenu;
                        //нажатие на кнопку перехвачено и обработано
                        escPressed = false;
                    }
                }

                //игровая логика
                switch (_state)
                {
                    case GameState.MainMenu:
                        //ждать щелчка мышью для перехода к началу игры
                        if (MouseState.LeftButtonPressed)
                        {
                            GameStart();
                        }
                        //по нажатию кнопки ECS выход из игры
                        if (escPressed)
                        {
                            _window.Close();
                            //нажатие на кнопку перехвачено и обработано
                            escPressed = false;
                        }
                        break;
                    case GameState.Start:
                        //ждать щелчка мышью для запуска игры
                        if (MouseState.LeftButtonPressed)
                        {
                            _ball.Speed = 400;
                            _state = GameState.Game;
                        }
                        break;
                    case GameState.Game:
                        //передвинуть мяч
                        _ball.Move(deltaTime);

                        //передвинуть биту
                        //int mousePos = Mouse.GetPosition(_window).X;
                        _bat.Move(_field, (int)(_bat.Sprite.Position.X + MouseState.LastMove.X));
                        //_prevMousePos = mousePos;

                        //проверить столкновения мяча
                        bool death = _ball.CollisionTest(_field, _bat, _bricks, out Brick crackedBrick);
                        
                        //проверить, не уничтожен ли кирпич
                        if (crackedBrick != null)
                        {
                            _score += crackedBrick.Score;
                            _bricks.Remove(crackedBrick);
                        }

                        //проверить, не умер ли мяч
                        if (death)
                        {
                            _balls--;
                            if (_balls >= 0)
                            {
                                //можно продолжать играть - разместить на поле новый мяч и новую биту
                                LevelStart(false);
                            }
                            else
                            {
                                //мячей больше не осталось - закончить игру
                                if (_score > _record)
                                {
                                    _record = _score;
                                    _state = GameState.GamoverRecord;
                                }
                                else
                                {
                                    _state = GameState.Gamover;
                                }
                            }
                        }
                        break;
                    case GameState.Gamover:
                    case GameState.GamoverRecord:
                        //ждать щелчка мышью для перехода в главное меню
                        if (MouseState.LeftButtonPressed)
                        {
                            _state = GameState.MainMenu;
                        }
                        break;
                }

                //отрисовка игрового экрана
                Draw();

                _window.Display();
            }
        }

        private static void _window_Closed(object? sender, EventArgs e)
        {
            _window.Close();
        }

        /// <summary>
        /// Начало новой игры.
        /// </summary>
        private static void GameStart()
        {
            _score = 0;
            _balls = 3;
            _level = 0;
            LevelStart(true);
        }

        /// <summary>
        /// Начало нового уровня.
        /// </summary>
        private static void LevelStart(bool newLevel)
        {
            if(newLevel) _level++;
            NewBall();
            NewBat();
            if(newLevel) GenerateLevel();
            _state = GameState.Start;
        }

        private static void Draw()
        {
            switch (_state)
            {
                case GameState.MainMenu:
                    //TODO: нарисовать главное меню
                    DrawText("АРКАНОИД", 72, Color.White, 195, 0 + 150);
                    DrawText($"Результат {_score}", 24, Color.White, 332, 90 + 150);
                    DrawText($"Рекорд {_record}", 24, Color.White, 348, 130 + 150);
                    DrawText("Щёлкните мышью для начала игры.", 24, Color.White, 186, 170 + 150);
                    DrawText("Нажмите [ESC] для выхода из игры.", 24, Color.White, 184, 210 + 150);
                    break;
                case GameState.Start:
                case GameState.Game:
                    //TODO: нарисовать оставшиеся мячики и текущие очки
                    _bat.Draw(_window, RenderStates.Default);
                    _ball.Draw(_window, RenderStates.Default);
                    _bat.Draw(_window, RenderStates.Default);
                    foreach (Brick brick in _bricks) brick.Draw(_window, RenderStates.Default);
                    break;
                case GameState.Gamover:
                    //TODO: нарисовать сообщение об окончании игры
                    break;
                case GameState.GamoverRecord:
                    //TODO: нарисовать сообщение об окончании игры с сообщением о рекорде
                    break;
            }
        }

        /// <summary>
        /// Создание уровня игры.
        /// </summary>
        private static void GenerateLevel()
        {
            _bricks = new List<Brick>();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    //синий кирпич имеет одно очко жизни
                    _bricks.Add(new(_blueBrick, 1, new Vector2f(j * 70 + 60, i * 25 + 60)));
                }
            }
        }

        /// <summary>
        /// Создание новой биты.
        /// </summary>
        private static void NewBat()
        {
            _bat = new Bat(_batSprite);
            _bat.Move(_field, 50f);
        }

        /// <summary>
        /// Создание нового мяча.
        /// </summary>
        private static void NewBall()
        {
            _ball = new Ball(_ballSprite, 0, (float)(Math.PI + Math.PI / 3));
            _ball.Sprite.Position = new Vector2f((_field.Width - _ballSprite.TextureRect.Width) / 2f, 500);
        }

        /// <summary>
        /// Вывод на экран текста.
        /// </summary>
        private static void DrawText(string text, uint size, Color color, int x, int y)
        {
            _text.DisplayedString = text;
            _text.CharacterSize = size;
            _text.FillColor = color;
            _text.Position = new Vector2f(x, y);
            _text.Draw(_window, RenderStates.Default);
        }
    }
}