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
        /// Спрайты кирпича.
        /// </summary>
        private List<Sprite> _sprites;

        /// <summary>
        /// Позиция кирпича на экране.
        /// </summary>
        public Vector2f Position;

        /// <summary>
        /// Размер кирпича.
        /// </summary>
        public Vector2Int Size => new(_sprites[0].TextureRect.Width, _sprites[0].TextureRect.Height);

        /// <summary>
        /// Очки жизни кирпича.
        /// </summary>
        private int _hitpoints;

        /// <summary>
        /// Очки жизни кирпича.
        /// </summary>
        public int Hitpoints
        {
            get => _hitpoints;
            set
            {
                _hitpoints = value;
                Score = value;
            }
        }

        /// <summary>
        /// Очки за уничтожение кирпича.
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Создать кирпич.
        /// </summary>
        /// <param name="sprites">Спрайты кирпича.</param>
        /// <param name="hitpoints">Прочность кирпича.</param>
        /// <param name="position">Позиция кирпича.</param>
        public Brick(List<Sprite> sprites, int hitpoints, Vector2f position)
        {
            _sprites = sprites;
            Hitpoints = hitpoints;
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

        /// <summary>
        /// Отрисочка кирпича.
        /// </summary>
        public void Draw(RenderTarget target, RenderStates states)
        {
            switch (_hitpoints)
            {
                case 1:
                    _sprites[0].Position = Position;
                    _sprites[0].Draw(target, states);
                    break;
                case 2:
                    _sprites[1].Position = Position;
                    _sprites[1].Draw(target, states);
                    break;
                default:
                    _sprites[2].Position = Position;
                    _sprites[2].Draw(target, states);
                    break;
            }
        }
    }
}
