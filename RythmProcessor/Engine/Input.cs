using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Input 
    {
        /// <summary>
        /// Pour prendre en compte les input clavier et souris.
        /// </summary>
        /// <param name="oldMouseState"></param>
        /// <param name="oldKbState"></param>
        /// <returns></returns>
        public static List<InputType> DefineInputs(ref MouseState oldMouseState, ref KeyboardState oldKbState)
        {
            List<InputType> inputs = new List<InputType>();
            inputs.AddRange(DefineInputs(ref oldMouseState));
            inputs.AddRange(DefineInputs(ref oldKbState));

            return inputs;
        }

        /// <summary>
        /// Pour prendre en compte les input souris uniquement.
        /// </summary>
        /// <param name="oldMouseState"></param>
        /// <returns></returns>
        public static List<InputType> DefineInputs(ref MouseState oldMouseState)
        {
            List<InputType> inputs = new List<InputType>();
            MouseState newMouseState = Mouse.GetState();

            if ((newMouseState.LeftButton == ButtonState.Pressed) && newMouseState != oldMouseState)
            {
                inputs.Add(InputType.LEFT_CLICK);
                Debug.Write("input clic");
            }
            
            oldMouseState = newMouseState;
            return inputs;
        }

        /// <summary>
        /// Pour prendre en compte les input clavier uniquement.
        /// </summary>
        /// <param name="oldKbState"></param>
        /// <returns></returns>
        public static List<InputType> DefineInputs(ref KeyboardState oldKbState) //TODO devrait y en avoir 2, un pour les NPC, un pour joueur avec entrée clavier
        {
            List<InputType> inputs = new List<InputType>();
            KeyboardState newKbState = Keyboard.GetState();
            if (newKbState.IsKeyDown(Keys.Up)) //TODO mettre left et right sur un pied d'égalité
            {
                inputs.Add(InputType.UP);
                Debug.Write("input up");
            }
            if (newKbState.IsKeyDown(Keys.Down))
            {
                inputs.Add(InputType.DOWN);
                Debug.Write("input down");
            }
            if (newKbState.IsKeyDown(Keys.Left)) //TODO mettre left et right sur un pied d'égalité
            {
                inputs.Add(InputType.LEFT);
                Debug.Write("input left");
            }
            if (newKbState.IsKeyDown(Keys.Right))
            {
                inputs.Add(InputType.RIGHT);
                Debug.Write("input right");
            }
            if (newKbState.IsKeyDown(Keys.Up) && newKbState != oldKbState)
            { //mettre à part les conditions à rallonge?
                inputs.Add(InputType.SINGLE_UP);
                Debug.Write("input single up");
            }
            if (newKbState.IsKeyDown(Keys.Down) && newKbState != oldKbState)
            { //mettre à part les conditions à rallonge?
                inputs.Add(InputType.SINGLE_DOWN);
                Debug.Write("input single down");
            }
            if (newKbState.IsKeyDown(Keys.Left) && newKbState != oldKbState) //TODO mettre left et right sur un pied d'égalité
            {
                inputs.Add(InputType.SINGLE_LEFT);
                Debug.Write("input single left");
            }
            if (newKbState.IsKeyDown(Keys.Right) && newKbState != oldKbState)
            {
                inputs.Add(InputType.SINGLE_RIGHT);
                Debug.Write("input single right");
            }




            if (newKbState.IsKeyDown(Keys.Enter) && newKbState != oldKbState)
            {
                inputs.Add(InputType.SINGLE_ENTER);
                Debug.Write("input single enter");
            }
            if (newKbState.IsKeyDown(Keys.Back) && newKbState != oldKbState)
            {
                inputs.Add(InputType.RETURNTOMENU);
                Debug.Write("input returntomenu (backspace)");
            }


            //____________________



            oldKbState = newKbState;

            return inputs;

        }

        //public static List<InputType> DefineFileInputs() //inputs auto
        //{
        //    List<InputType> inputs = new List<InputType>();
        //    inputs.Add(InputType.MOVE_RIGHT);
        //    return inputs; //TODO
        //}
    }

    public enum InputType
    {
        //TODO raccrocher à des actions clavier pour Player, et comportements pour NPC?

        UP,
        DOWN,
        LEFT,
        RIGHT,
        SINGLE_UP,
        SINGLE_DOWN,
        SINGLE_LEFT,
        SINGLE_RIGHT,
        JUMP, // fall n'est pas un "input"
        ATTACK1,
        SINGLE_ENTER,
        RETURNTOMENU,
        DO_NOTHING,
        RESET_POSE,
        LEFT_CLICK
    }
    public enum InputMethod
    {
        KEYBOARD,
        MOUSE,
        MOUSE_AND_KEYBOARD_,
        FILE,
        NONE
    }
}