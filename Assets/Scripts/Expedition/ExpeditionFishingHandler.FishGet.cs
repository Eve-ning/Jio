using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

using Photon.Pun;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace DIPProject
{
    public partial class ExpeditionFishingHandler 
    {
        #region Variables

        private int n;
        private string getConn = @"server=dipdatabase.cvxt4s3abkq1.us-east-2.rds.amazonaws.com;port=3306;username=admin;password=admin2073";

        public enum Fish
        {
            Generic = 0,
            Shroom  = 1,
            Angler  = 2,
            Neemo   = 3,
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
            "Generic",
            "Shroom",
            "Angler",
            "Neemo",
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
            RanNum();
            AddFish(n);
            setGetFish((Fish) n);
        }

        private void RanNum()
        {
            n = Random.Range(0, 5);
        }

        private void AddFish(int x)
        {
            try
            {

                using var conn = new MySqlConnection(getConn);
                conn.Open();

                string query = "UPDATE users.inventory SET Fish" + x + " = Fish" + x + " + 1 WHERE appID = '" + PhotonNetwork.NickName + "'";
                using var cmd = new MySqlCommand(query, conn);

                using MySqlDataReader rdr = cmd.ExecuteReader();

                conn.Close();
                
            } catch {

                Debug.Log("AddFish Error...");

            }
        }

        #endregion

    }
}