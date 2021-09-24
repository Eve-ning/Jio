using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIPProject { 
    public class InviteHandler : MonoBehaviour
    {
	    #region Variables
	    public GameObject uiInviteScreen;
        public CustomRoomHandler customRoomHandler;

        [SerializeField]
        [Tooltip("Room name for John's Invite")]
        private const string JoinJohnRoom = "JOHNR";
        [SerializeField]
        [Tooltip("Room name for Boya's Invite")]
        private const string JoinBoyaRoom = "BOYAR";
	    #endregion

	    #region Public Methods

        public void JoinJohn()
		{
            customRoomHandler.CustomRoom();
		}
	    public void ShowScreen()
        {
            uiInviteScreen.SetActive(true);
        }

        public void HideScreen()
        {
            uiInviteScreen.SetActive(false);
        }
	    #endregion
    }
}