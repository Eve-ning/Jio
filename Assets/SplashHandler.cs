using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIPProject
{
    public class SplashHandler : MonoBehaviourPunCallbacks
    {
        [Tooltip("Username UI")]
        public InputField uiUsername;

        [Tooltip("Enter Button")]
        public GameObject uiEnterButton;

        [Tooltip("Minimum Length before the Enter Button appears")]
        [SerializeField]
        private int minimumUsernameLength = 3;

        #region MonoBehaviour Callbacks

        // Start is called before the first frame update
        void Start()
        {
            uiEnterButton.SetActive(false);
            Connect();
        }

        #endregion

        #region Public Methods

        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinLobby();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = "0.0";
            }
        }

        public void validateUsername()
        {
            uiEnterButton.SetActive(uiUsername.text.Length >= minimumUsernameLength && PhotonNetwork.IsConnected);
        }

        public void joinLobby()
        {
            PhotonNetwork.NickName = uiUsername.text;
            PhotonNetwork.LoadLevel("Landing");
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        //Was able to connect to the master Server
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
            Debug.Log("Connected to Photon Server.");
        }

        #endregion
    }
}