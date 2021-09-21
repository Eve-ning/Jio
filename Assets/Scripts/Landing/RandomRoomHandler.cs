using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DIPProject
{
    /// <summary>
    /// Handles what happens if the player wants to create a random room
    /// </summary>
    public class RandomRoomHandler : MonoBehaviourPunCallbacks
    {
        #region Variables

        //public GameObject uiCanvas;
        //public GameObject uiCreateButton;

        [Tooltip("If the player is triggering this region.")]
        private bool triggered = false;

        [Tooltip("Maximum number of players allowed in the room.")]
        public const byte MAX_PLAYERS = 5;

        public const int ROOM_NAME_LENGTH = 5;
        private const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) && triggered) RandomRoom();
        }
        #endregion

        #region Random Room Method

        /// <summary>
        /// Creates a random room and loads it directly.
        /// This is usually called from the animator event.
        /// </summary>
        public void RandomRoom()
        {
            string roomName = RandomRoomName();

            Debug.Log(PhotonNetwork.NickName + " is Creating Room " + roomName);
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { MaxPlayers = MAX_PLAYERS }, null);
            PhotonNetwork.LoadLevel("Expedition");
        }

        /// <summary>
        /// Alias to RandomRoom().
        /// This should only be called by animation events
        /// </summary>
        public void TriggerToRandomRoom()
		{
            RandomRoom();
		}

        /// <summary>
        /// Creates a random room name of capital letters
        /// </summary>
        /// <returns></returns>
        private string RandomRoomName()
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
            Debug.Log(PhotonNetwork.NickName + " failed to Create Random Room.");
            PhotonNetwork.LoadLevel("Landing");
			base.OnCreateRoomFailed(returnCode, message);
		}

		#endregion
	}
}