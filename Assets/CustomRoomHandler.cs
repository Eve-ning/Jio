using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


namespace DIPProject
{
    /// <summary>
    /// Handles what happens if the user wants to create a custom room name
    /// </summary>
    public class CustomRoomHandler : MonoBehaviourPunCallbacks
    {
        #region Variables

        public GameObject uiCanvas;
        public GameObject uiJoinButton;
        public InputField uiRoomNameInput;

        [Tooltip("If the player is triggering this region.")]
        private bool triggered;

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
        public void CustomRoom()
        {
            CustomRoom(uiRoomNameInput.text);
        }

        public void CustomRoom(string roomName)
		{
            if (roomName.Length < RandomRoomHandler.ROOM_NAME_LENGTH)
            {
                Debug.Log("Room Name Received is too short " + roomName);
                return;
            }
            Debug.Log(PhotonNetwork.NickName + " is Attempting to Join / Create Room " + roomName);
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { MaxPlayers = RandomRoomHandler.MAX_PLAYERS }, null);
            PhotonNetwork.LoadLevel("Expedition");
        }

        #endregion

        #region Collider2D Callbacks 

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (IsMineColliding(collider)) ShowCanvas();
            triggered = true;
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (IsMineColliding(collider)) HideCanvas();
            triggered = false;
        }

		#endregion

		#region MonoBehaviour Callbacks

		private void Update()
		{
            if (Input.GetKeyDown(KeyCode.Return) && triggered) CustomRoom();
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
            Debug.Log(PhotonNetwork.NickName + " Joined Custom Room " + uiRoomNameInput.text);
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
            PhotonNetwork.LoadLevel("Landing");
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
        
        /// <summary>
        /// Truncates the input if it's too long
        /// </summary>
        public void EnsureAsciiInput()
		{
            string roomName = uiRoomNameInput.text;
            Regex rgx = new Regex("[^a-zA-Z -]");
            uiRoomNameInput.text = rgx.Replace(roomName, "");
        }

		#endregion
	}
}