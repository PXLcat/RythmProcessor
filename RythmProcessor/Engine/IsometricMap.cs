using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Engine.Tiles;
using Engine;

namespace IsoMap.Engine
{
    public class IsometricMap
    {
        public TmxMap snowMap;
        Dictionary<string, Texture2D> tilesetsTextures;

        Point originTileCoord; //à utiliser pour les Tilesets 32x16
        Point originBlockCoord; //à utiliser pour les Tilesets 32x32
        int tileOrBlockWidth;
        Point tileSize;
        Point blockSize;

        List<ModelTile> mapElements;

        // TODO : faire une fonction pour déterminer si un tileset a des tiles "plates" ou "block" et utiliser pour savoir quoi charger dans le Laod

        public void Load(ContentManager contentManager)
        {
            snowMap = new TmxMap("Content/testiso.tmx");
            tilesetsTextures = new Dictionary<string, Texture2D>
            {
                { "grassTileset", contentManager.Load<Texture2D>(snowMap.Tilesets[0].Name) },//se référer à l'ordre dans le xml
                { "decorNeigeTileset", contentManager.Load<Texture2D>(snowMap.Tilesets[1].Name) }//TODO générer par Factory
            };

            tileOrBlockWidth = snowMap.Tilesets[0].TileWidth;
            tileSize = new Point(snowMap.Tilesets[0].TileWidth, snowMap.Tilesets[0].TileHeight);
            blockSize = new Point(snowMap.Tilesets[1].TileWidth, snowMap.Tilesets[1].TileHeight);

            //originTileCoord = new Point(snowMap.Tilesets[0].TileWidth * (snowMap.Width / 4) - snowMap.Tilesets[0].TileWidth / 2, 0);
            originTileCoord = new Point(160, 0); //à suppr
            originBlockCoord = new Point(snowMap.Tilesets[0].TileWidth * (snowMap.Width / 2) - snowMap.Tilesets[0].TileWidth / 2,
                -snowMap.Tilesets[0].TileHeight); //attention tout se base sur les dimensions du premier tileset

            FillMapElements();

        }
        public void FillMapElements()
        {
            mapElements = new List<ModelTile>();

            for (int i = 0; i < snowMap.Layers.Count; i++)// Pour chaque layer
            {
                int orthogonalX = 0;
                int orthogonalY = 0;

                string layerNameZ = snowMap.Layers[i].Name.Substring(0, 2);

                ///A "true" si le nom du layer commence bien par deux chifres. layerZ représente la hauteur en blocs du layer
                bool correctLayerName = Int32.TryParse(layerNameZ, out int layerZ);
                if (!correctLayerName)
                {
                    throw new Exception("Erreur dans le nommage du layer " + snowMap.Layers[i].Name +
                        ". Le nom doit commencer par deux chiffres indiquant la hauteur du layer.");
                }
                Debug.WriteLine(snowMap.Layers[i].Name + " " + layerZ);

                //C'est ici qu'on initialise les tiles

                for (int y = 0; y < snowMap.Layers[i].Tiles.Count; y++) // Pour chaque tile
                {
                    if (snowMap.Layers[i].Tiles[y].Gid != 0) //si c'est pas du vide (Gid=0)
                    {
                        for (int ts = 0; ts < snowMap.Tilesets.Count; ts++)
                        {
                            if ((snowMap.Layers[i].Tiles[y].Gid >= snowMap.Tilesets[ts].FirstGid)
                                && (snowMap.Layers[i].Tiles[y].Gid < snowMap.Tilesets[ts].FirstGid + snowMap.Tilesets[ts].TileCount))
                            {
                                Point xAndYPosition = Tools.CarthesianToIsometricTile(snowMap, new Point(orthogonalX, orthogonalY)
                                        , originTileCoord);

                                mapElements.Add(CreateTile(ts, snowMap.Layers[i].Tiles[y].Gid,
                                    xAndYPosition, layerZ, snowMap.Tilesets[ts].TileWidth, snowMap.Tilesets[ts].TileHeight));
                            }
                            //else ce Gid ne fait pas partie de ce tileset
                        }

                    }

                    orthogonalX++;
                    if (orthogonalX >= snowMap.Width) //en théorie le = devrait suffire
                    {
                        orthogonalX = 0;
                        orthogonalY++;
                    }
                }

            }
        }

        public ModelTile CreateTile(int tilesheetNumber, int gid, Point xAndYPosition, int zPosition,
            int width, int height)
        {
            Vector2 currentPosition = new Vector2(xAndYPosition.X, xAndYPosition.Y);//TODO choisir entre le point et vector2 pour pas avoir à faire ça
            Rectangle sourceRectangle = new Rectangle((gid - 1) % snowMap.Tilesets[tilesheetNumber].Columns.Value * snowMap.Tilesets[tilesheetNumber].TileWidth
                                    , (int)Math.Floor((double)((gid - snowMap.Tilesets[tilesheetNumber].FirstGid) / snowMap.Tilesets[tilesheetNumber].Columns.Value) * snowMap.Tilesets[tilesheetNumber].TileHeight)
                                    , width,height);
            return new TileGround(currentPosition, sourceRectangle, width, height, zPosition, tilesheetNumber); 
        }

        public void Update()
        {

        }

        public enum TileStyle
        {
            FLAT,
            BLOCK
        }
        public TileStyle GetTileStyle(TmxTileset tileset)
        {
            TileStyle result;
            if (tileset.TileHeight == tileset.TileWidth / 2)
            {
                result = TileStyle.FLAT;
            }
            else
            {
                result = TileStyle.BLOCK;
            }
            return result;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ModelTile tile in mapElements)
            {
                spriteBatch.Draw(tilesetsTextures.Values.ElementAt(tile.TileSheetNb),
                    new Rectangle(tile.XPosition, tile.YPosition, tile.Width, tile.Height),
                    tile.SourceRectangle, Color.White, 0f,
                    new Vector2(0, tile.Height), SpriteEffects.None, 1f); //dessin avec origine en bas à gauche
            }

        }


        public void Unload()
        {

        }


    }
}