using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.CommonImagery
{
    public class AnimationJsonDTO
    {
        public FrameDTO[] Frames { get; set; }
    }
    public class FrameDTO
    {
        public int Duration { get; set; }
    }
}
