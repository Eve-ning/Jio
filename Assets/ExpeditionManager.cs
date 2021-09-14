using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIPProject
{
    public class ExpeditionManager : MonoBehaviourPunCallbacks
    {
		#region Variables
		[SerializeField]
		private Text roomNameText;

		private string ROOM_NAME_PREFIX = "ROOM NAME: ";

		#endregion

		#region MonoBehavior Callbacks

		// Start is called before the first frame update
		void Start()
        {

        }



        // Update is called once per frame
        void Update()
        {

        }

		#endregion

		#region Public Methods

        public void LeaveRoom()
		{
            PhotonNetwork.LeaveRoom();
		}

		#endregion

		#region MonoBehaviorPunCallbacks Callbacks
		/// <summary>
		/// Once the player joins the room, we instantiate it in the middle with some height
		/// </summary>
		public override void OnJoinedRoom()
        {
            PhotonNetwork.Instantiate("Player2D", new Vector3(-10, 1, -3.2f), Quaternion.identity);
			roomNameText.text = ROOM_NAME_PREFIX + PhotonNetwork.CurrentRoom.Name;
            base.OnJoinedRoom();
        }


		public override void OnLeftRoom()
		{
			Debug.Log("Leaving Room.");
			PhotonNetwork.LoadLevel("Landing");
			base.OnLeftRoom();
		}

		public override void OnPlayerLeftRoom(Player otherPlayer)
		{
            Debug.Log(otherPlayer.NickName + " has left the room " + PhotonNetwork.CurrentRoom.Name);
			base.OnPlayerLeftRoom(otherPlayer);
		}

		#endregion
	}
}