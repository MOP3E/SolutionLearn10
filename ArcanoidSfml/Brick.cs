using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace ArcanoidSfml
{
    internal class Brick
    {
        /// <summary>
        /// Спрайт кирпича.
        /// </summary>
        private Sprite _sprite;

        /// <summary>
        /// Позиция кирпича на экране.
        /// </summary>
        public Vector2f Position;

        /// <summary>
        /// Размер кирпича.
        /// </summary>
        public Vector2Int Size => new(_sprite.TextureRect.Width, _sprite.TextureRect.Height);

        /// <summary>
        /// Очки жизни кирпича.
        /// </summary>
        public int Hitpoints;

        /// <summary>
        /// Создать кирпич.
        /// </summary>
        /// <param name="sprite">Спрайт кирпича.</param>
        /// <param name="position">Позиция кирпича.</param>
        public Brick(Sprite sprite, Vector2f position)
        {
            _sprite = sprite;
            Position = position;
        }

        /// <summary>
        /// Удар шариком по кирпичу. Возвращает истину если кирпич разбит.
        /// </summary>
        public bool Hit()
        {
            Hitpoints--;
            return Hitpoints <= 0;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _sprite.Position = Position;
            _sprite.Draw(target, states);
        }
    }
}
