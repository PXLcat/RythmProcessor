using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.CommonImagery
{
    public class DrawableImage //avec origine au millieu
    {
        public Texture2D Texture { get; set; }
        public Vector2 CurrentPosition { get; set; }
        private Vector2 center;
        public int LayerDepth { get; set; }


        /// <summary>
        /// Constructeur de DrawableImage
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="currentPosition"></param>
        /// <param name="yOffset"></param>
        public DrawableImage(Texture2D texture, Vector2 currentPosition)
        {
            Texture = texture;
            CurrentPosition = currentPosition;
            center = new Vector2(Texture.Width / 2, Texture.Height / 2);

            LayerDepth = 0; //TODO rendre modifiable
        }

        public virtual void Draw(SpriteBatch sb, bool horizontalFlip = false)
        {
            Debug.WriteLine("Méthode Draw de DrawableImage pour " + Texture.Name);
            if (horizontalFlip)
                sb.Draw(Texture, CurrentPosition, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, 0,
                    center, 1, SpriteEffects.FlipHorizontally, LayerDepth);
            else
                sb.Draw(Texture, CurrentPosition, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, 0,
                    center, 1, SpriteEffects.None, LayerDepth);
        }

        public virtual void DrawTiled(SpriteBatch sb, int horizontalTilesNb, int verticalTilesNb, bool horizontalFlip = false)
        {
            for (int i = 0; i < verticalTilesNb; i++) 
            {
                for (int y = 0; y < horizontalTilesNb; y++)
                {
                    //n'utilise pas le centre, va se caler sur le point en haut à gauche
                    sb.Draw(Texture, new Rectangle((int)CurrentPosition.X+y* Texture.Width,
                        (int)CurrentPosition.Y+i* Texture.Height, Texture.Width, Texture.Height), Color.White); 
                }                
            }
            

        } 

    }
}
