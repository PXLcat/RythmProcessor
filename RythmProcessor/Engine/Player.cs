using Engine.CharacterClasses;
using Microsoft.Xna.Framework;
using RythmProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player
    {
        private static Player instance;
        public List<Character> charactersList;
        public Character currentCharacter;

        public static Player Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Player();

                }
                return instance;
            }
        }
        private Player() { }

        public void Load(MainGame mG)
        {
            Factory factory = Factory.Instance;
            //charactersList = factory.GetCharacters();
            
            //factory.LoadPlayer();
            this.currentCharacter = charactersList[0];
            currentCharacter.mapRepresentation.CurrentPosition = new Vector2(200, 200);

        }
        public void Update(List<InputType> inputs,float deltatime, List<ICollidable> levelActors)
        {
            currentCharacter.Update(inputs, deltatime, levelActors);
        }


    }

}
