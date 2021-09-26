using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIPProject { 
    public class InviteHandler : MonoBehaviour
    {
	    #region Variables
        public CustomRoomHandler customRoomHandler;
        public InputField uiRoomNameInput;

        [SerializeField]
        [Tooltip("Room name for John's Invite")]
        private const string JOHN_ROOMNAME = "JOHNR";
        [SerializeField]
        [Tooltip("Room name for Boya's Invite")]
        private const string BOYA_ROOMNAME = "BOYAR";
	    #endregion

	    #region Public Methods

        public void JoinJohn()
		{
            customRoomHandler.CustomRoom(JOHN_ROOMNAME);
            uiRoomNameInput.text = JOHN_ROOMNAME;
		}
        public void JoinBoya()
		{
            customRoomHandler.CustomRoom(BOYA_ROOMNAME);
            uiRoomNameInput.text = BOYA_ROOMNAME;
        }
	    #endregion
    }
}