using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public interface ICollidable
    {
        Rectangle HitBox { get; }
        bool Crossable { get; set; }
        void OnCollision(ICollidable other);
    }
}

