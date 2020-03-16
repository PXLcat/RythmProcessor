using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Engine
{
    public class CustomTimer
    {
        public double Interval { get; set; }
        public bool Enabled { get; set; }
        public int CurrentTick { get; set; }

        private Stopwatch watch;

        public event EventHandler<TickEventArgs> Tick;

        public CustomTimer(double interval)
        {
            Interval = interval;
            CurrentTick = 0;
        }

        public void Start()
        {
            watch = Stopwatch.StartNew();
            Thread myThread = new Thread(new ThreadStart(CountTime));
            myThread.Start();
            //TODO myThread.Suspend
        }

        private void CountTime()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                if (watch.ElapsedMilliseconds>Interval + CurrentTick*Interval)
                {
                    CurrentTick++;
                    Debug.WriteLine("tick");
                    Tick(this, new TickEventArgs(CurrentTick));
                }
            }
        }

        public void Stop()
        {
            watch.Reset();
        }




        public class TickEventArgs : EventArgs
        {
            public TimeSpan Duration { get; private set; }
            public long TotalTicks { get; private set; }

            public TickEventArgs(int totalTicks)
            {
                //this.Duration = totalDuration;
                this.TotalTicks = totalTicks;
            }
        }
    }
}