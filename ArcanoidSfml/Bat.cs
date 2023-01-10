using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using SFML.Graphics;
using SFML.System;

namespace ArcanoidSfml
{
    /// <summary>
    /// Бита.
    /// </summary>
    internal class Bat
    {
        /// <summary>
        /// Спрайт биты.
        /// </summary>
        public Sprite Sprite;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprite">Спрайт биты.</param>
        public Bat(Sprite sprite)
        {
            Sprite = sprite;
        }

        /// <summary>
        /// Перемещение биты.
        /// </summary>
        /// <param name="field">Параметры игрового поля.</param>
        /// <param name="position">Положение биты по оси Х (0...100% от ширины поля).</param>
        public void Move(IntRect field, float position)
        {
            if (position > 100f) position = 100f;
            if (position < 0f) position = 0f;

            Move(field, (int)((field.Width - Sprite.TextureRect.Width) * position / 100f));
        }

        /// <summary>
        /// Перемещение биты.
        /// </summary>
        /// <param name="field">Параметры игрового поля</param>
        /// <param name="x">Новая позиция биты по оси X.</param>
        public void Move(IntRect field, int x)
        {
            if (x > field.Width - Sprite.TextureRect.Width) x = field.Width - Sprite.TextureRect.Width;
            if (x < 0) x = 0;
            Sprite.Position = new Vector2f(x, field.Height - Sprite.TextureRect.Height);
        }
    }
}
