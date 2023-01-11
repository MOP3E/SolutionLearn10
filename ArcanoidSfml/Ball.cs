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
        /// 3/2 Пи
        /// </summary>
        private const float Pi32 = (float)(Math.PI * 1.5);
        
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
            //нижняя правя часть мяча
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
            //нижняя левая часть мяча
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
        /// Зоны коллизий с левого конца биты.
        /// </summary>
        private Dictionary<IntRect, float> _batLeftTipCollisions = new()
        {
            { new IntRect(new Vector2i(0, 7), new Vector2i(1, 13)), (float)(Math.PI + Math.PI / 9.0) },
            { new IntRect(new Vector2i(1, 6), new Vector2i(1, 14)), (float)(Math.PI + Math.PI / 9.0) },
            { new IntRect(new Vector2i(2, 5), new Vector2i(1, 15)), (float)(Math.PI + Math.PI / 9.0) },
            { new IntRect(new Vector2i(3, 4), new Vector2i(1, 16)), (float)(Math.PI + Math.PI / 9.0) },
            { new IntRect(new Vector2i(4, 3), new Vector2i(1, 17)), (float)(Math.PI + Math.PI / 9.0) },
            { new IntRect(new Vector2i(5, 2), new Vector2i(1, 18)), (float)(Math.PI + Math.PI / 9.0) },
            { new IntRect(new Vector2i(6, 1), new Vector2i(1, 19)), (float)(Math.PI + Math.PI / 9.0) },
            { new IntRect(new Vector2i(7, 0), new Vector2i(1, 20)), (float)(Math.PI + Math.PI / 9.0) }
        };
        
        /// <summary>
        /// Зоны коллизий с правого конца биты.
        /// </summary>
        private Dictionary<IntRect, float> _batRightTipCollisions = new()
        {
            { new IntRect(new Vector2i(92, 0), new Vector2i(1, 20)), (float)(Math.PI + Math.PI / 9.0 * 8.0) },
            { new IntRect(new Vector2i(93, 1), new Vector2i(1, 19)), (float)(Math.PI + Math.PI / 9.0 * 8.0) },
            { new IntRect(new Vector2i(94, 2), new Vector2i(1, 18)), (float)(Math.PI + Math.PI / 9.0 * 8.0) },
            { new IntRect(new Vector2i(95, 3), new Vector2i(1, 17)), (float)(Math.PI + Math.PI / 9.0 * 8.0) },
            { new IntRect(new Vector2i(96, 4), new Vector2i(1, 16)), (float)(Math.PI + Math.PI / 9.0 * 8.0) },
            { new IntRect(new Vector2i(97, 5), new Vector2i(1, 15)), (float)(Math.PI + Math.PI / 9.0 * 8.0) },
            { new IntRect(new Vector2i(98, 6), new Vector2i(1, 14)), (float)(Math.PI + Math.PI / 9.0 * 8.0) },
            { new IntRect(new Vector2i(99, 7), new Vector2i(1, 13)), (float)(Math.PI + Math.PI / 9.0 * 8.0) }
        };

        /// <summary>
        /// Зоны коллизий по центру биты.
        /// </summary>
        private Dictionary<int, float> _batCollisions = new()
        {
            { 8, -1.22173047639603f },
            { 9, -1.18682389135614f },
            { 10, -1.15191730631626f },
            { 11, -1.11701072127637f },
            { 12, -1.08210413623648f },
            { 13, -1.0471975511966f },
            { 14, -1.01229096615671f },
            { 15, -0.977384381116825f },
            { 16, -0.942477796076938f },
            { 17, -0.907571211037051f },
            { 18, -0.872664625997165f },
            { 19, -0.837758040957278f },
            { 20, -0.802851455917392f },
            { 21, -0.767944870877505f },
            { 22, -0.733038285837618f },
            { 23, -0.698131700797732f },
            { 24, -0.663225115757845f },
            { 25, -0.628318530717959f },
            { 26, -0.593411945678072f },
            { 27, -0.558505360638185f },
            { 28, -0.523598775598299f },
            { 29, -0.488692190558412f },
            { 30, -0.453785605518526f },
            { 31, -0.418879020478639f },
            { 32, -0.383972435438752f },
            { 33, -0.349065850398866f },
            { 34, -0.314159265358979f },
            { 35, -0.279252680319093f },
            { 36, -0.244346095279206f },
            { 37, -0.20943951023932f },
            { 38, -0.174532925199433f },
            { 39, -0.139626340159546f },
            { 40, -0.10471975511966f },
            { 41, -0.0698131700797732f },
            { 42, -0.0349065850398866f },
            { 43, 0f },
            { 44, 0f },
            { 45, 0f },
            { 46, 0f },
            { 47, 0f },
            { 48, 0f },
            { 49, 0f },
            { 50, 0f },
            { 51, 0f },
            { 52, 0f },
            { 53, 0f },
            { 54, 0f },
            { 55, 0f },
            { 56, 0f },
            { 57, 0.0349065850398866f },
            { 58, 0.0698131700797732f },
            { 59, 0.10471975511966f },
            { 60, 0.139626340159546f },
            { 61, 0.174532925199433f },
            { 62, 0.20943951023932f },
            { 63, 0.244346095279206f },
            { 64, 0.279252680319093f },
            { 65, 0.314159265358979f },
            { 66, 0.349065850398866f },
            { 67, 0.383972435438752f },
            { 68, 0.418879020478639f },
            { 69, 0.453785605518526f },
            { 70, 0.488692190558412f },
            { 71, 0.523598775598299f },
            { 72, 0.558505360638185f },
            { 73, 0.593411945678072f },
            { 74, 0.628318530717959f },
            { 75, 0.663225115757845f },
            { 76, 0.698131700797732f },
            { 77, 0.733038285837618f },
            { 78, 0.767944870877505f },
            { 79, 0.802851455917392f },
            { 80, 0.837758040957278f },
            { 81, 0.872664625997165f },
            { 82, 0.907571211037051f },
            { 83, 0.942477796076938f },
            { 84, 0.977384381116825f },
            { 85, 1.01229096615671f },
            { 86, 1.0471975511966f },
            { 87, 1.08210413623648f },
            { 88, 1.11701072127637f },
            { 89, 1.15191730631626f },
            { 90, 1.18682389135614f },
            { 91, 1.22173047639603f },
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
        public bool CollisionTest(IntRect field, Bat bat, List<Brick> bricks, out Brick? brick)
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

            //правый нижний угол мяча
            Vector2f ballRightLower = new(Sprite.Position.X + Sprite.TextureRect.Width, Sprite.Position.Y + Sprite.TextureRect.Height);
            //правый нижний угол биты
            Vector2f batRightLower = new(bat.Sprite.Position.X + bat.Sprite.TextureRect.Width, bat.Sprite.Position.Y + bat.Sprite.TextureRect.Height);

            //проверить столкновение мяча с битой
            if (ballRightLower.Y >= bat.Sprite.Position.Y && ballRightLower.X >= bat.Sprite.Position.X && Sprite.Position.X <= batRightLower.X)
            {
                //проверка столкновения нижней точки с плоскостью биты
                float center = Sprite.Position.X + _bottom.Position.X;
                //плоскость биты начинается с 8 пикселя и заканчивается на 91 пикселе
                if (center >= bat.Sprite.Position.X + 8f && center <= bat.Sprite.Position.X + 91f)
                {
                    //попадание мяча в плоскость биты - отразить угол по вертикали либо задать новый согласно пикселю биты
                    int pixel = (int)Math.Round(center - bat.Sprite.Position.X);
                    _angle = _batCollisions[pixel] == 0 ? Pi2 - _angle : Pi32 + _batCollisions[pixel];
                    CalculateIncrement();
                    return false;
                }

                //проверить столкновение нижней полуокружности мяча с концами биты
                //32-47 - нижняя правая часть мяча
                for (int i = 32; i < 48; i++)
                {
                    foreach (KeyValuePair<IntRect, float> collision in _batLeftTipCollisions)
                    {
                        if (Sprite.Position.X + _collisions[i].Position.X < bat.Sprite.Position.X + _batLeftTipCollisions.Count &&
                            Sprite.Position.X + _collisions[i].Position.X >= bat.Sprite.Position.X + collision.Key.Left &&
                            Sprite.Position.Y + _collisions[i].Position.Y >= bat.Sprite.Position.Y + collision.Key.Top)
                        {
                            //отражение от конца биты всегда даёт угол в 30 градусов в соответствующую сторону
                            _angle = collision.Value;
                            CalculateIncrement();
                            return false;
                        }
                    }
                }

                //48-63 - нижняя левая часть мяча
                for (int i = 48; i < 64; i++)
                {
                    foreach (KeyValuePair<IntRect, float> collision in _batLeftTipCollisions)
                    {
                        if (Sprite.Position.X + _collisions[i].Position.X >= ballRightLower.X - _batRightTipCollisions.Count &&
                            Sprite.Position.X + _collisions[i].Position.X <= bat.Sprite.Position.X + collision.Key.Left &&
                            Sprite.Position.Y + _collisions[i].Position.Y >= bat.Sprite.Position.Y + collision.Key.Top)
                        {
                            //отражение от конца биты всегда даёт угол в 30 градусов в соответствующую сторону
                            _angle = collision.Value;
                            CalculateIncrement();
                            return false;
                        }
                    }
                }
            }

            //проверить столкновение мяча с кирпичами
            for (int i = 0; i < bricks.Count; i++)
            {
                //правая нижняя граница кирпича
                Vector2f rightLower = new(bricks[i].Position.X + bricks[i].Size.X, bricks[i].Position.Y + bricks[i].Size.Y);
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

                //проверить столкновение мяча с углом кирпича
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
            Vector2f testPosition = new(Sprite.Position.X + pixelPosition.X, Sprite.Position.Y + pixelPosition.Y);
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
