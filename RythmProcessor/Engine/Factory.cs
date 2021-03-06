﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.IO;
using RythmProcessor;
using Engine;
using Engine.CommonImagery;
using Engine.CharacterClasses;

namespace Engine
{
    public class Factory //attention pas thread safe
    {
        private static Factory instance = null;
        public MainGame mG;


        public static Factory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Factory();
                }
                return instance;
            }
        }

        public void SetMainGame(MainGame mG)
        {
            this.mG = mG;
        }

        public Texture2D LoadTexture(String assetPath) => mG.Content.Load<Texture2D>(assetPath);

        private Factory()
        {
        }
        //LoadMapRepresentation(nom perso)
        //LoadSideRepresentation(nom perso)
        //LoadSideInterface

        //public void LoadPlayer()
        //{
        //    JsonSerializerSettings settings = new JsonSerializerSettings
        //    {
        //        NullValueHandling = NullValueHandling.Ignore, //attention dino danger
        //        MissingMemberHandling = MissingMemberHandling.Ignore
        //    };

        //    StreamReader sr = new StreamReader("./Content/charactersList.json");
        //    String jsonFile = sr.ReadToEnd();
        //    CharactersListDTO characterList = JsonConvert.DeserializeObject<CharactersListDTO>(jsonFile, settings);

        //    JsonToPlayerCharacters(characterList);

        //    //monsieurBloc.mapRepresentation.idleMapSprite = new AnimatedSprite(mG.Content.Load<Texture2D>("vertical_object"), new Vector2(200, 200), 1); //TODO remplacer plus tard par une collection de sprites (voir un peu  la version des utopiales)
        //    //Player.Instance.currentCharacter = monsieurBloc;
        //}

        public AnimatedSprite LoadGreat()
        {
            AnimatedSprite greatAnimated = new AnimatedSprite(mG.Content.Load<Texture2D>("greatAnimated"), new Vector2(20, 80), 2, 7, Factory.GenerateDefaultFrameDurations(14));
            return greatAnimated;
        }


        private void JsonToPlayerCharacters(CharactersListDTO characterList) //TODO: attention à la possibilité de champs vides. Faire des vérifs pour
        {
            Player.Instance.charactersList = new List<Character>();

            foreach (CharacterDTO characterDTO in characterList.Characters)
            {
                Character character = new Character
                {
                    name = characterDTO.Name,
                    maxHP = characterDTO.Hp,
                    menuRepresentation = characterDTO.MenuRepresentation == null ? null : //c'est chiant de faire des ternaires, c'est obligé dans une classe imbriquée?
                    new MenuRepresentation
                    {
                        avatar5050 = String.IsNullOrEmpty(characterDTO.MenuRepresentation.Avatar5050) ? null :
                        mG.Content.Load<Texture2D>("Images/" + characterDTO.Name + "/" + characterDTO.MenuRepresentation.Avatar5050)
                    },
                    sideRepresentation = characterDTO.SideRepresentation == null ? null :
                    new SideRepresentation
                    {
                        spriteIdle = String.IsNullOrEmpty(characterDTO.SideRepresentation.Idle.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>("Images/" + characterDTO.Name + "/" + characterDTO.SideRepresentation.Idle.ImgFile),
                        Vector2.Zero, //voir ce qu'on fout de cette position dans le constructeur pas focément utile
                        characterDTO.SideRepresentation.Idle.Columns, characterDTO.SideRepresentation.Idle.Rows, 
                        GenerateFrameDurationsFromDTO(characterDTO.SideRepresentation.Idle)),

                        spriteRun = String.IsNullOrEmpty(characterDTO.SideRepresentation.Run.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>("Images/" + characterDTO.Name + "/" + characterDTO.SideRepresentation.Run.ImgFile),
                        Vector2.Zero,
                        characterDTO.SideRepresentation.Idle.Columns, characterDTO.SideRepresentation.Idle.Rows,
                        GenerateFrameDurationsFromDTO(characterDTO.SideRepresentation.Run)),


                        spriteJump = String.IsNullOrEmpty(characterDTO.SideRepresentation.Jump.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>("Images/" + characterDTO.Name + "/" + characterDTO.SideRepresentation.Jump.ImgFile),
                        Vector2.Zero,
                        characterDTO.SideRepresentation.Idle.Columns, characterDTO.SideRepresentation.Idle.Rows,
                        GenerateFrameDurationsFromDTO(characterDTO.SideRepresentation.Jump)),

                        spriteFall = String.IsNullOrEmpty(characterDTO.SideRepresentation.Fall.ImgFile) ? null :
                        new AnimatedSprite(mG.Content.Load<Texture2D>("Images/" + characterDTO.Name + "/" + characterDTO.SideRepresentation.Fall.ImgFile),
                        Vector2.Zero,
                        characterDTO.SideRepresentation.Idle.Columns, characterDTO.SideRepresentation.Idle.Columns,
                        GenerateFrameDurationsFromDTO(characterDTO.SideRepresentation.Fall)),

                    },
                    //mapRepresentation = characterDTO.MapRepresentation == null ? null :
                    //new MapRepresentation
                    //{ //C'est défini dans le json mais pas présent dans le Content et inutile pour ce projet, donc je commente
                    //    idle_front = String.IsNullOrEmpty(characterDTO.MapRepresentation.Idle_front.ImgFile) ? null :
                    //    new AnimatedSprite(mG.Content.Load<Texture2D>("Images/" + characterDTO.MapRepresentation.Idle_front.ImgFile),
                    //    Vector2.Zero,
                    //    characterDTO.MapRepresentation.Idle_front.Columns, characterDTO.MapRepresentation.Idle_front.FrameSpeed),
                    //    idle_back = String.IsNullOrEmpty(characterDTO.MapRepresentation.Idle_back.ImgFile) ? null :
                    //    new AnimatedSprite(mG.Content.Load<Texture2D>("Images/" + characterDTO.MapRepresentation.Idle_back.ImgFile),
                    //    Vector2.Zero,
                    //    characterDTO.MapRepresentation.Idle_back.Columns, characterDTO.MapRepresentation.Idle_back.FrameSpeed),
                    //    run = String.IsNullOrEmpty(characterDTO.MapRepresentation.Run.ImgFile) ? null :
                    //    new AnimatedSprite(mG.Content.Load<Texture2D>("Images/" + characterDTO.MapRepresentation.Run.ImgFile),
                    //    Vector2.Zero,
                    //    characterDTO.MapRepresentation.Run.Columns, characterDTO.MapRepresentation.Run.FrameSpeed),
                    //}

                };


                Player.Instance.charactersList.Add(character);
            }


        }

        public void Load() {
            Fonts.Instance.Load(mG);
        }

        private int[] GenerateFrameDurationsFromDTO(SpriteStateDTO spriteStateDTO)
        {
            int[] framesSpeed;

            if (File.Exists("./Content/" + spriteStateDTO.ImgFile + ".json"))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore, //attention dino danger
                    MissingMemberHandling = MissingMemberHandling.Ignore
                }; //TODO est ce qu'on aurait pas ça dans la Factory plutôt?

                StreamReader sr = new StreamReader("./Content/Images" + spriteStateDTO.ImgFile + ".json"); //TODO mettre le chemin du fichier à part, si on veut un truc genre ./Content/Images/nomperso? ?
                String jsonFile = sr.ReadToEnd();
                AnimationJsonDTO animationJson = JsonConvert.DeserializeObject<AnimationJsonDTO>(jsonFile, settings);

                framesSpeed = new int[animationJson.Frames.Length];

                for (int i = 0; i < animationJson.Frames.Length; i++)
                {
                    framesSpeed[i] = animationJson.Frames[i].Duration;
                }


            }
            else
            {
                framesSpeed = GenerateDefaultFrameDurations(spriteStateDTO.Rows * spriteStateDTO.Columns);
            }

            return framesSpeed;
        }
        public static int[] GenerateDefaultFrameDurations(int frameCount)
        {
            int[] framesSpeed = new int[frameCount];
            for (int i = 0; i < frameCount; i++)
            {
                framesSpeed[i] = 100;
            }
            return framesSpeed;
        }
    }

}
