using UnityEngine;
using System;
using System.Collections.Generic;

namespace DIPProject { 
    public class PlayerReskinning : MonoBehaviour {

        #region Variables

        // The SpriteRenderer of the Player
        public SpriteRenderer SpriteRenderer;
        
        // The current "state" of the cat, specifyable by Cat enum
        public Cat CurrentCat = Cat.Pusheen;
        
        // This is the enum of the Cat for the Unity Dropdown and for ease of use
        public enum Cat
        {
            Pusheen = 0,
            Black = 1,
            Jiggly = 2,
            Garfield = 3
        }

        // These should be consistent with the Cat enum.
        private static readonly string[] catNames = {
            "Pusheen", "Black", "Jiggly", "Garfield"
        };

        // This is the indexable described in Start()
        private Dictionary<Cat, Dictionary<string, Sprite>> catSpritesMap;

        #endregion

        #region MonoBehavior Callbacks

        private void Start()
        {
            catSpritesMap = new Dictionary<Cat, Dictionary<string, Sprite>>();
            
            // In this loop, we yield all corresponding Sprites within the Resources/Characters/ Folder
            // We place these sprites in an indexable Dict<Cat, Dict<string, Sprite>> catSpritesMap
            // E.g. catSpritesMap[Pusheen]["Back Fishing Left 2"] will yield that particular sprite.
            
            for (int i = 0; i < catNames.Length; i++)
            {
                var catName = catNames[i];
                
                // This will yield the sprites as an Object[]
                var catSprites = Resources.LoadAll("Characters/" + catName, typeof(Sprite));
                
                // This yields the catSpritesMap
                var catSpriteStringMap = new Dictionary<string, Sprite>();
                
                // Populate map
                foreach (var spriteObj in catSprites)
                {
                    var sprite = (Sprite)spriteObj;
                    
                    // This will throw an exception of duplicate file names
                    // This occurs because Texture2D and Sprite are both "Sprites"
                    try
                    {
                        // We ToLower to make it case insensitive
                        catSpriteStringMap.Add(sprite.name.ToLower(), sprite);
                    }
                    catch (ArgumentException) { }
                }
                // Append to the indexable map
                catSpritesMap.Add((Cat) i, catSpriteStringMap);
            }
        }

        private void Update()
        {
            UpdateCatWithInput();            
        }

        private void UpdateCatWithInput()
        {
            if (!Input.GetKey(KeyCode.C)) return;
            if (Input.GetKeyDown(KeyCode.Alpha1)) CurrentCat = Cat.Pusheen;
            else if (Input.GetKeyDown(KeyCode.Alpha2)) CurrentCat = Cat.Garfield;
            else if (Input.GetKeyDown(KeyCode.Alpha3)) CurrentCat = Cat.Jiggly;
            else if (Input.GetKeyDown(KeyCode.Alpha4)) CurrentCat = Cat.Black;
        }
        

        /// <summary>
        /// We use a Late Update here because the Animations actually occur after Update
        /// hence overriding the reskin. Thus we need to reskin only after animation. 
        /// </summary>
        private void LateUpdate()
        {
            if (SpriteRenderer.sprite == null) return;
            var currentSpriteName = SpriteRenderer.sprite.name;
            // Here, we attempt to yield the Cat Sprite.
            // TryGetValue only attempts to get it without throwing an exception
            // If it gets it, it places it into value
            // Else, it will return a false.
            if (!catSpritesMap[CurrentCat].TryGetValue(currentSpriteName.ToLower(), out var value)) return;
                
            // We replace the sprite here.
            SpriteRenderer.sprite = value;
        }

        #endregion
    }
}