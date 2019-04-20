using Engine.CommonImagery;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RythmProcessor
{
    public class Utilities
    {

        //public static CollideType CheckCollision(ICollidable actor1, ICollidable actor2) //TODO la gravité devrait passer par là aussi ?
        //{
        //    CollideType result = new CollideType();

        //    if (actor1.HitBox.Intersects(actor2.HitBox))
        //    {
        //        if (actor1.HitBox.Top > actor2.HitBox.Bottom)
        //        {
        //            //collision par le haut
        //            result.collideTop = true;
        //            actor2.OnCollision(actor1);
        //            result.topCollisionDepth = actor1.HitBox.Top - actor1.HitBox.Bottom;
        //        }
        //        if (actor1.HitBox.Bottom > actor2.HitBox.Top)
        //        {
        //            //collision par le bas
        //            result.collideBottom = true;
        //            actor2.OnCollision(actor1);
        //            result.bottomCollisionDepth = actor1.HitBox.Bottom - actor2.HitBox.Top;
        //        }
        //        if (actor1.HitBox.Left > actor2.HitBox.Right)
        //        {
        //            //collision par la gauche
        //            result.collideLeft = true;
        //            actor2.OnCollision(actor1);
        //            result.leftCollisionDepth = actor1.HitBox.Left - actor1.HitBox.Right;
        //        }
        //        if (actor1.HitBox.Right > actor2.HitBox.Left)
        //        {
        //            //collision par la droite
        //            result.collideRight = true;
        //            actor2.OnCollision(actor1);
        //            result.rightCollisionDepth = actor1.HitBox.Right - actor1.HitBox.Left;
        //        }
        //    }

        //    return result;
        //}
        public static bool CheckCollision(ICollidable actor1, ICollidable actor2)
        {

        }

    }
    public class CollideType
    {
        public bool collideLeft;
        public bool collideRight;
        public bool collideTop;
        public bool collideBottom;

        public int leftCollisionDepth;
        public int rightCollisionDepth;
        public int topCollisionDepth;
        public int bottomCollisionDepth;

        public CollideType(bool collideLeft = false, bool collideRight = false, bool collideUp = false, bool collideDown = false)
        {
            this.collideLeft = collideLeft;
            this.collideRight = collideRight;
            this.collideTop = collideUp;
            this.collideBottom = collideDown;
        }
    }
}
