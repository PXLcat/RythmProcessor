using RythmProcessor.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Engine
{
    public class MusicTimer
    {

        #region Champs

        #endregion

        #region Propriétés
        public int CurrentBeat { get; set; }
        public int BPM { get; set; }
        public int IntervalsByBPM { get; set; }
        public List<Beat> Beats { get; set; }
        public Timer BPMTimer { get; set; }
        #endregion

        //public void Load() L'initialisation se fera dans le ctor
        //{
        //    //Load du Json
        //    CurrentBeat = 0;
        //}

        public MusicTimer(int bpm, int intervalsByBPM, List<Beat> beats)
        {
            BPM = bpm;
            IntervalsByBPM = intervalsByBPM;
            Beats = beats;
            CurrentBeat = 0;

            BPMTimer = new Timer(60000 / bpm / intervalsByBPM);
            BPMTimer.Elapsed += OnTimedEvent;
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

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            CurrentBeat++;
        }
    }
}
