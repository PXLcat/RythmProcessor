using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.CharacterClasses
{
    public class Character
    {
        public String name;
        public int currentHP;
        public int maxHP;
        public Status characterStatus;
        public Texture2D avatar; //shit faut que le cadre soit à part
        public MenuRepresentation menuRepresentation;
        public SideRepresentation sideRepresentation;
        public MapRepresentation mapRepresentation;
        


        public Character()
        {

        }

        public Character(string name, int maxHP) //utile à terme si c'est la Factory qui crée?
        {
            this.name = name;
            this.maxHP = maxHP;
            currentHP = maxHP;
            this.characterStatus = Status.NONE;
        }
        
        public enum Status
        {
            NONE,
            POISONED,
            PARALYSED,
            KO
        }

        public void Update(List<InputType> inputs,float deltatime, List<ICollidable> levelActors) {
            //mapRepresentation.CurrentPosition = 
            Debug.Write("test");
            //TODO ajouter un champ currentRepresentation au Gamestate pour savoir quelle représentation updater
            sideRepresentation.Update(inputs, deltatime, levelActors);
        }
    }
}
