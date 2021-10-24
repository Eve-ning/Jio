using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace DIPProject
{
    public class ExpeditionHandler : MonoBehaviourPunCallbacks
    {
        #region Variables

        [SerializeField] private TMP_Text roomNameText;

        // The reason for the dummy camera is so that the game scene isn't empty
        [Tooltip("The Dummy Camera used in the scene to be disabled on Start().")] [SerializeField]
        private GameObject dummyCamera;

        [Tooltip("The spawn point of the player. A 0-1 Random float will be added to avoid stacking.")]
        [SerializeField]
        private Vector2 spawnPoint = Vector2.zero;
        
        [Tooltip("The Canvases to snap to the player camera on instantiation.")] [SerializeField]
        private Canvas[] canvasSwap;

        private const string RoomNamePrefix = "CODE: ";

        [SerializeField] private TMP_Text[] playerList;

        #endregion
        
        #region MonoBehavior Callbacks

        private void Start()
        {
            dummyCamera.SetActive(false);
        }

        #endregion

        #region Public Methods

        public void Campfire()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region MonoBehaviorPunCallbacks Callbacks

        /// <summary>
        ///     Once the player joins the room, we instantiate it in the middle with some height
        /// </summary>
        public override void OnJoinedRoom()
        {
            var player = 
                PhotonNetwork.Instantiate(
                    "Player2D",
                    new Vector3(spawnPoint.x + Random.value * 0.1f, spawnPoint.y + Random.value * 0.1f, 0),
                    Quaternion.identity);
            
            Debug.Log("Room Name " + PhotonNetwork.CurrentRoom.Name);
            roomNameText.text = RoomNamePrefix + PhotonNetwork.CurrentRoom.Name;

            // Gets the camera of the new instantiated player and shifts camera to it
            var playerCamera = player.GetComponentInChildren<Camera>();
            foreach (var ui in canvasSwap) ui.worldCamera = playerCamera;

            UpdatePlayerList();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            UpdatePlayerList();
            base.OnPlayerEnteredRoom(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log(otherPlayer.NickName + " has left the room " + PhotonNetwork.CurrentRoom.Name);
            UpdatePlayerList();
            base.OnPlayerLeftRoom(otherPlayer);
        }

        private void UpdatePlayerList()
        {
            var position = 0;

            foreach (var item in playerList) item.text = "-";
            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                if (player.Value.IsMasterClient)
                {
                    playerList[0].text = player.Value.NickName;
                }
                else
                {
                    playerList[position + 1].text = player.Value.NickName;
                    position++;
                }
            }
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Leaving Room.");
            PhotonNetwork.LoadLevel("Landing");
            base.OnLeftRoom();
        }

        #endregion
    }
}