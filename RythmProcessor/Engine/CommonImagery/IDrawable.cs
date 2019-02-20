using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.CommonImagery
{
    public interface IDrawable
    {
        Vector2 CurrentPosition { get; set; }
        Texture2D Texture { get; set; }

        void Draw(SpriteBatch sb);
    }
}
