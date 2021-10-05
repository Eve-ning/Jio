using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace DIPProject
{
    /// <summary>
    ///     This controls the player in all 2D environments
    /// </summary>
    public class Player2D : MonoBehaviourPunCallbacks
    {
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

        #region Variables

        public new PhotonView photonView;
        public TextMeshPro playerNameText;
        public GameObject playerCamera;

        #region Movement

        [Tooltip("Controls how fast the player is moving in 2D.")] [SerializeField]
        public float speed = 5;

        private Rigidbody2D _rb;
        private Vector2 _moveVelocity;

        #endregion

        #region Chat

        [Tooltip("The Parent Chatbox GameObject")] [SerializeField]
        private GameObject chatBox;

        [Tooltip("The Chatbox TMP")] [SerializeField]
        private TextMeshPro chatText;

        [SerializeField] private float chatOpenTime = 2f;

        #endregion

        #region Defaults

        private readonly string _missingSelfName = "Missing Self Name";
        private readonly string _missingOtherName = "Missing Other Name";

        #endregion

        #endregion

        #region Animation Variables

        [Tooltip("The GameObj Sprite Image used for transform scaling")] [SerializeField]
        private GameObject spriteImage;

        [Tooltip("The front side of the image")] [SerializeField]
        private Sprite frontImage;

        [Tooltip("The back side of the image")] [SerializeField]
        private Sprite backImage;

        [Tooltip("0.01f Frames of the flipping animation")]
        private const int AnimationFrames = 10;

        [Tooltip("0.01f Frames of the flipping animation")]
        private const float AnimationFrameDelay = 0.01f;

        [Tooltip("A flag on if the sprite is now facing left. This is used to avoid repeated calls on held keys.")]
        private bool _facingLeft;

        #endregion


        #region MonoBehavior Callbacks

        private void Awake()
        {
            PhotonNetwork.NickName = PhotonNetwork.NickName == "" ? _missingSelfName : PhotonNetwork.NickName;
            playerCamera.SetActive(photonView.IsMine);
            if (photonView.IsMine) playerNameText.text = PhotonNetwork.NickName;
            else playerNameText.text = photonView.Owner.NickName == "" ? _missingOtherName : photonView.Owner.NickName;
        }

        // Start is called before the first frame update
        private void Start()
        {
            chatBox.SetActive(false);
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (photonView.IsMine)
            {
                var moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                _moveVelocity = moveInput * speed;
                SetSpriteDirection();
            }
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + _moveVelocity * Time.fixedDeltaTime);
        }

        #endregion

        #region Scripted Animations

        /// <summary>
        ///     This simply finds the input and sets the direction of the sprite.
        /// </summary>
        public void SetSpriteDirection()
        {
            if (Input.GetAxis("Horizontal") < 0 && !_facingLeft)
            {
                // We force the animation to the end before calling another coroutine
                spriteImage.transform.localScale = new Vector3(1, 1, 1);
                StartCoroutine(FlipChar(true));
                _facingLeft = true;
            }
            else if (Input.GetAxis("Horizontal") > 0 && _facingLeft)
            {
                // We force the animation to the end before calling another coroutine
                spriteImage.transform.localScale = new Vector3(-1, 1, 1);
                StartCoroutine(FlipChar(false));
                _facingLeft = false;
            }

            if (Input.GetAxis("Vertical") < 0)
                spriteImage.GetComponent<SpriteRenderer>().sprite = frontImage;
            else if (Input.GetAxis("Vertical") > 0) spriteImage.GetComponent<SpriteRenderer>().sprite = backImage;
        }

        /// <summary>
        ///     The asynchronous coroutine called when flipping
        /// </summary>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public IEnumerator FlipChar(bool reverse)
        {
            for (var i = -AnimationFrames; i <= AnimationFrames; i++)
            {
                spriteImage.transform.localScale = new Vector3((reverse ? -1 : 1) * (float) i / AnimationFrames, 1, 1);
                yield return new WaitForSeconds(AnimationFrameDelay);
            }
        }

        #endregion
    }
}