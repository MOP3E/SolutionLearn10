using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ArcanoidSfml
{
    internal class Ball
    {
        /// <summary>
        /// Координата мяча на экране.
        /// </summary>
        public Vector2Int Coord;

        /// <summary>
        /// Размер мяча.
        /// </summary>
        public Vector2Int Size;

        /// <summary>
        /// Скорость мяча.
        /// </summary>
        public Vector2 Speed;

        /// <summary>
        /// Угол движения мяча.
        /// </summary>
        public float Angle
        {
            get => Speed.Y >= 0 ? (float)Math.Acos(Speed.X / Speed.Length()) : 360f - (float)Math.Acos(Speed.X / Speed.Length());

            set
            {
                double speed = Speed.Length();
                float angle = value % 360f;
                Speed.X = (float)(speed * Math.Cos(angle));
                Speed.Y = (float)(speed * Math.Sin(angle));
            }
        }

        /// <summary>
        /// Ускорение мяча.
        /// </summary>
        public float Acceleration;

        public Ball()
        {
            //Speed.
        }
    }
}
