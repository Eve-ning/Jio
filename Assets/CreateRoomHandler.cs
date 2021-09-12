using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DIPProject
{
    /// <summary>
    /// Handles what happens if the player wants to create a room
    /// </summary>
    public class CreateRoomHandler : MonoBehaviourPunCallbacks
    {
        #region Variables

        public GameObject uiCanvas;
        public GameObject uiCreateButton;
        public InputField uiRoomNameInput;

        [Tooltip("Maximum number of players allowed in the room.")]
        [SerializeField]
        private byte maxPlayers = 5;

        public const int ROOM_NAME_LENGTH = 5;
        private const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        #endregion

        #region Collider2D Callbacks 

        /// <summary>
        /// Open Up canvas if we Enter
        /// </summary>
        /// <param name="collider"></param>
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (IsMineColliding(collider)) ShowCanvas();
        }

        /// <summary>
        /// Close canvas if we Exit
        /// </summary>
        /// <param name="collider"></param>
        private void OnTriggerExit2D(Collider2D collider)
        {
            if (IsMineColliding(collider)) HideCanvas();
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return)) CreateRoom();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if the trigger is being triggered by my own view.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        private bool IsMineColliding(Collider2D collider)
        {
            return collider.gameObject.GetComponent<PhotonView>().IsMine;
        }

        /// <summary>
        /// Shows the UI Canvas
        /// </summary>
        public void ShowCanvas()
        {
            uiCanvas.SetActive(true);
        }

        /// <summary>
        /// Hides the UI Canvas
        /// </summary>
        public void HideCanvas()
        {
            uiCanvas.SetActive(false);
        }

        #endregion

        #region Create Room Method

        public void CreateRoom()
        {
            string roomName = CreateRandomRoomName();

            Debug.Log(PhotonNetwork.NickName + " is Creating Room " + roomName);
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { MaxPlayers = maxPlayers }, null);
            PhotonNetwork.LoadLevel("Expedition");
        }


        private string CreateRandomRoomName()
        {
            char[] letters = new char[ROOM_NAME_LENGTH];
            for (int i = 0; i < ROOM_NAME_LENGTH; i++)
			{
                letters[i] = LETTERS[Random.Range(0, LETTERS.Length)];
            }

            return new string(letters);
		}

		#endregion

		#region MonoBehaviourPunCallbacks Callbacks

		public override void OnCreateRoomFailed(short returnCode, string message)
		{
            Debug.Log(PhotonNetwork.NickName + " is failed to Create Room " + uiRoomNameInput.text);

			base.OnCreateRoomFailed(returnCode, message);
		}

		#endregion
	}
}