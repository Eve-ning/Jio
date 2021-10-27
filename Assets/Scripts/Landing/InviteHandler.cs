using UnityEngine;
using UnityEngine.UI;

namespace DIPProject
{
    public class InviteHandler : MonoBehaviour
    {
        #region Variables

        public CustomRoomHandler customRoomHandler;
        public InputField uiRoomNameInput;

        [SerializeField] [Tooltip("Room name for John's Invite")]
        private const string JOHN_ROOMNAME = "JOHNR";

        [SerializeField] [Tooltip("Room name for Boya's Invite")]
        private const string BOYA_ROOMNAME = "BOYAR";

        [SerializeField] [Tooltip("Room name for June's Invite")]
        private const string JUNE_ROOMNAME = "JUNER";

        [SerializeField] [Tooltip("Room name for Fu Hao's Invite")]
        private const string FUHAO_ROOMNAME = "FUHAO";

        #endregion

        #region Public Methods

        private void JoinRoom(string roomName)
        {
            customRoomHandler.CustomRoom(roomName);
            uiRoomNameInput.text = roomName;
        }
        
        public void JoinJohn() { JoinRoom(JOHN_ROOMNAME); }
        public void JoinBoya() { JoinRoom(BOYA_ROOMNAME); }
        public void JoinJune() { JoinRoom(JUNE_ROOMNAME); }
        public void JoinFuhao() { JoinRoom(FUHAO_ROOMNAME); }

        #endregion
    }
}