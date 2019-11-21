using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace Engine.CommonImagery
{
    public class AnimatedSprite : DrawableImage
    {
        /// <summary>
        /// Nombre de colonnes du spritesheet de l'animation. Compté à partir de 0.
        /// </summary>
        private int columns { get; set; }
        /// <summary>
        /// Nombre de lignes du spritesheet de l'animation. Compté à partir de 0.
        /// </summary>
        private int rows { get; set; }
        /// <summary>
        /// Un tableau donc chaque entrée correspond à la durée d'affichage de chaque frame
        /// </summary>
        private int[] framesSpeed { get; set; }
        public int CurrentFrame
        {
            get { return currentFrame; }
            set
            {
                if ((value < 0) || (value > columns*rows))
                    throw new Exception("Not a valid frame number for this sprite");
                else
                    currentFrame = value;
            }
        }
        /// <summary>
        /// Le timerFrame augmente à chaque Update. Lorsqu'il atteint la valeur de la durée de la frame, on passe à la frame (currentFrame) suivante.
        /// </summary>
        private float timerFrame;
        /// <summary>
        /// La frame de l'animation actuelle
        /// </summary>
        private int currentFrame;
        /// <summary>
        /// Sert à savoir, dans le cas d'une action qui ne boucle pas (ex: attaque), si l'animation a fini de s'afficher.
        /// </summary>
        public bool FirstLoopDone;

        public int FrameWidth, FrameHeight;

        /// <summary>
        /// Centre d'une frame (autour duquel se font les rotations par ex.)
        /// </summary>
        public Vector2 center;


        /// <summary>
        /// Constructeur de AnimatedSprite. Par défaut, l'origine du sprite sera en haut à droite de l'image.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="currentPosition"></param>
        /// <param name="columns">Nombre d'images du spritesheet</param>
        /// <param name="framespeed">La vitesse à laquelle on va passer d'une frame à une autre</param> //TODO on devrait récup direct le tableau et mettre GenerateFrameDurations dans la Factory
        public AnimatedSprite(Texture2D texture, Vector2 currentPosition, int columns, int rows, int[] framesSpeed, 
            Origin origin = Origin.LEFT_UP) : base(texture, currentPosition) //avec la lecture Json, le framespeed est de base mis à 0, donc on ne peut plus utiliser l'argument optionnel
        {
            this.columns = columns;
            this.rows = rows;
            this.framesSpeed = framesSpeed;
            

            CurrentFrame = 0;
            timerFrame = 0;
            FrameWidth = Texture.Width / this.columns;
            FrameHeight = Texture.Height / this.rows;


            switch (origin)
            {
                case Origin.CENTER:
                    center = new Vector2(FrameWidth / 2, FrameHeight / 2);
                    break;
                case Origin.LEFT_UP:
                    center = Vector2.Zero;
                    break;
                case Origin.MIDDLE_DOWN:
                    center = new Vector2(FrameWidth / 2, FrameHeight);
                    break;
                case Origin.MIDDLE_DOWN_ANCHORED:
                    center = new Vector2(FrameWidth / 2, FrameHeight-3);
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Passage d'une frame à une autre selon FrameSpeed
        /// </summary>
        public void Update(float deltaTime)
        {
            timerFrame += deltaTime * 9; //nombre arbitraire pour que ça corresponde à peu près à du 100ms 
            if (timerFrame >= framesSpeed[CurrentFrame])
            {
                CurrentFrame++;
                if (CurrentFrame == (columns*rows - 1))
                {
                    FirstLoopDone = true;
                }
                else if (CurrentFrame == columns * rows)
                {
                    CurrentFrame = 0;

                }
                timerFrame = 0;
            }
        }
        public override void Draw(SpriteBatch sb, bool horizontalFlip = false)
        {

            
            Rectangle sourceRectangle = new Rectangle((CurrentFrame % columns)* FrameWidth, (int)Math.Floor((double)CurrentFrame / columns) *FrameHeight, FrameWidth, FrameHeight);
            //Debug.WriteLine("Current Frame: " + CurrentFrame + " nbColumns: " + Columns);

            if (horizontalFlip)
                sb.Draw(Texture, CurrentPosition, sourceRectangle, Color.White, 0, center, 1, SpriteEffects.FlipHorizontally, LayerDepth);
            else
                sb.Draw(Texture, CurrentPosition, sourceRectangle, Color.White, 0, center, 1, SpriteEffects.None, LayerDepth);

        }
        public void BackToFirstFrame()
        {
            CurrentFrame = 0;
            timerFrame = 0;
            FirstLoopDone = false;
        }
    }

}
