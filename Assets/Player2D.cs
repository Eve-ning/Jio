using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DIPProject
{
    /// <summary>
    /// This controls the player in all 2D environments
    /// </summary>
    public class Player2D : MonoBehaviourPunCallbacks
        {

		#region Variables

		new public PhotonView photonView;
        public TextMeshPro playerNameText;
        public GameObject playerCamera;

		#region Movement
		[Tooltip("Controls how fast the player is moving in 2D.")]
        [SerializeField]
        public float speed = 5;
        private Rigidbody2D rb;
        private Vector2 moveVelocity;
		#endregion
		#region Chat
		[Tooltip("The Parent Chatbox GameObject")]
        [SerializeField]
        private GameObject chatBox;
        [Tooltip("The Chatbox TMP")]
        [SerializeField]
        private TextMeshPro chatText;
        [SerializeField]
        private float chatOpenTime = 2f;
        #endregion
        #region Defaults
        private string MISSING_SELF_NAME = "Missing Self Name";
        private string MISSING_OTHER_NAME = "Missing Other Name";
		#endregion

		#endregion


		#region MonoBehavior Callbacks
		private void Awake()
        {
            PhotonNetwork.NickName = PhotonNetwork.NickName == "" ? MISSING_SELF_NAME : PhotonNetwork.NickName;
            playerCamera.SetActive(photonView.IsMine);
            if (photonView.IsMine) playerNameText.text = PhotonNetwork.NickName;
            else playerNameText.text = photonView.Owner.NickName == "" ? MISSING_OTHER_NAME : photonView.Owner.NickName;
        }

		// Start is called before the first frame update
		void Start()
        {
            chatBox.SetActive(false);
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine)
            {
                Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                moveVelocity = moveInput * speed;
            }
        }

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }

		#endregion

		#region Chat Methods

		public IEnumerator ChatPopup(object msg)
        {
            chatBox.SetActive(true);
            chatText.text = msg.ToString();
            yield return new WaitForSeconds(chatOpenTime);
            chatBox.SetActive(false);
            chatText.text = "";
        }

		#endregion
	}
}