using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RythmProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class BattleScene : Scene
    {
        Texture2D buttonUnclicked;
        Texture2D buttonClicked;

        public BattleScene(MainGame mG) : base(mG)
        {

        }
        public override void Load()
        {
            mainGame.IsMouseVisible = true;
            buttonUnclicked = mainGame.Content.Load<Texture2D>("buttonUnclicked");
            buttonClicked = mainGame.Content.Load<Texture2D>("buttonClicked");
            

            //snowMap = new IsometricMap(); // pas super
            //snowMap.Load(mainGame.Content);

            zoom = 2;


            //Player : 

            Player.Instance.Load(mainGame);
            Factory.Instance.LoadPlayer();
            //Player.Instance.currentCharacter.mapRepresentation.Load();

            base.Load();

        }
        public override void Unload()
        {
            base.Unload();
        }
        public override void Update(GameTime gameTime, float deltaTime)
        {
            base.Update(gameTime, deltaTime);
            List<InputType> playerInputs = Input.DefineInputs(ref mainGame.gameState.oldKbState);
            //Player.Instance.currentCharacter.mapRepresentation.Update(playerInputs, deltaTime);


            //snowMap.Update();
        }
        protected void DrawSceneToTexture(RenderTarget2D renderTarget, GameTime gameTime)
        {
            // Set the render target
            mainGame.GraphicsDevice.SetRenderTarget(renderTarget);

            mainGame.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true }; //G PA KONPRI

            // Draw the scene
            //__________________CONTENU DU DRAW "CLASSIQUE"_____________

            mainGame.spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null); //SamplerState.PointClamp => Permet de resize du pixel art sans blur

            Tools.DrawTiled(mainGame.spriteBatch, buttonUnclicked, 1, 1, new Vector2(100, 100));


            //snowMap.Draw(mainGame.spriteBatch);
            //Player.Instance.currentCharacter.mapRepresentation.Draw(mainGame.spriteBatch);

            base.Draw(gameTime);

            mainGame.spriteBatch.End();
            //__________________________________________________________

            // Drop the render target
            mainGame.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawSceneToTexture(renderTarget, gameTime);

            mainGame.GraphicsDevice.Clear(Color.Black);

            mainGame.spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null); //SamplerState.PointClamp => Permet de resize du pixel art sans blur

            mainGame.spriteBatch.Draw(renderTarget, new Rectangle(0, 0, 800, 600), Color.White);

            mainGame.spriteBatch.End();

        }
    }
}
