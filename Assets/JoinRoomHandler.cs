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

        private bool isMineColliding(Collider2D collider)
        {
            return collider.gameObject.GetComponent<PhotonView>().IsMine;
        }
        public void showCanvas()
        {
            uiCanvas.SetActive(true);
        }

        public void hideCanvas()
        {
            uiCanvas.SetActive(false);
        }

        /// <summary>
        /// Attempts to Join a Room specified for the uiRoomNameInput
        /// </summary>
        public void joinRoom()
        {
            Debug.Log(PhotonNetwork.NickName + " is Attempting to Join Room " + uiRoomNameInput.text);
            PhotonNetwork.JoinRoom(uiRoomNameInput.text, null);
            return;
        }

        #endregion

        #region Collider2D Callbacks 

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (isMineColliding(collider))
            {
                showCanvas();
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (isMineColliding(collider))
            {
                hideCanvas();
            }
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
	}
}