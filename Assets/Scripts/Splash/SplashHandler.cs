using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DIPProject
{
    /// <summary>
    /// Handles the splash screen interactions
    /// </summary>
    public class SplashHandler : MonoBehaviourPunCallbacks
    {
		#region Variables

		[Tooltip("Username UI")]
        public TMP_InputField uiUsername;

        [Tooltip("Enter Button")]
        public GameObject uiEnterButton;

        [Tooltip("Minimum Length before the Enter Button appears")]
        [SerializeField]
        private int minimumUsernameLength = 3;

        [Tooltip("The foreground animator, the Logo, Input and Join Button")]
        [SerializeField]
        private Animator foregroundAnimator;

        [Tooltip("The background animator, the clouds")]
        [SerializeField]
        private Animator backgroundAnimator;    

        #endregion

        #region MonoBehaviour Callbacks

        // Start is called before the first frame update
        void Start()
        {
            uiEnterButton.SetActive(false);
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = "0.0";
        }

		void Update()
		{
            if (Input.GetKeyDown(KeyCode.Return) && ReadyToJoin()) JoinLobby();
        }

		#endregion

		#region Public Methods

        /// <summary>
        /// Attempts to Join the Lobby
        /// </summary>
		public void JoinLobby()
        {
            if (ReadyToJoin())
            {
                foregroundAnimator.SetTrigger("Join");
                backgroundAnimator.SetTrigger("Join");
            }
        }

        /// <summary>
        /// This should only be called by the animation event trigger.
        /// </summary>
        public void JoinLobbyAfterAnimation()
		{
            PhotonNetwork.NickName = uiUsername.text;
            PhotonNetwork.JoinLobby();
            PhotonNetwork.LoadLevel("Landing");
        }

        /// <summary>
        /// Validates if the player inputs are good and if is connected
        /// </summary>
        /// <returns></returns>
        public bool ReadyToJoin()
		{
            return uiUsername.text.Length >= minimumUsernameLength && PhotonNetwork.IsConnected;
        }

        /// <summary>
        /// Checks if the username is good, if it's good, then the Join button will appear
        /// </summary>
        public void ButtonUpdate()
        {
            uiEnterButton.SetActive(ReadyToJoin());
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        /// <summary>
        /// Was able to connect to the master Server
        /// </summary>
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Photon Server.");
        }

        #endregion
    }
}