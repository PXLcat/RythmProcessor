using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.CommonImagery;
using IsoMap.Engine;
using Microsoft.Xna.Framework;

namespace Engine.Tiles
{
    public class TileGround : ModelTile
    {
        public TileGround(Vector2 currentPosition, Rectangle sourceRectangle, int width, int height, int zOrder, int tileSheetNb)
            : base(currentPosition, sourceRectangle, width, height, zOrder, tileSheetNb)
        {
            
            Crossable = false;
        }
        public override void OnCollision(ICollidable other)
        {

        }
    }
}
