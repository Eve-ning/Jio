using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.PhotonPunTut.DIPProject
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
		#region Private Serializable Fields

		[Tooltip("Maximum Players in a room.")]
		[SerializeField]
		private byte maxPlayersPerRoom = 4;

		[Tooltip("UI Panel for Playing and Setting Username")]
		[SerializeField]
		private GameObject controlPanel;

		[Tooltip("Label to inform connection in progress")]
		[SerializeField]
		private GameObject progressLabel;

        #endregion

        #region Private Fields

        string gameVersion = "1";
		/// <summary>
		/// This is to avoid the player from automatically connecting on quitting scene.
		/// This will be enabled when the player clicks the connect button.
		/// </summary>
		bool enableConnecting;

		#endregion

		#region MonoBehavior CallBacks

		private void Awake()
		{
            PhotonNetwork.AutomaticallySyncScene = true;
		}

		// Start is called before the first frame update
		void Start()
        {
			progressLabel.SetActive(false);
			controlPanel.SetActive(true);
			enableConnecting = false;
        }

		#endregion

		#region MonoBehaviorPunCallbacks CallBacks

		public override void OnConnectedToMaster()
		{
			Debug.Log("OnConnectedToMaster was called.");
			Connect();
			base.OnConnectedToMaster();
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			progressLabel.SetActive(false);
			controlPanel.SetActive(true);

			Debug.LogWarningFormat("Disconnected due to {0}", cause);

			base.OnDisconnected(cause);
		}

		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			Debug.Log("Failed to Join Room, no random room available.");

			PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
			base.OnJoinRoomFailed(returnCode, message);
		}

		public override void OnJoinedRoom()
		{
			Debug.Log("Successfully Joined Room!");
			if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
			{
				Debug.Log("Loading Room for 1");
				PhotonNetwork.LoadLevel("Room for 1");
			}

			base.OnJoinedRoom();
		}

		#endregion

		#region Public Methods

		public void Connect()
		{
			if (PhotonNetwork.IsConnected)
			{
				if (enableConnecting)
				{
					progressLabel.SetActive(true);
					controlPanel.SetActive(false);
					PhotonNetwork.JoinRandomRoom();
				}
			}
			else
			{
				PhotonNetwork.ConnectUsingSettings();
				PhotonNetwork.GameVersion = gameVersion;
			}
		}

		public void EnableConnect()
		{
			enableConnecting = true;
		}

		#endregion
    }
}