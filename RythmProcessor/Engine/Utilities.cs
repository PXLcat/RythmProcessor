using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Utilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor1">généralement le perso</param>
        /// <param name="actor2"></param>
        /// <returns></returns>
        public static CollideType CheckCollision(ICollidable actor1, ICollidable actor2)
        {

            CollideType result = new CollideType();

            if (actor1.HitBox.Intersects(actor2.HitBox))
            {

                if (actor1.HitBox.Top > actor2.HitBox.Bottom)
                {
                    //collision par le haut
                    result.collideTop = true;
                    actor2.OnCollision(actor1);
                    result.topCollisionDepth = actor2.HitBox.Bottom - actor1.HitBox.Top;
                }
                if (actor1.HitBox.Bottom > actor2.HitBox.Top)
                {
                    //collision par le bas
                    result.collideBottom = true;
                    actor2.OnCollision(actor1);
                    result.bottomCollisionDepth = actor1.HitBox.Bottom - actor2.HitBox.Top;
                }
            }
            Rectangle xCollisionHitBox = new Rectangle(actor1.HitBox.X,
                actor1.HitBox.Y,
                actor1.HitBox.Width,
                actor1.HitBox.Height - 8);

            if (xCollisionHitBox.Intersects(actor2.HitBox))
            {
                if ((xCollisionHitBox.Left < actor2.HitBox.Right) && (xCollisionHitBox.Center.X > actor2.HitBox.Center.X)) //!
                {
                    //collision par la gauche
                    result.collideLeft = true;
                    actor2.OnCollision(actor1);
                    result.leftCollisionDepth = actor2.HitBox.Right - xCollisionHitBox.Left;
                }
                if ((xCollisionHitBox.Right > actor2.HitBox.Left) && (xCollisionHitBox.Center.X < actor2.HitBox.Center.X))
                {
                    //collision par la droite
                    result.collideRight = true;
                    actor2.OnCollision(actor1);
                    result.rightCollisionDepth = xCollisionHitBox.Right - actor2.HitBox.Left;
                }
            }
            return result;
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
