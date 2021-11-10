using UnityEngine;
using System;
using System.Collections.Generic;

using TMPro;
using Photon.Pun;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace DIPProject
{
    /// <summary>
    ///     The purpose of this class is to set the landing intro to be active.
    ///     By default, in the editor, it's inactive because it helps the devs see the scene.
    /// </summary>
    public class LandingHandler : MonoBehaviour
    {
        
        public GameObject landingIntro;
        public TMP_Text[] fishCounts;
        private bool fishQuery = false; 

        private string getConn = @"server=dipdatabase.cvxt4s3abkq1.us-east-2.rds.amazonaws.com;port=3306;username=admin;password=admin2073";

        // Start is called before the first frame update
        private void Start()
        {
            landingIntro.SetActive(true);
        }

        public void FishQuery()
        {
            if (fishQuery) return;

            try
            {
                using var conn = new MySqlConnection(getConn);
                conn.Open();

                for (int i = 0; i < 5; i ++ ){
                    string query = "SELECT Fish" + i + " FROM users.inventory WHERE appID = '" + PhotonNetwork.NickName + "'";
                    using var cmd = new MySqlCommand(query, conn);

                    using MySqlDataReader rdr = cmd.ExecuteReader();

                    List<String> lstStr = new List<String>();
                    while (rdr.Read())
                    {
                        lstStr.Add(rdr[0].ToString());
                    }

                    fishCounts[i].text = lstStr[0];
                }
                conn.Close();

                fishQuery = true;
                
            } catch {

                Debug.Log("test Error...");

            }
        }
    }
}