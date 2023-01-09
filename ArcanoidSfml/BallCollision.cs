using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ArcanoidSfml
{
    /// <summary>
    /// Коллизия мяча.
    /// </summary>
    internal struct BallCollision
    {
        /// <summary>
        /// Позиция пикселя коллизии относительно верхнего левого угла спрайта мяча.
        /// </summary>
        public Vector2Int Position;

        /// <summary>
        /// Нормальный вектор коллизии.
        /// </summary>
        public Vector2Int Vector;

        public BallCollision(int coordX, int coordY, int vecotorX, int vectorY)
        {
            Position = new Vector2Int(coordX, coordY);
            Vector = new Vector2Int(vecotorX, vectorY);
        }
    }
}
