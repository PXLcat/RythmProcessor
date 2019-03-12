using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Engine.CommonImagery
{
    public class AnimatedSprite : DrawableImage
    {
        /// <summary>
        /// Nombre de colonnes du spritesheet/de frames de l'animation. Compté à partir de 0.
        /// </summary>
        public int Columns { get; set; }
        /// <summary>
        /// Nombre de lignes du spritesheet/de frames de l'animation. Compté à partir de 0.
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// La vitesse (en nombre d'updates) à laquelle on va passer d'une frame à une autre
        /// </summary>
        public int FrameSpeed { get; set; }
        public int CurrentFrame
        {
            get { return currentFrame; }
            set
            {
                if ((value < 0) || (value > Columns*Rows))
                    throw new Exception("Not a valid frame number for this sprite");
                else
                    currentFrame = value;
            }
        }
        /// <summary>
        /// Sert à savoir, dans le cas d'une action qui ne boucle pas (ex: attaque), si l'animation a fini de s'afficher.
        /// </summary>
        public bool FirstLoopDone;
        /// <summary>
        /// Le compteurFrame augmente de 1 à chaque Update. Lorsqu'il atteint la valeur du FrameSpeed, on passe à la frame (currentFrame) suivante.
        /// </summary>
        private float compteurFrame;

        public int FrameWidth, FrameHeight;//TODO : public pour pouvoir gérer les collisions, mettre un accesseur?

        /// <summary>
        /// Millieu d'une frame
        /// </summary>
        public Vector2 center;

        private int currentFrame;

        /// <summary>
        /// Constructeur de AnimatedSprite à utiliser lorsque le xOffset est inconnu. Il se mettra de base à la moitié de la largeur du sprite.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="currentPosition"></param>
        /// <param name="columns">Nombre d'images du spritesheet</param>
        /// <param name="framespeed">La vitesse à laquelle on va passer d'une frame à une autre</param>
        public AnimatedSprite(Texture2D texture, Vector2 currentPosition, int columns, int rows, int framespeed) : base(texture, currentPosition) 
        {
            Columns = columns;
            Rows = rows;
            FrameSpeed = framespeed; //avec la lecture Json, le framespeed est de base mis à 0, donc on ne peut plus utiliser l'argument optionnel

            CurrentFrame = 0;
            compteurFrame = 0;
            FrameWidth = Texture.Width / Columns;
            FrameHeight = Texture.Height/Rows;

            //on met l'origine de l'image au point en bas au millieu
            center = new Vector2(FrameWidth / 2, FrameHeight);
        }

        /// <summary>
        /// Passage d'une frame à une autre selon FrameSpeed
        /// </summary>
        public void Update(float deltaTime)
        {
            compteurFrame += deltaTime;//demander à Gaët si ça marche 
            if (compteurFrame >= FrameSpeed)
            {
                CurrentFrame++;
                if (CurrentFrame == (Columns*Rows - 1))
                {
                    FirstLoopDone = true;
                }
                else if (CurrentFrame == Columns * Rows)
                {
                    CurrentFrame = 0;

                }
                compteurFrame = 0;
            }
        }
        public override void Draw(SpriteBatch sb, bool horizontalFlip = false)
        {


            Rectangle sourceRectangle = new Rectangle((CurrentFrame % (Columns-1))* FrameWidth, (CurrentFrame % Rows)*FrameHeight, FrameWidth, FrameHeight);
            int layerDepth = 0; //TODO attention à layer depth, à ajouter comme para plus tard
            Vector2 drawPosition = new Vector2(CurrentPosition.X, CurrentPosition.Y + 8); //permet de donner l'impression que le sprite est ancré dans le sol et non en train de flotter au dessus


            //Debug.WriteLine("Current Frame: " + CurrentFrame + " nbColumns: " + Columns);
            if (horizontalFlip)
                sb.Draw(Texture, drawPosition, sourceRectangle, Color.White, 0, center, 1, SpriteEffects.FlipHorizontally, layerDepth);
            else
                sb.Draw(Texture, drawPosition, sourceRectangle, Color.White, 0, center, 1, SpriteEffects.None, layerDepth);

        }
    }
}
