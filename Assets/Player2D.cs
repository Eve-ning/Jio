using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIPProject
{
    /// <summary>
    /// This controls the player in 2D environments, specifically in the Landing.
    /// </summary>
    public class Player2D : MonoBehaviour
    {

		#region Variables

		public PhotonView photonView;
        public Text playerNameText;

        /// <summary>
        /// Not too sure if this is needed? The 2D scene is not yet multiplayer?
        /// I'll leave this here just in case we want players too see each other in lobby?
        /// </summary>
        public GameObject playerCamera;

        [Tooltip("Controls how fast the player is moving in 2D.")]
        [SerializeField]
        public float speed = 5;

        private Rigidbody2D rb;
        private Vector2 moveVelocity;

		#endregion

		#region MonoBehavior Callbacks
		private void Awake()
        {
            PhotonNetwork.NickName = PhotonNetwork.NickName == "" ? "Unknown Name" : PhotonNetwork.NickName;

            playerCamera.SetActive(photonView.IsMine);
            if (photonView.IsMine)
            {
                playerNameText.text = PhotonNetwork.NickName;
            }
            else
            {
                playerNameText.text = photonView.Owner.NickName == "" ? photonView.Owner.NickName : "Unexpected Missing Name";
            }

        }

        // Start is called before the first frame update
        void Start()
        {
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

	}
}