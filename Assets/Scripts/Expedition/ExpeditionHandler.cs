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
		
		[Tooltip("The UI to snap to the player camera on instantiation.")]
		[SerializeField]
		private Canvas[] roomUi;

		private string ROOM_NAME_PREFIX = "CODE: ";

		[Tooltip("The duration of the room, in seconds")]
		[SerializeField]
		private int roomDuration = 10;

		[SerializeField]
		private Text[] playerList;

		#endregion

		#region RPC Methods

		public void updateRoomDuration(int roomDuration)
		{
			this.roomDuration = roomDuration;
		}

		#endregion

		#region Public Methods

		public void Campfire()
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

			// Gets the camera of the new instantiated player and shifts camera to it
			Camera camera = player.GetComponentInChildren<Camera>();
			foreach (var ui in roomUi) ui.worldCamera = camera;

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

		public void UpdatePlayerList()
		{
			var position = 0;

			foreach (var item in playerList) item.text = "-";
			foreach (var player in PhotonNetwork.CurrentRoom.Players)
			{
				if (player.Value.IsMasterClient) { playerList[0].text = player.Value.NickName; }
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