using Engine;
using Engine.Scenes;
using Microsoft.Xna.Framework.Input;
using RythmProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Gamestate
    {
        public InputMethod currentInputMethod;

        public KeyboardState oldKbState;
        public MouseState oldMouseState;

        public enum SceneType
        {
            BATTLE,
            TESTCOLLISIONS
        }

        protected MainGame mainGame;
        public Scene CurrentScene { get; set; }

        public Gamestate(MainGame mG)
        {
            mainGame = mG;
        }

        public void ChangeScene(SceneType sT)
        {
            if (CurrentScene != null)
            {
                CurrentScene.Unload();
                CurrentScene = null;
            }

            switch (sT)
            {
                case SceneType.BATTLE:
                    CurrentScene = new BattleScene(mainGame);
                    break;
                case SceneType.TESTCOLLISIONS:
                    CurrentScene = new TestCollisionsScene(mainGame);
                    break;
            }

            CurrentScene.Load();
        }
    }
}
