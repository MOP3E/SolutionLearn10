using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace ArcanoidSfml
{
    internal class Ball
    {
        /// <summary>
        /// Пи
        /// </summary>
        private const float Pi = (float)(Math.PI);

        /// <summary>
        /// Пи * 2
        /// </summary>
        private const float Pi2 = (float)(Math.PI * 2);

        /// <summary>
        /// Пи * 3
        /// </summary>
        private const float Pi3 = (float)(Math.PI * 3);

        /// <summary>
        /// Спрайт мяча.
        /// </summary>
        public Sprite Sprite;

        /// <summary>
        /// Коллизии мяча.
        /// </summary>
        private List<BallCollision> _collisions = new()
        {
            new BallCollision(0, 11, 12, 1),
            new BallCollision(0, 10, 12, 2),
            new BallCollision(0, 9, 12, 3),
            new BallCollision(1, 8, 11, 4),
            new BallCollision(1, 7, 11, 5),
            new BallCollision(2, 6, 10, 6),
            new BallCollision(2, 5, 10, 7),
            new BallCollision(3, 4, 9, 8),
            new BallCollision(4, 3, 8, 9),
            new BallCollision(5, 2, 7, 10),
            new BallCollision(6, 2, 6, 10),
            new BallCollision(7, 1, 5, 11),
            new BallCollision(8, 1, 4, 11),
            new BallCollision(9, 0, 3, 12),
            new BallCollision(10, 0, 2, 12),
            new BallCollision(11, 0, 1, 12),
            new BallCollision(13, 0, -1, 12),
            new BallCollision(14, 0, -2, 12),
            new BallCollision(15, 0, -3, 12),
            new BallCollision(16, 1, -4, 11),
            new BallCollision(17, 1, -5, 11),
            new BallCollision(18, 2, -6, 10),
            new BallCollision(19, 2, -7, 10),
            new BallCollision(20, 3, -8, 9),
            new BallCollision(21, 4, -9, 8),
            new BallCollision(22, 5, -10, 7),
            new BallCollision(22, 6, -10, 6),
            new BallCollision(23, 7, -11, 5),
            new BallCollision(23, 8, -11, 4),
            new BallCollision(24, 9, -12, 3),
            new BallCollision(24, 10, -12, 2),
            new BallCollision(24, 11, -12, 1),
            new BallCollision(24, 13, -12, -1),
            new BallCollision(24, 14, -12, -2),
            new BallCollision(24, 15, -12, -3),
            new BallCollision(23, 16, -11, -4),
            new BallCollision(23, 17, -11, -5),
            new BallCollision(22, 18, -10, -6),
            new BallCollision(22, 19, -10, -7),
            new BallCollision(21, 20, -9, -8),
            new BallCollision(20, 21, -8, -9),
            new BallCollision(19, 22, -7, -10),
            new BallCollision(18, 22, -6, -10),
            new BallCollision(17, 23, -5, -11),
            new BallCollision(16, 23, -4, -11),
            new BallCollision(15, 24, -3, -12),
            new BallCollision(14, 24, -2, -12),
            new BallCollision(13, 24, -1, -12),
            new BallCollision(11, 24, 1, -12),
            new BallCollision(10, 24, 2, -12),
            new BallCollision(9, 24, 3, -12),
            new BallCollision(8, 23, 4, -11),
            new BallCollision(7, 23, 5, -11),
            new BallCollision(6, 22, 6, -10),
            new BallCollision(5, 22, 7, -10),
            new BallCollision(4, 21, 8, -9),
            new BallCollision(3, 20, 9, -8),
            new BallCollision(2, 19, 10, -7),
            new BallCollision(2, 18, 10, -6),
            new BallCollision(1, 17, 11, -5),
            new BallCollision(1, 16, 11, -4),
            new BallCollision(0, 15, 12, -3),
            new BallCollision(0, 14, 12, -2),
            new BallCollision(0, 13, 12, -1),
        };

        /// <summary>
        /// Левый перпендикуляр.
        /// </summary>
        private BallCollision _left = new(0, 12, -1, 1);

        /// <summary>
        /// Верхний перпендикуляр.
        /// </summary>
        private BallCollision _top = new(12, 0, 1, -1);

        /// <summary>
        /// Правый перпендикуляр.
        /// </summary>
        private BallCollision _right = new(24, 12, -1, 1);

        /// <summary>
        /// Нижний перпендикуляр.
        /// </summary>
        private BallCollision _bottom = new(12, 24, 1, -1);

        /// <summary>
        /// Приращение координат мяча за одну секунду.
        /// </summary>
        private Vector2 _increment;

        /// <summary>
        /// Скорость мяча.
        /// </summary>
        private float _speed;

        /// <summary>
        /// Скорость мяча, пикселей/с.
        /// </summary>
        public float Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                CalculateIncrement();
            }
        }

        /// <summary>
        /// Угол движения мяча, радиан.
        /// </summary>
        private float _angle;

        /// <summary>
        /// Угол движения мяча.
        /// </summary>
        public float Angle
        {
            get => _angle;

            set
            {
                _angle = value % 360f;
                CalculateIncrement();
            }
        }

        public Ball(Sprite sprite, float speed = 0, float angle = 0)
        {
            Sprite = sprite;
            Start(speed, angle);
        }

        /// <summary>
        /// Расчёт инкремента по координатам.
        /// </summary>
        private void CalculateIncrement()
        {
            _increment.X = (float)(_speed * Math.Cos(_angle));
            _increment.Y = (float)(_speed * Math.Sin(_angle));
        }

        /// <summary>
        /// Запуск мяча из новой позиции.
        /// </summary>
        public void Start(Vector2f position, float speed, float angle)
        {
            Sprite.Position = position;
            Start(speed, angle);
        }

        /// <summary>
        /// Запуск мяча из текущей позиции.
        /// </summary>
        public void Start(float speed, float angle)
        {
            _speed = speed;
            _angle = angle;
            CalculateIncrement();
        }

        /// <summary>
        /// Перемещение мяча.
        /// </summary>
        /// <param name="deltaTime">Промежуток времени, за который нужно переместить мяч, с.</param>
        public void Move(float deltaTime)
        {
            Sprite.Position = new Vector2f(Sprite.Position.X + _increment.X * deltaTime, Sprite.Position.Y + _increment.Y * deltaTime);
        }

        /// <summary>
        /// Проверка столкновения мяча. Возвращает истину, если мяч погиб.
        /// </summary>
        /// <param name="field">Параметры игрового поля.</param>
        /// <param name="bat">Бита.</param>
        /// <param name="bricks">Кирпичи.</param>
        /// <param name="brick">Кирпич, с котороым произошло столкновение.</param>
        public bool CollisionTest(IntRect field, Bat? bat, List<Brick> bricks, out Brick? brick)
        {
            brick = null;

            //проверить столкновение мяча со стенками и полом
            if (Sprite.Position.X < field.Left || Sprite.Position.X + Sprite.TextureRect.Width > field.Left + field.Width)
            {
                //столкновение с левой или правой стенкой, нужно отражать угол по горизонтали
                _increment.X = -_increment.X;
                _angle = Pi3 - _angle;
                if (_angle > Pi2) _angle -= Pi2;
                return false;
            }

            if (Sprite.Position.Y < field.Top || Sprite.Position.Y + Sprite.TextureRect.Height > field.Top + field.Height)
            {
                //столкновение с верхней или нижней стенкой, нужно отражать угол по вертикали
                _increment.Y = -_increment.Y;
                _angle = Pi2 - _angle;
                return false;
            }

            //проверить столкновение мяча с битой

            //проверить столкновение мяча с кирпичами
            //левая верхняя граница мяча
            Sprite s = Sprite;
            Vector2f ballRightLower = new Vector2f(Sprite.Position.X + Sprite.TextureRect.Width, Sprite.Position.Y + Sprite.TextureRect.Height);
            for (int i = 0; i < bricks.Count; i++)
            {
                //правая нижняя граница кирпича
                Vector2f rightLower = new Vector2f(bricks[i].Position.X + bricks[i].Size.X, bricks[i].Position.Y + bricks[i].Size.Y);
                //проверить, совпадают ли в пространстве кирпич и мяч
                if (ballRightLower.X < bricks[i].Position.X || ballRightLower.Y < bricks[i].Position.Y || Sprite.Position.X > rightLower.X || Sprite.Position.Y > rightLower.Y) continue;

                //проверить столкновение мяча с пллоскостью кирпича
                if (HitTest(_left.Position, bricks[i].Position, rightLower) || HitTest(_right.Position, bricks[i].Position, rightLower))
                {
                    //столкновение с плоскостью кирпича слева или справа, нужно отразить угол по горизонтали
                    brick = bricks[i];
                    _increment.X = -_increment.X;
                    _angle = Pi3 - _angle;
                    if (_angle > Pi2) _angle -= Pi2;
                    return false;
                }
                if (HitTest(_bottom.Position, bricks[i].Position, rightLower) || HitTest(_top.Position, bricks[i].Position, rightLower))
                {
                    //столкновение с плоскостью кирпича сверху или снизу, нужно отразить угол по вертикали
                    brick = bricks[i];
                    _increment.Y = -_increment.Y;
                    _angle = Pi2 - _angle;
                    return false;
                }

                //проверить столкновение мяча с окружностью кирпича
                foreach (BallCollision collision in _collisions)
                {
                    if (HitTest(collision.Position, bricks[i].Position, rightLower))
                    {
                        //столкновение с углом кирпича, нужно рассчитать угол отскока от упругой точки
                        brick = bricks[i];

                        //рассчитать угол отскока через синус угла между векторами
                        float angle = (float)Math.Asin((_increment.X * collision.Vector.Y - _increment.Y * collision.Vector.X) /
                                              (Math.Sqrt(_increment.X * _increment.X + _increment.Y * _increment.Y) *
                                               Math.Sqrt(collision.Vector.X * collision.Vector.X + collision.Vector.Y * collision.Vector.Y)));
                        _angle = _angle - angle * 2f + Pi;
                        CalculateIncrement();

                        return false;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Проверка попадания пикселя в границы кирпича.
        /// </summary>
        /// <param name="pixelPosition">Позиция пикселя.</param>
        /// <param name="brickPosition">Позиция спрайта кирпича.</param>
        /// <param name="rightLower">Правый нижний угол кирпича.</param>
        /// <returns></returns>
        private bool HitTest(Vector2Int pixelPosition, Vector2f brickPosition, Vector2f rightLower)
        {
            Vector2f testPosition = new Vector2f(Sprite.Position.X + pixelPosition.X, Sprite.Position.Y + pixelPosition.Y);
            return testPosition.X >= brickPosition.X && testPosition.Y >= brickPosition.Y && testPosition.X <= rightLower.X && testPosition.Y <= rightLower.Y;
        }

        /// <summary>
        /// Отрисовка мяча.
        /// </summary>
        public void Draw(RenderTarget target, RenderStates states)
        {
            Sprite.Draw(target, states);
        }
    }
}
