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
        public int DistanceFromRightBorder { get; set; }
        public decimal DecimalesDeplacement { get; set; }
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
            DecimalesDeplacement = 0;


        }
        public void Load()
        {
        }

        public void Unload()
        {
        }

        public void Update(int currentBeat, int bpm, int divisionDeTemps, float deltaTime, bool currentlyPlaying, int lineSize, int tempsDAvance) 
        {
            deltaTime /= 100;

            if (currentBeat > BeatNumber - divisionDeTemps * tempsDAvance)
            {
                Visible = true;
            }
            else if (BeatNumber < currentBeat- divisionDeTemps)
            {
                Visible = false;
            } //ça devrait être ailleurs, 1 méthode pour 1 utilité

            if (Visible && currentlyPlaying)
            {
                decimal distanceParTemps = lineSize / tempsDAvance;
                float bPS = bpm / 60f;
                decimal vitesse = distanceParTemps * (decimal)bPS;

                decimal deplacement = vitesse * (decimal)deltaTime;

                DecimalesDeplacement += deplacement - Math.Floor(deplacement);

                while (DecimalesDeplacement>=1)
                {
                    DecimalesDeplacement -= 1;
                    DistanceFromRightBorder += 1;
                }

                DistanceFromRightBorder += (int)Math.Floor(deplacement);
                //DistanceFromRightBorder étant un nombre à virgule, on stocke les décimales pour ne pas causer de problème avec le nombre de pixels qui doit être en int

                

            }
        }

        public void Draw(SpriteBatch sb, Texture2D musicTexture, Texture2D rythmTexture, int hauteurBarreMusic, int hauteurBarreRythme, int windowWidth, int zoom)
        {
            if (Visible)
            {
                switch (type)
                {
                    case BeatType.MUSIC:
                        sb.Draw(musicTexture, new Rectangle(windowWidth / zoom - (int)DistanceFromRightBorder, hauteurBarreMusic-2, rythmTexture.Width, rythmTexture.Height), Color.White);
                        break;
                    case BeatType.RYTHM:
                        sb.Draw(rythmTexture, new Rectangle(windowWidth / zoom - (int)DistanceFromRightBorder, hauteurBarreRythme-2, rythmTexture.Width, rythmTexture.Height), Color.White);
                        break;
                    default:
                        break;
                }

                
            }
        }
        public void Reset()
        {
            Visible = false;
            DistanceFromRightBorder = 0;
            DecimalesDeplacement = 0;
        }
        
    }
    public enum BeatType
    {
        MUSIC,
        RYTHM
    }
}
