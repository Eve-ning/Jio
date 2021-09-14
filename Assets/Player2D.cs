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

        #region Animation Variables
        [Tooltip("The GameObj Sprite Image used for transform scaling")]
        [SerializeField]
        private GameObject spriteImage;
        [Tooltip("The front side of the image")]
        [SerializeField]
        private Sprite frontImage;
        [Tooltip("The back side of the image")]
        [SerializeField]
        private Sprite backImage;

        [Tooltip("0.01f Frames of the flipping animation")]
        private const int ANIMATION_FRAMES = 10;
        [Tooltip("0.01f Frames of the flipping animation")]
        private const float ANIMATION_FRAME_DELAY = 0.01f;

        [Tooltip("A flag on if the sprite is now facing left. This is used to avoid repeated calls on held keys.")]
        private bool facingLeft = false;
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
            SetSpriteDirection();
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

		#region Scripted Animations

        /// <summary>
        /// This simply finds the input and sets the direction of the sprite.
        /// </summary>
        public void SetSpriteDirection()
		{
            if (Input.GetAxis("Horizontal") < 0 && !facingLeft)
            {
                // We force the animation to the end before calling another coroutine
                spriteImage.transform.localScale = new Vector3(1, 1, 1);
                StartCoroutine(FlipChar(true));
                facingLeft = true;
            }
            else if (Input.GetAxis("Horizontal") > 0 && facingLeft)
            {
                // We force the animation to the end before calling another coroutine
                spriteImage.transform.localScale = new Vector3(-1, 1, 1);
                StartCoroutine(FlipChar(false));
                facingLeft = false;
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                spriteImage.GetComponent<Image>().sprite = frontImage;
            }
            else if (Input.GetAxis("Vertical") > 0)
            {
                spriteImage.GetComponent<Image>().sprite = backImage;
            }
        }

        /// <summary>
        /// The asynchronous coroutine called when flipping
        /// </summary>
        /// <param name="reverse"></param>
        /// <returns></returns>
		public IEnumerator FlipChar(bool reverse)
        {
            for (int i = -ANIMATION_FRAMES; i < ANIMATION_FRAMES; i++)
            {
                spriteImage.transform.localScale = new Vector3((reverse ? -1 : 1) * (float) i / ANIMATION_FRAMES, 1, 1);
                yield return new WaitForSeconds(ANIMATION_FRAME_DELAY);
            }
            
        }

        #endregion
    }
}