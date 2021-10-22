using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace DIPProject
{
	/// <summary>
	///     Handles what happens if the player wants to create a random room
	/// </summary>
	public class CreateRoomHandler : RoomHandler
    {
        #region Variables
        // Room name letters will be chosen from these letters
        private const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log(PhotonNetwork.NickName + " failed to Create Random Room.");
            PhotonNetwork.LoadLevel("Landing");
            base.OnCreateRoomFailed(returnCode, message);
        }

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
            JoinOrCreateRoom(CreateRoomName());
        }

        /// <summary>
        ///     Creates a random room name of capital letters
        /// </summary>
        /// <returns>A string of random letters selected from LETTERS.</returns>
        private string CreateRoomName()
        {
            var letters = new char[RoomNameLength];
            for (var i = 0; i < RoomNameLength; i++) 
                letters[i] = Letters[Random.Range(0, Letters.Length)];

            return new string(letters);
        }

        #endregion
    }
}