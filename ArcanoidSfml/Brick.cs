using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanoidSfml
{
    internal class Brick
    {
        /// <summary>
        /// Координата кирпича на экране.
        /// </summary>
        public Vector2Int Coord;

        /// <summary>
        /// Размер кирпича.
        /// </summary>
        public Vector2Int Size;

        /// <summary>
        /// Очки жизни кирпича.
        /// </summary>
        public int Hitpoints;
    }
}
