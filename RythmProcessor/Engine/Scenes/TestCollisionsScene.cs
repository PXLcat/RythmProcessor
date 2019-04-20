using Engine.CommonImagery;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RythmProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Scenes
{
    public class TestCollisionsScene : Scene
    {

        #region Texture2D

        Texture2D idle;
        Texture2D jump;
        Texture2D fall;
        Texture2D textureTonneau;

        TestCharacter testProp;
        BackgroundItem tonneau;

        #endregion



        public TestCollisionsScene(MainGame mG) : base(mG)
        {

        }

        public override void Load()
        {
            #region TextureLoading
            idle = mainGame.Content.Load<Texture2D>("testsCollisions/idle");
            jump = mainGame.Content.Load<Texture2D>("testsCollisions/jump");
            fall = mainGame.Content.Load<Texture2D>("testsCollisions/fall");

            textureTonneau = mainGame.Content.Load<Texture2D>("testsCollisions/tonneau");
            #endregion

            testProp = new TestCharacter(idle, jump, fall, new Vector2(100,100));
            tonneau = new BackgroundItem(textureTonneau, new Vector2(150,150));
            

            mainGame.IsMouseVisible = true;

            zoom = 2; //C'est ici qu'on définit réellement le zoom

            base.Load();

        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Update(GameTime gameTime, float deltaTime)
        {
            base.Update(gameTime, deltaTime);
            testProp.Update(playerInputs);

            TestCollisions(testProp, tonneau);

        }

        private void TestCollisions(TestCharacter testProp, BackgroundItem tonneau)
        {
            if (true)
            {

            }
        }

        protected void DrawSceneToTexture(RenderTarget2D renderTarget, GameTime gameTime)
        {
            // Set the render target
            mainGame.GraphicsDevice.SetRenderTarget(renderTarget);

            mainGame.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true }; //G PA KONPRI

            // Draw the scene
            //__________________CONTENU DU DRAW "CLASSIQUE"_____________

            mainGame.spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null); //SamplerState.PointClamp => Permet de resize du pixel art sans blur
            testProp.Draw(mainGame.spriteBatch);
            tonneau.Draw(mainGame.spriteBatch);


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
            mainGame.spriteBatch.Draw(renderTarget, new Rectangle(0, 0, 960, 540), Color.White);
            mainGame.spriteBatch.End();

        }
    }
    public class TestCharacter : ICollidable
    {

        #region Champs

        #endregion

        #region Propriétés
        public Texture2D Idle { get; set; }
        public Texture2D Fall { get; set; }
        public Texture2D Jump { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D CurrentTexture { get; set; }

        public Rectangle HitBox {
            get {return new Rectangle((int)Position.X, (int)Position.Y, CurrentTexture.Width, CurrentTexture.Height);
            }
            set { HitBox = value; }
        }


        #endregion

        public TestCharacter(Texture2D idle, Texture2D fall, Texture2D jump, Vector2 position)
        {
            Idle = idle;
            Fall = fall;
            Jump = jump;
            Position = position;

            CurrentTexture = Idle;
        }

        public void Load()
        {
        }

        public void Unload()
        {
        }

        public void Update(List<InputType> inputs)
        {
            if (inputs.Contains(InputType.UP))
            {
                Position = new Vector2(Position.X, Position.Y - 1);
            }
            if (inputs.Contains(InputType.DOWN))
            {
                Position = new Vector2(Position.X, Position.Y + 1);
            }
            if (inputs.Contains(InputType.LEFT))
            {
                Position = new Vector2(Position.X-1, Position.Y);
            }
            if (inputs.Contains(InputType.RIGHT))
            {
                Position = new Vector2(Position.X + 1, Position.Y);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(CurrentTexture, Position, Color.White);
        }

        public void OnCollision(ICollidable other)
        {
            throw new NotImplementedException();
        }
    }
    public class BackgroundItem : ICollidable
    {

        #region Champs

        #endregion

        #region Propriétés
        public Texture2D Image { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle HitBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);
            }
            set { HitBox = value; }
        }
        #endregion
        public BackgroundItem(Texture2D texture, Vector2 position)
        {
            Image = texture;
            Position = position;
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

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Image, Position, Color.White);
        }

        public void OnCollision(ICollidable other)
        {
            throw new NotImplementedException();
        }


    }
}
