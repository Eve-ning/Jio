using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIPProject
{
    public class ExpeditionHandler : MonoBehaviourPunCallbacks
    {
		#region Variables
		[SerializeField]
		private Text roomNameText;
		// The reason for the dummy camera is so that the game scene isn't empty
		[Tooltip("The Dummy Camera used in the scene to be disabled on Start().")]
		[SerializeField]
		private GameObject dummyCamera;
		
		[Tooltip("The Dummy Camera used in the scene to be disabled on Start().")]
		[SerializeField]
		private Canvas roomUi;

		private string ROOM_NAME_PREFIX = "CODE: ";

		#endregion

		#region Public Methods

        public void LeaveRoom()
		{
            PhotonNetwork.LeaveRoom();
		}

		#endregion

		#region MonoBehavior Callbacks

		private void Start()
		{
			dummyCamera.SetActive(false);
		}

		#endregion


		#region MonoBehaviorPunCallbacks Callbacks
		/// <summary>
		/// Once the player joins the room, we instantiate it in the middle with some height
		/// </summary>
		public override void OnJoinedRoom()
        {
            GameObject player = PhotonNetwork.Instantiate("Player2D", new Vector3(0, 1, 0), Quaternion.identity);
			Debug.Log("Room Name " + PhotonNetwork.CurrentRoom.Name);
			roomNameText.text = ROOM_NAME_PREFIX + PhotonNetwork.CurrentRoom.Name;
			Camera camera = player.GetComponentInChildren<Camera>();
			roomUi.worldCamera = camera;
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