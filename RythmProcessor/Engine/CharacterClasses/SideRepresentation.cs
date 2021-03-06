﻿using Engine.CommonImagery;
using Engine.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Engine
{
    public class SideRepresentation : ICollidable
    {
        public AnimatedSprite spriteIdle, spriteRun, spriteJump, spriteFall, spriteAttack1;
        private State characterState;
        public State CharacterState
        {
            get { return characterState; }
            set { characterState = value;
                CurrentSprite.CurrentPosition = Position;
            }
        }

        public AnimatedSprite CurrentSprite
        {
            get
            {
                switch (CharacterState)
                {
                    case State.IDLE:
                        return spriteIdle;
                    case State.RUNNING:
                        return spriteRun;
                    case State.JUMPING:
                        return spriteJump;
                    case State.FALLING:
                        return spriteFall;
                    case State.ATTACKING1: // TODO overwrite pour player pour avoir plus d'attaques
                        return spriteAttack1;
                    default:
                        throw new System.Exception();
                }
            }
        }

        public Vector2 ConstantHitboxSize { get; set; }
        public Vector2 Position { get; set; }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;
        public float XMovement { get; set; }

        public Rectangle HitBox
        {
            get
            {
                return new Rectangle((int)(Position.X - ConstantHitboxSize.X / 2), (int)(Position.Y - ConstantHitboxSize.Y),
            (int)ConstantHitboxSize.X, (int)ConstantHitboxSize.Y);
            }
        }

        public bool isOnGround { get; set; }
        public bool wasJumping { get; set; }

        public int MaxJumps { get; set; }
        private int jumpsDone;
        public float JumpHeight { get; set; }

        public bool HorizontalFlip {
            get
            {
                if (CharacterFaces == Facing.LEFT)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            }
        public bool Crossable { get; set; } 
        public Facing CharacterFaces { get; private set; }

        // Constants for controlling vertical movement
        private const float GravityAcceleration = 0.5f;
        private const float MaxFallJumpSpeed = 4f;

        public float MaxMoveSpeed = 4f;

        Texture2D hitboxTexture;

        public SideRepresentation()
        {

        }

        void ICollidable.OnCollision(ICollidable other)
        {
            throw new System.NotImplementedException();
        }


        public void Update(List<InputType> playerInputs, float deltaTime, List<ICollidable> levelActors)
        {


            CurrentSprite.Update(deltaTime);//avant ou après ApplyPhysics?
            //Movement = Vector2.Zero;
            //this.deltaTime = deltaTime; //qu'est ce qui est le mieux entre stocker le dT ou le passer de méthode en méthode? 
            //if (playerInputs.Count > 0)
            //{
            //    SortAndExecuteInput(playerInputs);
            //}
            //CurrentPosition += Movement;
            //currentSprite.CurrentPosition = CurrentPosition;
            //currentSprite.Update(deltaTime);
            SortAndExecuteInput(playerInputs, deltaTime);

            ApplyPhysics(playerInputs, deltaTime, levelActors);

            CurrentSprite.CurrentPosition = Position;

            XMovement = 0;


        }

        private void ApplyPhysics(List<InputType> playerInputs, float deltaTime, List<ICollidable> levelActors)
        {
            Vector2 previousPosition = Position; //dans le update plutôt?

            //Mouvement axe X :
            velocity.X = XMovement * deltaTime;
            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            //la gravité:
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * deltaTime, -MaxFallJumpSpeed, MaxFallJumpSpeed);

            //Gestion du saut :
            velocity.Y = DoJump(playerInputs, velocity.Y, deltaTime);

            // Apply velocity.
            Position += velocity * deltaTime; //TODO pourquoi on applique le dt là alors que ça a déjà été appliqué à la vélocité
            //Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            HandleCollisions(levelActors);

        }

        private float DoJump(List<InputType> playerInputs, float velocityY, float deltaTime)
        {
            if (playerInputs.Contains(InputType.SINGLE_UP))
            {
                velocityY -= 18;
            }
            else
            {

            }
            return velocityY;
        }

        private void HandleCollisions(List<ICollidable> levelActors)
        {
            isOnGround = false;
            int nombreDObstaclesCroises = 0;

            foreach (ICollidable actor in levelActors)
            {
                if (!actor.Crossable && actor.HitBox.Intersects(HitBox) && isOnGround == false)
                {
                    CollideType collision = Utilities.CheckCollision(this, actor); //attention, avec ce système on ignore tous les obstacles de moins de 8px

                    if (collision.collideBottom && !(collision.collideRight || collision.collideLeft))
                    {
                        this.Position = new Vector2(Position.X, actor.HitBox.Top);
                        isOnGround = true;
                    }
                    if (collision.collideRight)
                    {
                        this.Position = new Vector2(actor.HitBox.Left-(this.HitBox.Width/2), Position.Y);
                    }
                    if (collision.collideLeft)
                    {
                        this.Position = new Vector2(actor.HitBox.Right + (this.HitBox.Width / 2), Position.Y);
                    }

                    nombreDObstaclesCroises++;
                }
            }
            //Debug.Write(nombreDObstaclesCroises);
        }

        private void SortAndExecuteInput(List<InputType> playerInputs, float deltaTime)
        {
            if (playerInputs.Contains(InputType.LEFT) && playerInputs.Contains(InputType.RIGHT))
            {
                //ResetPose();
            }
            else if (playerInputs.Contains(InputType.LEFT) && !playerInputs.Contains(InputType.RIGHT))
            {
                MoveLeft();
            }
            else if (playerInputs.Contains(InputType.RIGHT) && !playerInputs.Contains(InputType.LEFT))
            {
                MoveRight();
            }
            //if (inputs.Contains(InputType.UP))
            //{
            //    MoveUp();
            //}
            //else if (inputs.Contains(InputType.DOWN))
            //{
            //    MoveDown();
            //}
            if (playerInputs.Count==0 && (characterState ==State.RUNNING))
            {
                CharacterState = State.IDLE;
            }

        }

        private void MoveDown()
        {

        }

        private void MoveUp()
        {

        }

        private void MoveRight()
        {
            if (CharacterState != State.ATTACKING1)
            {
                if (CharacterState == State.IDLE)
                {
                    CharacterState = State.RUNNING;
                }
                CharacterFaces = Facing.RIGHT;

                XMovement += 2; //TODO mettre en variable
            }
            
        }

        private void MoveLeft()
        {
            if (CharacterState != State.ATTACKING1)
            {
                if (CharacterState == State.IDLE)
                {
                    CharacterState = State.RUNNING;
                }
                CharacterFaces = Facing.LEFT;
                XMovement -= 2;
            }
        }

        public void Draw(SpriteBatch sb)
        {

#if DEBUG
            hitboxTexture = new Texture2D(sb.GraphicsDevice, 1, 1); // à terme, rendre la texture de hitbox générale?
            hitboxTexture.SetData(new[] { Color.Red });
            sb.Draw(hitboxTexture, HitBox, Color.White * 0.5f);
#endif
            CurrentSprite.Draw(sb, HorizontalFlip);

        }

        public enum State
        {
            IDLE,
            RUNNING,
            JUMPING,
            FALLING,
            ATTACKING1

        }
        public enum Facing
        {
            RIGHT,
            LEFT
        }
    }
}
