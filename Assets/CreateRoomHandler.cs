using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DIPProject
{
    public class CreateRoomHandler : MonoBehaviour
    {
        #region Variables

        public GameObject uiCanvas;
        public GameObject uiCreateButton;
        public InputField uiRoomNameInput;

        [Tooltip("Maximum number of players allowed in the room.")]
        [SerializeField]
        private byte maxPlayers = 5;

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

        #region Public Methods

        /// <summary>
        /// Checks if the trigger is being triggered by my own view.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        private bool isMineColliding(Collider2D collider)
        {
            return collider.gameObject.GetComponent<PhotonView>().IsMine;
        }

        /// <summary>
        /// Shows the UI Canvas
        /// </summary>
        public void showCanvas()
        {
            uiCanvas.SetActive(true);
        }

        /// <summary>
        /// Hides the UI Canvas
        /// </summary>
        public void hideCanvas()
        {
            uiCanvas.SetActive(false);
        }

        #endregion

        #region Create Room Method

        public void createRoom()
        {
            Debug.Log(PhotonNetwork.NickName + " is Creating Room " + uiRoomNameInput.text);
            PhotonNetwork.JoinOrCreateRoom(uiRoomNameInput.text, new RoomOptions() { MaxPlayers = maxPlayers }, null);
            PhotonNetwork.LoadLevel("Expedition");
        }

        #endregion
    }
}