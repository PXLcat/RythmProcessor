using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class MapImageLayer
    {

        #region Champs

        #endregion

        #region Propriétés
        public Texture2D Image { get; set; }
        public Point BasePosition { get; set; }
        public Point CurrentPosition { get; set; }
        public Point Size { get; set; }
        #endregion

        public MapImageLayer(Texture2D image, Point basePosition, Point size)
        {
            this.Image = image;
            this.BasePosition = basePosition;
            CurrentPosition = this.BasePosition; //devrait être dans le load si on l'utilise
            this.Size = size;

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
}
