using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
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
        public InputField uiUsername;

        [Tooltip("Enter Button")]
        public GameObject uiEnterButton;

        [Tooltip("Minimum Length before the Enter Button appears")]
        [SerializeField]
        private int minimumUsernameLength = 1;

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
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = "0.0";
        }

		void Update()
		{
            if (Input.GetKeyDown(KeyCode.Return) && ReadyToJoin())
			{
                foregroundAnimator.SetTrigger("Join");
                backgroundAnimator.SetTrigger("Join");
			}
        }

		#endregion

		#region Public Methods

		public void JoinLobby()
        {
            if (ReadyToJoin())
            {
                PhotonNetwork.NickName = uiUsername.text;
                PhotonNetwork.LoadLevel("Landing");
                PhotonNetwork.JoinLobby();
            }
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

        //Was able to connect to the master Server
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
            Debug.Log("Connected to Photon Server.");
        }

        #endregion
    }
}