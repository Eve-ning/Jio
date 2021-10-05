using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace DIPProject
{
	/// <summary>
	///     Handles what happens if the player wants to create a random room
	/// </summary>
	public class CreateRoomHandler : MonoBehaviourPunCallbacks
    {
        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log(PhotonNetwork.NickName + " failed to Create Random Room.");
            PhotonNetwork.LoadLevel("Landing");
            base.OnCreateRoomFailed(returnCode, message);
        }

        #endregion

        #region Variables'

        [Tooltip("Maximum number of players allowed in the room.")]
        public const byte MAX_PLAYERS = 5;

        [Tooltip("The length of the room name")]
        public const int ROOM_NAME_LENGTH = 5;

        // Room name letters will be chosen from these letters
        private const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        [Tooltip("The Landing Animator")] [SerializeField]
        private Animator landingAnimatorHandler;

        #endregion

        #region Create Room Method

        /// <summary>
        ///     Creates a random room and loads it.
        ///     This is called from the button click.
        /// </summary>
        public void CreateRoom()
        {
            landingAnimatorHandler.SetTrigger("Create");
        }

        /// <summary>
        ///     This should only be called after the animation
        /// </summary>
        public void CreateRoomAfterAnimation()
        {
            var roomName = CreateRoomName();
            Debug.Log(PhotonNetwork.NickName + " is Creating Room " + roomName);
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions {MaxPlayers = MAX_PLAYERS}, null);
            PhotonNetwork.LoadLevel("Expedition");
        }

        /// <summary>
        ///     Creates a random room name of capital letters
        /// </summary>
        /// <returns>A string of random letters selected from LETTERS.</returns>
        private string CreateRoomName()
        {
            var letters = new char[ROOM_NAME_LENGTH];
            for (var i = 0; i < ROOM_NAME_LENGTH; i++) letters[i] = LETTERS[Random.Range(0, LETTERS.Length)];

            return new string(letters);
        }

        #endregion
    }
}