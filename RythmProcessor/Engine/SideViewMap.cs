using Engine.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace Engine
{
    public class SideViewMap
    {
        public TmxMap sideSnowMap;
        Dictionary<string, Texture2D> tilesetsTextures;

        Point originTileCoord; //à utiliser pour les Tilesets 32x16
        int tileOrBlockWidth;
        Point tileSize;


        List<ModelTile> mapElements;

        // TODO : faire une fonction pour déterminer si un tileset a des tiles "plates" ou "block" et utiliser pour savoir quoi charger dans le Laod

        public void Load(ContentManager contentManager)
        {
            sideSnowMap = new TmxMap("Content/mapneigeside.tmx");
            tilesetsTextures = new Dictionary<string, Texture2D>
            {
                { "neige side", contentManager.Load<Texture2D>(sideSnowMap.Tilesets[0].Name) },//se référer à l'ordre dans le xml
            };//TODO générer par Factory
            sideSnowMap.Layers[]

            tileOrBlockWidth = sideSnowMap.Tilesets[0].TileWidth;
            tileSize = new Point(sideSnowMap.Tilesets[0].TileWidth, sideSnowMap.Tilesets[0].TileHeight);


            //originTileCoord = new Point(snowMap.Tilesets[0].TileWidth * (snowMap.Width / 4) - snowMap.Tilesets[0].TileWidth / 2, 0);
            originTileCoord = new Point(0, -2); //à mettre ailleurs

            FillMapElements();

        }
        public void FillMapElements()
        {
            mapElements = new List<ModelTile>();

            for (int i = 0; i < sideSnowMap.Layers.Count; i++)// Pour chaque layer
            {
                int orthogonalX = 0;
                int orthogonalY = 0;

                string layerNameZ = sideSnowMap.Layers[i].Name.Substring(0, 2);

                ///A "true" si le nom du layer commence bien par deux chifres. layerZ représente la hauteur en blocs du layer
                bool correctLayerName = Int32.TryParse(layerNameZ, out int layerZ);
                if (!correctLayerName)
                {
                    throw new Exception("Erreur dans le nommage du layer " + sideSnowMap.Layers[i].Name +
                        ". Le nom doit commencer par deux chiffres indiquant la hauteur du layer.");
                }
                Debug.WriteLine(sideSnowMap.Layers[i].Name + " " + layerZ);

                //C'est ici qu'on initialise les tiles

                for (int y = 0; y < sideSnowMap.Layers[i].Tiles.Count; y++) // Pour chaque tile
                {
                    if (sideSnowMap.Layers[i].Tiles[y].Gid != 0) //si c'est pas du vide (Gid=0)
                    {
                        for (int ts = 0; ts < sideSnowMap.Tilesets.Count; ts++)
                        {
                            if ((sideSnowMap.Layers[i].Tiles[y].Gid >= sideSnowMap.Tilesets[ts].FirstGid)
                                && (sideSnowMap.Layers[i].Tiles[y].Gid < sideSnowMap.Tilesets[ts].FirstGid + sideSnowMap.Tilesets[ts].TileCount))
                            {
                                Point xAndYPosition = new Point(originTileCoord.X +orthogonalX*tileSize.X, originTileCoord.Y+orthogonalY*tileSize.Y);

                                mapElements.Add(CreateTile(ts, sideSnowMap.Layers[i].Tiles[y].Gid,
                                    xAndYPosition, (orthogonalX + orthogonalY + layerZ), sideSnowMap.Tilesets[ts].TileWidth, sideSnowMap.Tilesets[ts].TileHeight));
                            }
                            //else ce Gid ne fait pas partie de ce tileset
                        }

                    }

                    orthogonalX++;
                    if (orthogonalX >= sideSnowMap.Width) //en théorie le = devrait suffire
                    {
                        orthogonalX = 0;
                        orthogonalY++;
                    }
                }

            }
            sideSnowMap = null; //On a plus besoin de la TmxMap, tout est dans la liste
        }

        public ModelTile CreateTile(int tilesheetNumber, int gid, Point xAndYPosition, int zPosition,
            int width, int height)
        {
            Vector2 currentPosition = new Vector2(xAndYPosition.X, xAndYPosition.Y);//TODO choisir entre le point et vector2 pour pas avoir à faire ça
            Rectangle sourceRectangle = new Rectangle((gid - 1) % sideSnowMap.Tilesets[tilesheetNumber].Columns.Value * sideSnowMap.Tilesets[tilesheetNumber].TileWidth
                                    , (int)Math.Floor((double)((gid - sideSnowMap.Tilesets[tilesheetNumber].FirstGid) / sideSnowMap.Tilesets[tilesheetNumber].Columns.Value) * sideSnowMap.Tilesets[tilesheetNumber].TileHeight)
                                    , width, height);
            return new TileGround(currentPosition, sourceRectangle, width, height, zPosition, tilesheetNumber);
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ModelTile tile in mapElements)
            {
                spriteBatch.Draw(tilesetsTextures.Values.ElementAt(tile.TileSheetNb),
                    new Rectangle(tile.XPosition, tile.YPosition, tile.Width, tile.Height),
                    tile.SourceRectangle, Color.White, 0f,
                    new Vector2(0, tile.Height), SpriteEffects.None, tile.ZOrder == 0 ? 0 : 1 / tile.ZOrder); //dessin avec origine en bas à gauche //TODO est-ce que ça devrait pas être dans le Draw de ModelTile?


            }

        }


        public void Unload()
        {

        }


    }

}
