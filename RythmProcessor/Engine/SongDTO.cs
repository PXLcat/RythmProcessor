using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class SongDTO
    {
        public String Name { get; set; }
        public int BPM { get; set; }
        public int[] MusicLine { get; set; }
        public int[] RythmLine { get; set; }
        
    }
}
