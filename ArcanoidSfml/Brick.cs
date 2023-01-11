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
        private int _hitpoints;

        /// <summary>
        /// Очки за уничтожение кирпича.
        /// </summary>
        public readonly int Score;

        /// <summary>
        /// Создать кирпич.
        /// </summary>
        /// <param name="sprite">Спрайт кирпича.</param>
        /// <param name="hitpoints">Прочность кирпича.</param>
        /// <param name="position">Позиция кирпича.</param>
        public Brick(Sprite sprite, int hitpoints, Vector2f position)
        {
            _sprite = sprite;
            _hitpoints = hitpoints;
            Score = hitpoints;
            Position = position;
        }

        /// <summary>
        /// Удар шариком по кирпичу. Возвращает истину если кирпич разбит.
        /// </summary>
        public bool Hit()
        {
            _hitpoints--;
            return _hitpoints <= 0;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _sprite.Position = Position;
            _sprite.Draw(target, states);
        }
    }
}
