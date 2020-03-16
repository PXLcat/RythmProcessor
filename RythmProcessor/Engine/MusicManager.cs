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

namespace Engine
{
    public class MusicManager
    {

        private static MusicManager instance = null;

        public static MusicManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MusicManager();
                }
                return instance;
            }
        }

        private MusicManager()
        {

        }


        #region Champs
        private JsonSerializerSettings settings;
        private MainGame mainGame;

        #endregion

        #region Propriétés
        public List<String> SongNamesList { get; set; }
        public SongDTO CurrentSongDTO { get; set; }
        public Song CurrentSong { get; set; }
        public MusicTimer ManagedTimer { get; set; }
        
        #endregion

        public void Load(MainGame mainGame, BattleSong defaultSong)
        {
            this.mainGame = mainGame;

            settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore, //attention dino danger
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            SwitchMusic(defaultSong);
        }

        public void SwitchMusic(BattleSong song)
        {
            StreamReader sr = null; 
            switch (song)
            {
                case BattleSong.PAY_NO_MIND:
                    sr = new StreamReader("./Content/testpnm.json");
                    break;
                case BattleSong.LIONHEARTED:
                    break;
                default:
                    throw new Exception("Song not implemented");
            }
            String jsonFile = sr.ReadToEnd();
            CurrentSongDTO = JsonConvert.DeserializeObject<SongDTO>(jsonFile, settings);
            CurrentSong = mainGame.Content.Load<Song>(CurrentSongDTO.Name);

            List<Beat> beats = new List<Beat>();
            foreach (int i in CurrentSongDTO.MusicLine)
            {
                beats.Add(new Beat(BeatType.MUSIC, i));
            }
            foreach (int i in CurrentSongDTO.RythmLine)
            {
                beats.Add(new Beat(BeatType.RYTHM, i));
            }

            ManagedTimer = new MusicTimer(CurrentSongDTO.BPM, CurrentSongDTO.IntervalsByBPM, beats);



        }
        public void Play()
        {
            if (MediaPlayer.State == MediaState.Paused)
            {
                MediaPlayer.Resume();
                ManagedTimer.BPMTimer.Resume();
            }
            else
            {
                MediaPlayer.Play(CurrentSong);
                ManagedTimer.BPMTimer.Start();
            }

            
        }
        public void Pause()
        {
            MediaPlayer.Pause();
            ManagedTimer.BPMTimer.Pause(); //TODO attention c'est peut être ça qui décale : le BPMtimer et le timer qu'il contient se désynhronisent
        }
        public void Stop()
        {
            MediaPlayer.Stop();
            ManagedTimer.BPMTimer.Stop();
            foreach (Beat b in ManagedTimer.Beats)
            {
                b.Reset();
            }
            ManagedTimer.CurrentBeat = 0;
        }

        public void Unload()
        {
        }

        public void Update()
        {
        }

        public void Draw()
        {
        }

    }
    public enum BattleSong
    {
        PAY_NO_MIND,
        LIONHEARTED
    }
}
