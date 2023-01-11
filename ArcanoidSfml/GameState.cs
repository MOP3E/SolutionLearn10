using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanoidSfml
{
    internal enum GameState
    {
        /// <summary>
        /// Главное меню.
        /// </summary>
        MainMenu,
        /// <summary>
        /// Начало уровня.
        /// </summary>
        Start,
        /// <summary>
        /// Игра.
        /// </summary>
        Game,
        /// <summary>
        /// Конец игры.
        /// </summary>
        Gamover,
        /// <summary>
        /// Конец игры.
        /// </summary>
        GamoverRecord
    }
}
