using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RythmProcessor
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        public GraphicsDeviceManager graphics; //safe de mettre en public?
        public SpriteBatch spriteBatch;
        public Gamestate gameState;
        public float deltaTime;

        public MainGame()
        {           
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            gameState = new Gamestate(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 960;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 540;   // set this value to the desired height of your window
            graphics.ApplyChanges();

            gameState.ChangeScene(Gamestate.SceneType.BATTLE);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 10; //histoire d'avoir genre 1,6
            //Debug.WriteLine("Delta time :"+ deltaTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (gameState.CurrentScene != null)
            {
                gameState.CurrentScene.Update(gameTime, deltaTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            if (gameState.CurrentScene != null)
            {
                gameState.CurrentScene.Draw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}
