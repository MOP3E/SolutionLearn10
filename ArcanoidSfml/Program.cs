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
        /// Спрайты кирпичей.
        /// </summary>
        private static List<Sprite> _brickSprites;

        /// <summary>
        /// Спрайт мяча.
        /// </summary>
        private static Sprite _ballSprite;

        /// <summary>
        /// Спрайт биты.
        /// </summary>
        private static Sprite _batSprite;

        /// <summary>
        /// Настройки уровней.
        /// </summary>
        private static List<LevelSettings> _levels = new()
        {
            new LevelSettings(0,0),
            new LevelSettings(10,0),
            new LevelSettings(15,0),
            new LevelSettings(20,0),
            new LevelSettings(25,0),
            new LevelSettings(30,10),
            new LevelSettings(35,15),
            new LevelSettings(40,20),
            new LevelSettings(45,25),
            new LevelSettings(50,30),
        };
        
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
        private static Balls _balls;

        /// <summary>
        /// Текущее состояние игры.
        /// </summary>
        private static GameState _state;

        /// <summary>
        /// Предыдущее состояние клавиши ECS.
        /// </summary>
        private static bool _prevEsc;
        
        /// <summary>
        /// Текст для вывода текста на экран.
        /// </summary>
        private static Text _text;

        /// <summary>
        /// Игровой бог.
        /// </summary>
        private static Random _random;

        static void Main(string[] args)
        {
            //молитва игровому богу
            _random = new Random((int)(DateTime.Now.Ticks & 0xFFFFFFFF));

            //инициализация окна
            _window = new RenderWindow(new VideoMode(800, 600), "Арканоид");
            _window.SetFramerateLimit(60);
            _window.Closed += _window_Closed;
            _window.SetMouseCursorVisible(false);
            _window.SetMouseCursorGrabbed(true);

            //загрузка спрайтов
            _brickSprites = new()
            {
                new Sprite(new Texture("blue_brick.png")),
                new Sprite(new Texture("green_brick.png")),
                new Sprite(new Texture("red_brick.png")),
            };
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
                        }
                        break;
                    case GameState.Start:
                        //ждать щелчка мышью для запуска игры
                        if (MouseState.LeftButtonPressed)
                        {
                            _ball.Speed = 400;
                            _state = GameState.Game;
                        }

                        //передвинуть биту
                        _bat.Move(_field, Mouse.GetPosition(_window).X);
                        break;
                    case GameState.Game:
                        //передвинуть мяч
                        _ball.Move(deltaTime);

                        //передвинуть биту
                        _bat.Move(_field, Mouse.GetPosition(_window).X);

                        //проверить столкновения мяча
                        bool death = _ball.CollisionTest(_field, _bat, _bricks, out Brick crackedBrick);
                        
                        //проверить, не уничтожен ли кирпич
                        if (crackedBrick != null && crackedBrick.Hit())
                        {
                            _score += crackedBrick.Score;
                            _bricks.Remove(crackedBrick);
                            //проверить, не пройден ли уровень
                            if (_bricks.Count <= 0)
                            {
                                //перейти на следующий уровень
                                LevelStart(true);
                            }
                        }

                        //проверить, не умер ли мяч
                        if (death)
                        {
                            if (!_balls.Death())
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

        /// <summary>
        /// Событие нажатия на кнопку закрытия окна.
        /// </summary>
        private static void _window_Closed(object sender, EventArgs e)
        {
            _window.Close();
        }

        /// <summary>
        /// Начало новой игры.
        /// </summary>
        private static void GameStart()
        {
            _score = 0;
            _balls = new Balls(3);
            _level = 0;
            LevelStart(true);
        }

        /// <summary>
        /// Начало нового уровня, либо переинициализация текущего.
        /// </summary>
        /// <param name="newLevel">Истина - это новый уровень.</param>
        private static void LevelStart(bool newLevel)
        {
            if(newLevel)
            {
                _level++;
                //проверить, не закончились ли уровни?
                if(_level > _levels.Count)
                {
                    //уровни закончились - конец игры
                    _state = _score > _record ? GameState.GamoverRecord : GameState.Gamover;
                    return;
                }
            }
            NewBall();
            NewBat();
            if(newLevel) GenerateLevel();
            _state = GameState.Start;
        }

        /// <summary>
        /// Отрисовка игрового экрана.
        /// </summary>
        private static void Draw()
        {
            switch (_state)
            {
                case GameState.MainMenu:
                    //нарисовать главное меню
                    int verticalOffset = 150;
                    DrawText("АРКАНОИД", 72, Color.White, 195, 0 + verticalOffset);
                    DrawText($"Результат {_score}", 24, Color.White, 332, 90 + verticalOffset);
                    DrawText($"Рекорд {_record}", 24, Color.White, 348, 130 + verticalOffset);
                    DrawText("Щёлкните мышью для начала игры.", 24, Color.White, 186, 170 + verticalOffset);
                    DrawText("Нажмите [ESC] для выхода из игры.", 24, Color.White, 184, 210 + verticalOffset);
                    break;
                case GameState.Start:
                case GameState.Game:
                    //нарисовать оставшиеся мячики, номер уровня и текущие очки
                    Vector2f ballPosition = _ball.Sprite.Position;
                    for (int i = 0; i < _balls.Count; i++)
                    {
                        _ball.Sprite.Position = new Vector2f(i * 37 + 10, 10);
                        _ball.Draw(_window, RenderStates.Default);
                    }
                    _ball.Sprite.Position = ballPosition;
                    DrawText($"{_score}", 24, Color.White, 740, 8);
                    DrawText($"Уровень {_level}", 24, Color.White, 342, 8);

                    //нарисовать мяч и биту
                    _bat.Draw(_window, RenderStates.Default);
                    _ball.Draw(_window, RenderStates.Default);

                    //нарисовать подсказку
                    if(_state == GameState.Start) DrawText("Щёлкните мышью для запуска мяча.", 24, Color.White, 182, 382);

                    //нарисовать кирпичи
                    foreach (Brick brick in _bricks) brick.Draw(_window, RenderStates.Default);
                    break;
                case GameState.Gamover:
                    //нарисовать сообщение об окончании игры
                    DrawText("ИГРА ОКОНЧЕНА", 72, Color.White, 89, 210);
                    DrawText($"Ваши очки {_score}", 24, Color.White, 319, 300);
                    DrawText("Нажмите [ESC] для возврата в меню.", 24, Color.White, 182, 340);
                    break;
                case GameState.GamoverRecord:
                    //нарисовать сообщение об окончании игры с сообщением о рекорде
                    DrawText("ИГРА ОКОНЧЕНА", 72, Color.White, 89, 190);
                    DrawText($"Ваши очки {_score}", 24, Color.White, 319, 280);
                    DrawText("Поздравляем, вы побили рекорд!", 24, Color.White, 202, 320);
                    DrawText("Нажмите [ESC] для возврата в меню.", 24, Color.White, 182, 360);
                    break;
            }
        }

        /// <summary>
        /// Создание уровня игры.
        /// </summary>
        private static void GenerateLevel()
        {
            //заполнение поля синими кирпичами
            _bricks = new List<Brick>();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    //синий кирпич имеет одно очко жизни
                    _bricks.Add(new(_brickSprites, 1, new Vector2f(j * 70 + 60, _field.Top + i * 25 + 60)));
                }
            }

            //превращение синих кирпичей в зелёные и красные
            List<int> greens = new();
            List<int> reds = new();
            List<int> numbers = new();
            for (int i = 0; i < 100; i++) numbers.Add(i);
            for (int i = 0; i < _levels[_level-1].Greens; i++)
            {
                int pos = _random.Next(numbers.Count);
                greens.Add(numbers[pos]);
                numbers.RemoveAt(pos);
            }
            for (int i = 0; i < _levels[_level-1].Reds; i++)
            {
                int pos = _random.Next(numbers.Count);
                reds.Add(numbers[pos]);
                numbers.RemoveAt(pos);
            }
            //зелёный кирпич имеет два очка жизни
            foreach (int green in greens) _bricks[green].Hitpoints = 2;
            //красный кирпич имеет три очка жизни
            foreach (int red in reds) _bricks[red].Hitpoints = 3;
        }

        /// <summary>
        /// Создание новой биты.
        /// </summary>
        private static void NewBat()
        {
            _bat = new Bat(_batSprite);
            _bat.Move(_field, Mouse.GetPosition(_window).X);
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