using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IsoMap.Engine;
using Engine.CommonImagery;

namespace Engine.Tiles
{
    abstract public class ModelTile : ICollidable, IMapDrawable
    {
        public Rectangle SourceRectangle { get; set; }
        public bool crossable = false;


        public Rectangle HitBox
        {
            get => new Rectangle((int)CurrentPosition.X, (int)CurrentPosition.Y, 100, 100);//TODO
        }

        public Vector2 CurrentPosition { get; set; }
        public Vector2 BasePosition { get; set; }

        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int ZPosition { get; set; }
        public int ZOrder { get; set; }
        public int TileSheetNb { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public virtual void OnCollision(ICollidable other)
        {
            
        }

        public void Draw(SpriteBatch sb)
        {
            //sb.Draw(Map.tileset, new Vector2(CurrentPosition.X, CurrentPosition.Y), sourceRectangle, Color.White);
            //TODO ne pas oublier que le Draw doit se faire avec l'origine venant d'en bas!


        }

        public ModelTile(Vector2 basePosition, Rectangle sourceRectangle,int width, int height, int zOrder, int tileSheetNb)
        {
            BasePosition = basePosition;
            CurrentPosition = BasePosition;
            this.XPosition = (int)basePosition.X;
            this.YPosition = (int)basePosition.Y;
            //TODO attention double entre basePosition et C & Y position
            this.SourceRectangle = sourceRectangle;
            this.ZOrder = zOrder;
            this.TileSheetNb = tileSheetNb;
            this.Width = width;
            this.Height = height;
        }

        public void Update(int scrollX, int scrollY) {
            CurrentPosition = new Vector2(BasePosition.X - scrollX, BasePosition.Y - scrollY);
        }
    }
}
