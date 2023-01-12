using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanoidSfml
{
    /// <summary>
    /// Запасные мячи игрока.
    /// </summary>
    internal class Balls
    {
        /// <summary>
        /// Текущее количество запасных мячей.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="count">Число запасных мячей.</param>
        public Balls(int count)
        {
            Count = count;
        }

        /// <summary>
        /// Смерть меча. Возвращает истину, если мяч окончательно умер (запасных мячей не осталось).
        /// </summary>
        /// <returns></returns>
        public bool Death()
        {
            Count--;
            return Count < 0;
        }
    }
}
