﻿using Engine.CommonImagery;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using RythmProcessor;
using RythmProcessor.Engine;
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
        bool inputButtonClicked;

        int currentBeat;
        int tempsDAvance;
        int divisionDeTemps;


        bool playMusic; //false pause, true play
        SideViewMap snowMap;


        //TODO faire une liste de boutons





        bool tempoMatch;

        Song payNoMind; //externaliser ce qui est musique

        SongDTO jsonTempoFile;
        Timer bpmTimer;

        List<Beat> beats;

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
            #endregion

            payNoMind = mainGame.Content.Load<Song>("paynomind");

            hauteurBarreMusique = 20;
            hauteurBarreRythme = 40;
            currentBeat = 0;

            tempsDAvance = 4;

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore, //attention dino danger
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            StreamReader sr = new StreamReader("./Content/testpnm.json");//TODO externaliser
            String jsonFile = sr.ReadToEnd();
            jsonTempoFile = JsonConvert.DeserializeObject<SongDTO>(jsonFile, settings);
            divisionDeTemps = 4;//TODO à mettre dans le json! CF EDITEURRYTHME timerBPM.Interval = secFromBpm / 4;


            mainGame.IsMouseVisible = true;





            playPauseZone = new Rectangle((int)playPauseOrigin.X, (int)playPauseOrigin.Y - playButton.Height, playButton.Width, playButton.Height);
            inputButtonZone = new Rectangle((int)inputButtonOrigin.X, (int)inputButtonOrigin.Y, buttonUnclicked.Width, buttonUnclicked.Height);
            stopBtnZone = new Rectangle(stopBtnOrigin.X, stopBtnOrigin.Y - stopButton.Height, stopButton.Width, stopButton.Height);

            snowMap = new SideViewMap(); // pas super
            snowMap.Load(mainGame.Content);

            zoom = 2;

            beats = new List<Beat>();
            foreach (int i in jsonTempoFile.MusicLine)
            {
                beats.Add(new Beat(BeatType.MUSIC, i));
            }
            foreach (int i in jsonTempoFile.RythmLine)
            {
                beats.Add(new Beat(BeatType.RYTHM, i));
            }


            //Player : 

            Player.Instance.Load(mainGame);
            Factory.Instance.LoadPlayer();
            //Player.Instance.currentCharacter.mapRepresentation.Load();

            bpmTimer = new Timer(60000 / jsonTempoFile.BPM / divisionDeTemps);
            bpmTimer.Elapsed += OnTimedEvent;
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
                        PauseMusic();
                    }
                }
                else if (stopBtnZone.Contains(cursorPosition))
                {
                    StopMusic();
                }
            }
            if ((playerInputs.Contains(InputType.SINGLE_LEFT_CLICK) || playerInputs.Contains(InputType.LEFT_CLICK))
                && inputButtonZone.Contains(cursorPosition))
            {
                inputButtonClicked = true;
            }
            else
            {
                inputButtonClicked = false;
            }
            //Player.Instance.currentCharacter.mapRepresentation.Update(playerInputs, deltaTime);

            tempoMatch = jsonTempoFile.RythmLine.Contains<int>(currentBeat);

            foreach (Beat b in beats)
            {
                b.Update(currentBeat, jsonTempoFile.BPM, divisionDeTemps, mainGame.deltaTime, playMusic, (mainGame.graphics.PreferredBackBufferWidth - (int)posBarreTemps.X) / 2, tempsDAvance);
            }

            //snowMap.Update();
        }

        private void StartMusic()
        {
            if (MediaPlayer.State == MediaState.Paused)
            {
                MediaPlayer.Resume();
            }
            else
            {
                MediaPlayer.Play(payNoMind);
            }

            bpmTimer.AutoReset = true;
            bpmTimer.Start();

        }
        private void PauseMusic()
        {
            MediaPlayer.Pause();
            bpmTimer.Stop();
        }

        private void StopMusic()
        {
            MediaPlayer.Stop();
            bpmTimer.Stop();
            foreach (Beat b in beats)
            {
                b.Reset();
            }
            currentBeat = 0;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            currentBeat++;
        }

        protected void DrawSceneToTexture(RenderTarget2D renderTarget, GameTime gameTime)
        {
            // Set the render target
            mainGame.GraphicsDevice.SetRenderTarget(renderTarget);

            mainGame.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true }; //G PA KONPRI

            // Draw the scene
            //__________________CONTENU DU DRAW "CLASSIQUE"_____________

            mainGame.spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null); //SamplerState.PointClamp => Permet de resize du pixel art sans blur

            snowMap.Draw(mainGame.spriteBatch);
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

            
            foreach (Beat b in beats)
            {
                b.Draw(mainGame.spriteBatch, inputButtonClicked ? musicClicked : musicUnclicked , inputButtonClicked ? rythmClicked : rythmUnclicked, 
                    hauteurBarreMusique, hauteurBarreRythme, mainGame.graphics.PreferredBackBufferWidth, zoom); //TODO décalage sur Y selon que tempoMatch (2px plus bas) ou non
            }


            mainGame.spriteBatch.Draw(inputButtonClicked ? buttonClicked : buttonUnclicked, new Vector2(inputButtonOrigin.X, inputButtonOrigin.Y), Color.White);
            mainGame.spriteBatch.Draw(stopButton, new Rectangle(stopBtnOrigin.X, stopBtnOrigin.Y, stopButton.Width, stopButton.Height),
                null, Color.White, 0, new Vector2(0, stopButton.Height), SpriteEffects.None, 1);

            //celui là c'est pour savoir si il le tempo correspond ou pas:
            mainGame.spriteBatch.Draw(buttonUnclicked, new Vector2(100, 200), tempoMatch ? Color.Green : Color.Red);

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

            mainGame.spriteBatch.DrawString(Fonts.Instance.kenPixel16, currentBeat.ToString(), new Vector2(0, 50), Color.White);
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
