using System;
using Photon.Pun;
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

        private void Start()
        {
            UpdateName();
        }

        public void UpdateName()
        {
            profileName.text = PhotonNetwork.NickName;
        }
    }
}