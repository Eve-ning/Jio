using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DIPProject
{
    public class ProfileHandler : MonoBehaviourPunCallbacks
    {
        [Tooltip("The Profile Page Name TMP Text")]
        public TMP_Text profileName;

		public override void OnJoinedLobby()
		{
            UpdateName();
            base.OnJoinedLobby();
        }
		public override void OnLeftRoom()
		{
            UpdateName();
            base.OnJoinedLobby();
        }

        public void UpdateName()
		{
            profileName.text = PhotonNetwork.NickName;
        }
    }
}