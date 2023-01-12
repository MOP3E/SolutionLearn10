using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanoidSfml
{
    /// <summary>
    /// Настройки уровня арканоида.
    /// </summary>
    internal struct LevelSettings
    {
        /// <summary>
        /// Количество зелёных кирпичей.
        /// </summary>
        public int Greens;

        /// <summary>
        /// Количество красных кирпичей.
        /// </summary>
        public int Reds;

        public LevelSettings(int greens, int reds)
        {
            Greens = greens;
            Reds = reds;
        }
    }
}
