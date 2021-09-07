using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DIPProject
{
    public class JoinRoomHandler : MonoBehaviour
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

		#region Join Room Method
		public void joinRoom()
        {
            Debug.Log(PhotonNetwork.NickName + " is Joining Room " + uiRoomNameInput.text);
            PhotonNetwork.JoinRoom(uiRoomNameInput.text, null);
            PhotonNetwork.LoadLevel("Expedition");
            return;
        }

		#endregion
	}
}