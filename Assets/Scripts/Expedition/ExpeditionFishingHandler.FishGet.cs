using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DIPProject
{
    public partial class ExpeditionFishingHandler 
    {
        #region Variables

        public enum Fish
        {
            Neemo   = 0,
            Angler  = 1,
            Generic = 2,
            Shroom  = 3,
            Tortle  = 4 
        }
        
        [Tooltip("The fish image shown when the user has gotten a fish.")]
        public Image fishGetImage;
        [Tooltip("The text that comes along with the fish getting animation.")]
        public TMP_Text fishGetText;

        [Tooltip("Fish sprites.")]
        public Sprite[] fishSprites;

        [Tooltip("Fish sprite names respectively.")]
        public List<string> fishSpriteNames = new List<string>
        {
            "Neemo",
            "Angler",
            "Generic",
            "Shroom",
            "Tortle"
        };

        [Tooltip("The format of the fishGetText")]
        public string fishTextFormat = "! You Got a <color=#{0}>{1}</color> !";

        [Tooltip("The hex value hues of the fish respectively, do not include the #")]
        public string[] fishTextColor;
        #endregion

        #region Player Movement Methods

        public void setGetFish(Fish f)
        {
            int fishIndex = (int) f;
            fishGetImage.sprite = fishSprites[fishIndex];
            fishGetText.text = String.Format(fishTextFormat, fishTextColor[fishIndex], fishSpriteNames[fishIndex]);
        }
        public void setGetFishRandom()
        {
            setGetFish((Fish) Random.Range(0, 5));
        }

        #endregion

    }
}