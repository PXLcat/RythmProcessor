using Engine.CommonImagery;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using RythmProcessor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Engine
{
    public class BattleScene : Scene
    {
        Texture2D buttonUnclicked;
        Texture2D buttonClicked;
        Vector2 inputButtonOrigin = new Vector2(100, 100);
        Rectangle inputButtonZone;
        bool inputButtonClicked;

        Texture2D playButton;
        Texture2D pauseButton;
        bool playMusic; //false pause, true play

        Texture2D barreHorizontale;

        Vector2 playPauseOrigin = new Vector2(200, 100); //coin bas gauche
        Rectangle playPauseZone;

        bool tempoMatch;

        Song testMusic; //externaliser ce qui est musique

        SongDTO jsonTempoFile;
        Timer bpmTimer;

        int songTimer;

        public BattleScene(MainGame mG) : base(mG)
        {

        }
        public override void Load()
        {
            songTimer = 0;

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore, //attention dino danger
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            StreamReader sr = new StreamReader("./Content/testpnm.json");//TODO externaliser
            String jsonFile = sr.ReadToEnd();
            jsonTempoFile = JsonConvert.DeserializeObject<SongDTO>(jsonFile, settings);


            mainGame.IsMouseVisible = true;
            buttonUnclicked = mainGame.Content.Load<Texture2D>("buttonUnclicked");
            buttonClicked = mainGame.Content.Load<Texture2D>("buttonClicked");
            playButton = mainGame.Content.Load<Texture2D>("playButton");
            pauseButton = mainGame.Content.Load<Texture2D>("pauseButton");
            barreHorizontale = mainGame.Content.Load<Texture2D>("barreHoriz");

            testMusic = mainGame.Content.Load<Song>("paynomind");
            

            playPauseZone = new Rectangle((int)playPauseOrigin.X, (int)playPauseOrigin.Y - playButton.Height, playButton.Width, playButton.Height);
            inputButtonZone = new Rectangle((int)inputButtonOrigin.X, (int)inputButtonOrigin.Y, buttonUnclicked.Width, buttonUnclicked.Height);

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
            base.Update(gameTime, deltaTime); //la récupération des inputs se fait dans la méthode de la classe mère

            if (playerInputs.Contains(InputType.SINGLE_LEFT_CLICK))
            {
                if (playPauseZone.Contains(cursorPosition))
                {
                    playMusic = !playMusic;
                    if (playMusic)
                    {
                        StartMusic();
                    }
                    else
                    {
                        StopMusic();
                    }
                }
            }
            if ((playerInputs.Contains(InputType.SINGLE_LEFT_CLICK)|| playerInputs.Contains(InputType.LEFT_CLICK))
                && inputButtonZone.Contains(cursorPosition))
            {
                inputButtonClicked = true;
            }
            else
            {
                inputButtonClicked = false;
            }
            //Player.Instance.currentCharacter.mapRepresentation.Update(playerInputs, deltaTime);

            tempoMatch = jsonTempoFile.RythmLine.Contains<int>(songTimer);

                //snowMap.Update();
            }

        private void StartMusic()
        {
            MediaPlayer.Play(testMusic);
            int divisionDeTemps = 4;//TODO à mettre dans le json! CF EDITEURRYTHME timerBPM.Interval = secFromBpm / 4;
            bpmTimer = new Timer(60000 / jsonTempoFile.BPM / divisionDeTemps); //à mettre peut-être en dehors du Start
            //TODO vérifier que le Timer est indépendant de l'Update
            bpmTimer.Elapsed += OnTimedEvent;
            bpmTimer.AutoReset = true;
            bpmTimer.Start();

        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            songTimer++;
        }

        private void StopMusic()
        {
            MediaPlayer.Stop();
        }

        protected void DrawSceneToTexture(RenderTarget2D renderTarget, GameTime gameTime)
        {
            // Set the render target
            mainGame.GraphicsDevice.SetRenderTarget(renderTarget);

            mainGame.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true }; //G PA KONPRI

            // Draw the scene
            //__________________CONTENU DU DRAW "CLASSIQUE"_____________

            mainGame.spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null); //SamplerState.PointClamp => Permet de resize du pixel art sans blur

            Tools.DrawTiled(mainGame.spriteBatch, barreHorizontale, new Vector2(0,20),30) ;

            mainGame.spriteBatch.Draw(inputButtonClicked?buttonClicked: buttonUnclicked, inputButtonOrigin, Color.White);

            //celui là c'est pour savoir si il le tempo correspond ou pas:
            mainGame.spriteBatch.Draw(buttonUnclicked, new Vector2(100,200), tempoMatch ? Color.Green : Color.Red);

            Texture2D toDrawButton = null;
            if (playMusic)
            {
                toDrawButton = pauseButton;
            }
            else
            {
                toDrawButton = playButton;
            }
            mainGame.spriteBatch.Draw(toDrawButton, new Rectangle((int)playPauseOrigin.X, (int)playPauseOrigin.Y, toDrawButton.Width, toDrawButton.Height),
                    null, Color.White, 0, new Vector2(0, toDrawButton.Height), SpriteEffects.None, 1);

            mainGame.spriteBatch.DrawString(Fonts.Instance.kenPixel16, songTimer.ToString(), new Vector2(0, 50), Color.White);


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
