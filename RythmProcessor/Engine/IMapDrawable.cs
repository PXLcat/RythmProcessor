using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public interface IMapDrawable
    {
        /// <summary>
        /// Cordonnée carthésienne de la position sur une grille isométrique
        /// </summary>
        int XPosition { get; set; }

        /// <summary>
        /// Cordonnée carthésienne de la position sur une grille isométrique
        /// </summary>
        int YPosition { get; set; }

        /// <summary>
        /// Cordonnée carthésienne de la position (hauteur) sur une grille isométrique
        /// </summary>
        int ZPosition { get; set; }

        /// <summary>
        /// Détermine la distance en profondeur d'un sprite
        /// </summary>
        int ZOrder { get; set; }

    }
}
