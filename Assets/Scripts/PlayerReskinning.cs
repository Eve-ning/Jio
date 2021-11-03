using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerReskinning : MonoBehaviour {

    public SpriteRenderer SpriteRenderer;
    public Cat CurrentCat = Cat.Pusheen;
    
    public enum Cat
    {
        Pusheen = 0,
        Black = 1,
        Jiggly = 2,
        Garfield = 3
    }

    private static readonly string[] catNames = {
        "Pusheen", "Black", "Jiggly", "Garfield"
    };

    private Dictionary<Cat, Dictionary<string, Sprite>> catSpritesMap;

    private void Start()
    {
        catSpritesMap = new Dictionary<Cat, Dictionary<string, Sprite>>();
        for (int i = 0; i < catNames.Length; i++)
        {
            var catName = catNames[i];
            
            var catSprites = Resources.LoadAll("Characters/" + catName, typeof(Sprite));
            
            // This yields the catSpritesMap
            var catSpriteStringMap = new Dictionary<string, Sprite>();
            
            // Populate map
            foreach (var spriteObj in catSprites)
            {
                var sprite = (Sprite)spriteObj;
                print(sprite.name);
                try
                {
                    catSpriteStringMap.Add(sprite.name, sprite);
                }
                catch (ArgumentException) { }
            }
            catSpritesMap.Add((Cat) i, catSpriteStringMap);
        }
        // var sprite = catGO.GetComponent<SpriteRenderer>().sprite;
        // print(sprite.name);
    }

    private void LateUpdate()
    {
        var currentSpriteName = SpriteRenderer.sprite.name;
        if (!catSpritesMap[CurrentCat].TryGetValue(currentSpriteName, out var value)) return;
            
        SpriteRenderer.sprite = value;
    }
}