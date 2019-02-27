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
        public bool Active { get; set; }

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

        public void Update()
        {
        }

        public void Draw()
        {
        }



    }
    public enum BeatType
    {
        MUSIC,
        RYTHM
    }
}
