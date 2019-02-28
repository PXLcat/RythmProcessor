using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RythmProcessor.Engine
{
    public class Beat
    {
        public BeatType type;
        public int BeatNumber { get; set; }
        public float DistanceFromRightBorder { get; set; }
        public bool Visible { get; set; }

        //méthode destroy des beat du tableau de int pour que le foreach passe plus dans le début du json?

        #region Champs

        #endregion

        #region Propriétés

        #endregion

        public Beat(BeatType type, int BeatNumber)
        {
            this.type = type;
            this.BeatNumber = BeatNumber;
            DistanceFromRightBorder = 0;



        }
        public void Load()
        {
        }

        public void Unload()
        {
        }

        public void Update(int currentBeat, int divisionDeTemps, float deltaTime, bool currentlyPlaying)
        {
            if (currentBeat > BeatNumber - divisionDeTemps * 4)
            {
                Visible = true;
            }
            else if (BeatNumber < currentBeat- divisionDeTemps)
            {
                Visible = false;
            }

            if (Visible && currentlyPlaying)
            {
                int vitesse = 2;  //comment/par rapport à quoi la définir? sûrement par rapport au tempo
                DistanceFromRightBorder += vitesse * deltaTime;
            }
        }

        public void Draw(SpriteBatch sb, Texture2D beatTexture, int hauteurBarre, int windowWidth, int zoom)
        {
            if (Visible)
            {
                sb.Draw(beatTexture, new Rectangle(windowWidth/zoom - (int)DistanceFromRightBorder, hauteurBarre, beatTexture.Width, beatTexture.Height), Color.White);
            }
        }

        
    }
    public enum BeatType
    {
        MUSIC,
        RYTHM
    }
}
