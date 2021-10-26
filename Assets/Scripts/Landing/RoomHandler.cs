using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DIPProject
{
	/// <summary>
	///     Handles what happens if the player wants to create a random room
	/// </summary>
	public abstract class RoomHandler : MonoBehaviourPunCallbacks
    {
        #region Variables

        [Tooltip("Maximum number of players allowed in the room.")]
        private const byte MaxPlayers = 5;

        [Tooltip("The length of the room name")]
        protected const int RoomNameLength = 5;
 
        [Tooltip("The Landing Animator")] [SerializeField]
        protected Animator landingAnimatorHandler;

        #endregion
        
        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log(PhotonNetwork.NickName + " failed to Create Random Room.");
            PhotonNetwork.LoadLevel("Landing");
            base.OnCreateRoomFailed(returnCode, message);
        }

        #endregion

        #region Join Or Create Room
        // Note that this will be called by both
        
        /// <summary>
        /// Joins or creates a new room based on the roomName provided
        /// </summary>
        /// <param name="roomName"></param>
        protected void JoinOrCreateRoom(string roomName)
        {
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions {MaxPlayers = MaxPlayers}, null);
            
            var sum = 0;
            foreach (var c in roomName) sum += System.Convert.ToInt32(c);

            const int FIRST_EXP = 3;
            const int LAST_EXP = 7;
            sum %= LAST_EXP - FIRST_EXP;
            print("Joining expedition " + sum + " with room name " + roomName);

            PhotonNetwork.LoadLevel(sum + FIRST_EXP);
        }
   
        #endregion
    }
}