using Engine.CharacterClasses;
using Engine.CommonImagery;
using Engine.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using RythmProcessor;
using RythmProcessor.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Engine
{
    public class BattleScene : Scene
    {
        //Texture xxxBtn La texture à afficher
        //Vector2 xxxBtnOrigin La position
        //Rectangle xxxBtnZone La hitbox

        #region Texture2D
        Texture2D barreHorizontale;
        Texture2D barreTempsVerticale;
        Texture2D buttonUnclicked;
        Texture2D buttonClicked;
        Texture2D playButton;
        Texture2D pauseButton;
        Texture2D stopButton;
        Texture2D rythmClicked;
        Texture2D rythmUnclicked;
        Texture2D musicClicked;
        Texture2D musicUnclicked;
        Texture2D missed;

        AnimatedSprite greatAnimated;
        AnimatedSprite missedAnimated;
        #endregion

        Point inputButtonOrigin = new Point(100, 100);
        Point playPauseOrigin = new Point(200, 100); //coin bas gauche
        Point stopBtnOrigin = new Point(250, 100);

        Point posBarreTemps = new Point(20, 10);
        Rectangle playPauseZone;
        int hauteurBarreRythme;
        int hauteurBarreMusique;

        Rectangle inputButtonZone;
        Rectangle stopBtnZone;
        bool inputRythmPushed;
        bool inputMusicPushed;

        bool showRythmPushed;
        bool showMusicPushed;

        int tempsDAvance;
        int divisionDeTemps;

        bool showGreat;
        bool showMissed;


        bool playMusic; //false pause, true play
        SideViewMap snowMap;


        //TODO faire une liste de boutons

        bool rythmTempoMatch;
        bool musicTempoMatch;
        bool inputRythmMatchTempo;
        bool inputMusicMatchTempo;


        public MusicManager musicManager = MusicManager.Instance;

        Song payNoMind; //externaliser ce qui est musique

        SongDTO jsonTempoFile;
        Timer bpmTimer;

        Character player;


        public BattleScene(MainGame mG) : base(mG)
        {

        }
        public override void Load()
        {
            #region TextureLoading
            buttonUnclicked = mainGame.Content.Load<Texture2D>("buttonUnclicked");
            buttonClicked = mainGame.Content.Load<Texture2D>("buttonClicked");
            playButton = mainGame.Content.Load<Texture2D>("playButton");
            pauseButton = mainGame.Content.Load<Texture2D>("pauseButton");
            barreHorizontale = mainGame.Content.Load<Texture2D>("barreHoriz");
            rythmClicked = mainGame.Content.Load<Texture2D>("rythmClicked");
            rythmUnclicked = mainGame.Content.Load<Texture2D>("rythmUnclicked");
            barreTempsVerticale = mainGame.Content.Load<Texture2D>("barreTemps");
            stopButton = mainGame.Content.Load<Texture2D>("stopButton");
            musicUnclicked = mainGame.Content.Load<Texture2D>("musicUnclicked");
            musicClicked = mainGame.Content.Load<Texture2D>("musicClicked");

            greatAnimated = new AnimatedSprite(mainGame.Content.Load<Texture2D>("greatAnimated"), new Vector2(20, 80), 2, 7, Factory.GenerateDefaultFrameDurations(14));
            missedAnimated = new AnimatedSprite(mainGame.Content.Load<Texture2D>("missedAnimated"), new Vector2(20, 80), 2, 4,Factory.GenerateDefaultFrameDurations(8));
            #endregion

            payNoMind = mainGame.Content.Load<Song>("paynomind");

            hauteurBarreMusique = 20;
            hauteurBarreRythme = 40;                                      


            tempsDAvance = 4;


            musicManager.Load(mainGame, BattleSong.PAY_NO_MIND);

            mainGame.IsMouseVisible = true;


            playPauseZone = new Rectangle((int)playPauseOrigin.X, (int)playPauseOrigin.Y - playButton.Height, playButton.Width, playButton.Height);
            inputButtonZone = new Rectangle((int)inputButtonOrigin.X, (int)inputButtonOrigin.Y, buttonUnclicked.Width, buttonUnclicked.Height);
            stopBtnZone = new Rectangle(stopBtnOrigin.X, stopBtnOrigin.Y - stopButton.Height, stopButton.Width, stopButton.Height);

            snowMap = new SideViewMap(); // pas super
            snowMap.Load(mainGame.Content);

            zoom = 2;

            player = new Character();
            player.name = "ciale";
            player.sideRepresentation = new SideRepresentation();
            player.sideRepresentation.spriteIdle = new AnimatedSprite(mainGame.Content.Load<Texture2D>("Images/ciale/ciale_side_idle"),
                Vector2.Zero, //voir ce qu'on fout de cette position dans le constructeur pas focément utile
                3, 2, Factory.GenerateDefaultFrameDurations(6), Origin.MIDDLE_DOWN_ANCHORED);
            player.sideRepresentation.spriteRun = new AnimatedSprite(mainGame.Content.Load<Texture2D>("Images/ciale/ciale_side_run"),
                Vector2.Zero, //voir ce qu'on fout de cette position dans le constructeur pas focément utile      
                4, 3, Factory.GenerateDefaultFrameDurations(12), Origin.MIDDLE_DOWN_ANCHORED);
            player.sideRepresentation.Position = new Vector2(100, 100);
            player.sideRepresentation.ConstantHitboxSize = new Vector2(20, 68);
            ////Player : 

            //Player.Instance.Load(mainGame);
            //Factory.Instance.LoadPlayer();
            ////Player.Instance.currentCharacter.mapRepresentation.Load();


            base.Load();

        }
        public override void Unload()
        {
            base.Unload();
        }
        public override void Update(GameTime gameTime, float deltaTime)
        {
            //Debug.WriteLine("update");
            base.Update(gameTime, deltaTime); //la récupération des inputs se fait dans la méthode de la classe mère
            rythmTempoMatch = musicManager.CurrentSongDTO.RythmLine.Contains<int>(musicManager.ManagedTimer.CurrentBeat);
            inputRythmMatchTempo = false;
            showRythmPushed = false;
            showMusicPushed = false;

            if (playerInputs.Contains(InputType.SINGLE_LEFT_CLICK))
            {
                if (playPauseZone.Contains(cursorPosition))
                {
                    playMusic = !playMusic;
                    if (playMusic)
                    {
                        musicManager.Play();
                    }
                    else
                    {
                        musicManager.Pause();
                    }
                }
                else if (stopBtnZone.Contains(cursorPosition))
                {
                    musicManager.Stop();
                }
            }
            if (playerInputs.Contains(InputType.SINGLE_SPACE))
            {
                inputMusicPushed = true;
                if (musicTempoMatch)
                {
                    inputMusicMatchTempo = true;
                }
            }
            if (playerInputs.Contains(InputType.SINGLE_A)|| playerInputs.Contains(InputType.SINGLE_E))//TODO deuxième partie à enlever
            {
                inputRythmPushed = true;
                showRythmPushed = true;
                if (rythmTempoMatch)
                {
                    inputRythmMatchTempo = true;
                }

                if (playerInputs.Contains(InputType.SINGLE_A) && playerInputs.Contains(InputType.SINGLE_E))
                {
                    //défense
                }
                else if (playerInputs.Contains(InputType.SINGLE_A))
                {
                    //dash à gauche
                }
                else if (playerInputs.Contains(InputType.SINGLE_E))
                {
                    //dash à droite
                }
            }
            else if (playerInputs.Contains(InputType.A) || playerInputs.Contains(InputType.E))
            {
                showRythmPushed = true;
                inputRythmPushed = false;
            }
            else
            {
                inputRythmPushed = false;
                showRythmPushed = false;
            }
            //Player.Instance.currentCharacter.mapRepresentation.Update(playerInputs, deltaTime);

            foreach (Beat b in musicManager.ManagedTimer.Beats)
            {
                b.Update(musicManager.ManagedTimer.CurrentBeat, musicManager.ManagedTimer.BPM, musicManager.ManagedTimer.IntervalsByBPM, mainGame.deltaTime, playMusic, (mainGame.graphics.PreferredBackBufferWidth/zoom - (int)posBarreTemps.X) , tempsDAvance);
            }

            if (showMissed)
            {
                if (missedAnimated.FirstLoopDone == true)
                {
                    showMissed = false;
                }
                else
                {
                    missedAnimated.Update(deltaTime);
                }
            }
            if (showGreat)
            {
                if (greatAnimated.FirstLoopDone == true)
                {
                    showGreat = false;
                }
                else
                {
                    greatAnimated.Update(deltaTime);
                }
            }
            if (inputRythmPushed)
            {
                if (inputRythmMatchTempo)
                {
                    showGreat = true;
                    showMissed = false;
                }
                else
                {
                    showGreat = false;
                    showMissed = true;

                }
                greatAnimated.BackToFirstFrame();
                missedAnimated.BackToFirstFrame();
            }

            //Attention, le passage d'inputs au personnage du joueur doit se faire selon les résultats de l'algorithme précédent, pas avec une liste d'input classique

            List<ICollidable> levelActors = new List<ICollidable>(snowMap.tilesElements);
            player.Update(playerInputs, deltaTime, levelActors);
            
            

            //snowMap.Update();
        }

        protected void DrawSceneToTexture(RenderTarget2D renderTarget, GameTime gameTime)
        {
            // Set the render target
            mainGame.GraphicsDevice.SetRenderTarget(renderTarget);

            mainGame.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true }; //G PA KONPRI

            // Draw the scene
            //__________________CONTENU DU DRAW "CLASSIQUE"_____________

            mainGame.spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState :SamplerState.PointClamp); //SamplerState.PointClamp => Permet de resize du pixel art sans blur

            snowMap.Draw(mainGame.spriteBatch);

            player.sideRepresentation.Draw(mainGame.spriteBatch);

            DrawInterface();

            //Player.Instance.currentCharacter.mapRepresentation.Draw(mainGame.spriteBatch);

            base.Draw(gameTime);

            mainGame.spriteBatch.End();
            //__________________________________________________________

            // Drop the render target
            mainGame.GraphicsDevice.SetRenderTarget(null);
        }
        public void DrawInterface()
        {
            Tools.DrawTiled(mainGame.spriteBatch, barreHorizontale, new Vector2(0, hauteurBarreMusique), 30);
            Tools.DrawTiled(mainGame.spriteBatch, barreHorizontale, new Vector2(0, hauteurBarreRythme), 30);

            mainGame.spriteBatch.Draw(barreTempsVerticale, new Rectangle((int)posBarreTemps.X, (int)posBarreTemps.Y, barreTempsVerticale.Width, barreTempsVerticale.Height),
                null, Color.White, 0, new Vector2(barreTempsVerticale.Width / 2, 0), SpriteEffects.None, 1);//la barre fait 3px de large, comment ça fera divisé par 2? prend en compte le zoom?

            
            foreach (Beat b in musicManager.ManagedTimer.Beats)
            {
                b.Draw(mainGame.spriteBatch, inputMusicPushed ? musicClicked : musicUnclicked , showRythmPushed ? rythmClicked : rythmUnclicked, 
                    hauteurBarreMusique, hauteurBarreRythme, mainGame.graphics.PreferredBackBufferWidth, zoom); //TODO décalage sur Y selon que tempoMatch (2px plus bas) ou non
            }
            if (showGreat)
            {
                greatAnimated.Draw(mainGame.spriteBatch);
            }
            else if (showMissed)
            {
                missedAnimated.Draw(mainGame.spriteBatch);
            }


            mainGame.spriteBatch.Draw(showRythmPushed ? buttonClicked : buttonUnclicked, new Vector2(inputButtonOrigin.X, inputButtonOrigin.Y), Color.White);
            mainGame.spriteBatch.Draw(stopButton, new Rectangle(stopBtnOrigin.X, stopBtnOrigin.Y, stopButton.Width, stopButton.Height),
                null, Color.White, 0, new Vector2(0, stopButton.Height), SpriteEffects.None, 1);

            //celui là c'est pour savoir si il le tempo correspond ou pas:
            mainGame.spriteBatch.Draw(buttonUnclicked, new Vector2(100, 200), rythmTempoMatch ? Color.Green : Color.Red);

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

            mainGame.spriteBatch.DrawString(Fonts.Instance.kenPixel16, musicManager.ManagedTimer.CurrentBeat.ToString(), new Vector2(0, 50), Color.White);
        }


        public override void Draw(GameTime gameTime)
        {
            DrawSceneToTexture(renderTarget, gameTime);

            mainGame.GraphicsDevice.Clear(Color.Black);

            mainGame.spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null); //SamplerState.PointClamp => Permet de resize du pixel art sans blur

            mainGame.spriteBatch.Draw(renderTarget, new Rectangle(0, 0, 960, 540), Color.White); //TODO cette résolution et celle de maingame doivent être liées

            mainGame.spriteBatch.End();

        }
    }
}
