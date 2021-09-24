using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
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
        public InputField uiRoomNameInput;

        [SerializeField]
        private Animator landingAnimatorHandler;

        [Tooltip("If the canvas is active")]
        private bool active = false;

        #endregion

        #region Public Methods

        public void ShowScreen()
        {
            uiCanvas.SetActive(true);
            // Force User to select input field
            EventSystem.current.SetSelectedGameObject(uiRoomNameInput.gameObject, null);
            active = true;
        }
        public void HideScreen()
        {
            uiCanvas.SetActive(false);
            active = false;
        }

        /// <summary>
        /// Attempts to Join a Room specified for the uiRoomNameInput
        /// This will call the trigger if the condition passes
        /// </summary>
        public void CustomRoom()
        {
            if (uiRoomNameInput.text.Length == CreateRoomHandler.ROOM_NAME_LENGTH) landingAnimatorHandler.SetTrigger("Custom");
        }

        /// <summary>
        /// Assuming the condition has passed, this will move the user to the custom room
        /// This should only be called by animation events
        /// </summary>
        public void CustomRoomAfterAnimation()
		{
            Debug.Log(PhotonNetwork.NickName + " is Attempting to Join / Create Room " + uiRoomNameInput.text);
            PhotonNetwork.JoinOrCreateRoom(uiRoomNameInput.text, new RoomOptions() { MaxPlayers = CreateRoomHandler.MAX_PLAYERS }, null);
            PhotonNetwork.LoadLevel("Expedition");
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
		{
            if (Input.GetKeyDown(KeyCode.Return) && active) CustomRoom();
            if (Input.GetKeyDown(KeyCode.Escape) && active) HideScreen();
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
        /// Capitalizes, Truncates and "Letter-only" Constraints the input
        /// </summary>
        public void ValidateInput()
		{
            string roomName = uiRoomNameInput.text;
            roomName = roomName.ToUpper();
            roomName = roomName.Length > 5 ? roomName.Substring(0, 5) : roomName;
            Regex rgx = new Regex("[^a-zA-Z -]");
            uiRoomNameInput.text = rgx.Replace(roomName, "");
        }

		#endregion
	}
}