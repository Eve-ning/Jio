using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DIPProject
{
    public class JoinRoomHandler : MonoBehaviourPunCallbacks
    {
        #region Variables

        public GameObject uiCanvas;
        public GameObject uiJoinButton;
        public InputField uiRoomNameInput;

        #endregion

        #region Public Methods

        private bool IsMineColliding(Collider2D collider)
        {
            return collider.gameObject.GetComponent<PhotonView>().IsMine;
        }
        public void ShowCanvas()
        {
            uiCanvas.SetActive(true);
        }
        public void HideCanvas()
        {
            uiCanvas.SetActive(false);
        }

        /// <summary>
        /// Attempts to Join a Room specified for the uiRoomNameInput
        /// </summary>
        public void JoinRoom()
        {
            string roomName = uiRoomNameInput.text;
            if (roomName.Length < CreateRoomHandler.ROOM_NAME_LENGTH)
			{
                Debug.Log("Room Name Received is too short " + roomName);
                return;
			}
            Debug.Log(PhotonNetwork.NickName + " is Attempting to Join Room " + roomName);
            PhotonNetwork.JoinRoom(roomName, null);
            PhotonNetwork.LoadLevel("Expedition");
        }

        #endregion

        #region Collider2D Callbacks 

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (IsMineColliding(collider)) ShowCanvas();
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (IsMineColliding(collider)) HideCanvas();
        }

		#endregion

		#region MonoBehaviour Callbacks

		private void Update()
		{
            if (Input.GetKeyDown(KeyCode.Return)) JoinRoom();
        }

		#endregion

		#region MonoBehaviourPunCallbacks Callbacks

		/// <summary>
		/// We load the level if we successfully joined the room.
		/// 
		/// We have it as separate functions to be explicit on the flow.
		/// </summary>
		public override void OnJoinedRoom()
		{
            Debug.Log(PhotonNetwork.NickName + " Joined Room " + uiRoomNameInput.text);
            PhotonNetwork.LoadLevel("Expedition");
            base.OnJoinedRoom();
		}

        /// <summary>
        /// Just in case we failed to join a room that doesn't exist. We don't load level.
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
		public override void OnJoinRoomFailed(short returnCode, string message)
		{
            Debug.Log(PhotonNetwork.NickName + " Failed to Join Room " + uiRoomNameInput.text);
			base.OnJoinRoomFailed(returnCode, message);
		}

        #endregion

        #region Input Validation

        /// <summary>
        /// Capitalizes the input
        /// </summary>
        public void CapitalizeInput()
		{
            uiRoomNameInput.text = uiRoomNameInput.text.ToUpper();
        }

        /// <summary>
        /// Truncates the input if it's too long
        /// </summary>
        public void TruncateInput()
		{
            string roomName = uiRoomNameInput.text;
            uiRoomNameInput.text = roomName.Length > 5 ? roomName.Substring(0, 5) : roomName;
		}

		#endregion
	}
}