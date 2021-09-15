using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


namespace Com.PhotonPunTut.DIPProject
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
		#region Public Fields
		public static GameManager Instance;
		#endregion

		#region Photon Callbacks

		public override void OnLeftRoom()
		{
            SceneManager.LoadScene(0);
			base.OnLeftRoom();
		}

		public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
		{
			Debug.LogFormat("Player {0} entered room", newPlayer.NickName);

			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("Master Client ? {0}", newPlayer.IsMasterClient);
				LoadArena();
			}

			base.OnPlayerEnteredRoom(newPlayer);
		}

		public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
		{
			Debug.LogFormat("Player {0} left room", otherPlayer.NickName);

			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("Master Client ? {0}", otherPlayer.IsMasterClient);
				LoadArena();
			}

			base.OnPlayerLeftRoom(otherPlayer);
		}

		#endregion

		#region Public Methods
		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
		}

		private void Start()
		{
			Instance = this;
		}

		#endregion

		#region Private Methods

		void LoadArena()
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				Debug.LogError("Trying to load Level but not Master Client!");
			}
			Debug.LogFormat("Loading Level {0}", PhotonNetwork.CurrentRoom.PlayerCount);
			PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
		}

		#endregion
	}
}
