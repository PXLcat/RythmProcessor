using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.CommonImagery;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.CharacterClasses
{
    public class MapRepresentation : CommonImagery.IDrawable, ICollidable
    {
        public AnimatedSprite idle_front, idle_back, run;
        public AnimatedSprite currentSprite;

        public Rectangle HitBox => throw new NotImplementedException();

        public Vector2 CurrentPosition { get => currentPosition; set => currentPosition = value; }
        public Texture2D Texture { get => texture; set => texture = value; }

        private Vector2 currentPosition;
        private Texture2D texture;

        public Vector2 Movement { get; set; }
        public int Zorder { get; set; }

        private float deltaTime;

        public bool HorizontalFlip { get; set; }

        public void OnCollision(ICollidable other)
        {
            throw new NotImplementedException();
        }
        public MapRepresentation()
        {

        }
        public void Load()
        {
            currentSprite = idle_front;
        }

        public void Draw(SpriteBatch sb)
        {
            currentSprite.Draw(sb, HorizontalFlip);
            
        }

        public void Update(List<InputType> playerInputs, float deltaTime) {
            Movement = Vector2.Zero;
            this.deltaTime = deltaTime; //qu'est ce qui est le mieux entre stocker le dT ou le passer de méthode en méthode? 
            if (playerInputs.Count > 0)
            {
                SortAndExecuteInput(playerInputs);
            }
            CurrentPosition += Movement;
            currentSprite.CurrentPosition = CurrentPosition;
            currentSprite.Update(deltaTime);
        }

        private void SortAndExecuteInput(List<InputType> inputs)
        {
            if (inputs.Contains(InputType.LEFT) && inputs.Contains(InputType.RIGHT))
            {
                //ResetPose();
            }
            else if (inputs.Contains(InputType.LEFT) && !inputs.Contains(InputType.RIGHT))
            {
                MoveLeft();
            }
            else if (inputs.Contains(InputType.RIGHT) && !inputs.Contains(InputType.LEFT))
            {
                MoveRight();
            }
            if (inputs.Contains(InputType.UP))
            {
                MoveUp();
            }
            else if (inputs.Contains(InputType.DOWN))
            {
                MoveDown();
            }

        }

        private void MoveDown()
        {
            Movement += new Vector2(-4 * deltaTime, 2 * deltaTime); //remplacer par vitesse de déplacement
            currentSprite = idle_front;
            HorizontalFlip = false;
        }

        private void MoveUp()
        {
            Movement += new Vector2(4 * deltaTime, -2 * deltaTime);
            currentSprite = idle_back;
            HorizontalFlip = false;
        }

        private void MoveRight()
        {
            Movement += new Vector2(4 * deltaTime, 2 * deltaTime);
            currentSprite = idle_front;
            HorizontalFlip = true;
        }

        private void MoveLeft()
        {
            Movement += new Vector2(-4 * deltaTime, -2 * deltaTime);
            currentSprite = idle_back;
            HorizontalFlip = true;
        }
    }
}
