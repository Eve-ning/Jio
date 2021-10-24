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

        private const string MissingSelfName = "Missing Self Name";
        private const string MissingOtherName = "Missing Other Name";

        #endregion

        #endregion

        #region Animation Variables

        [Tooltip("0.01f Frames of the flipping animation")]
        private const int AnimationFrames = 10;

        [Tooltip("0.01f Frames of the flipping animation")]
        private const float AnimationFrameDelay = 0.01f;

        #endregion


        #region MonoBehavior Callbacks

        private void Awake()
        {
            PhotonNetwork.NickName = PhotonNetwork.NickName == "" ? MissingSelfName : PhotonNetwork.NickName;
            playerCamera.SetActive(photonView.IsMine);
            if (photonView.IsMine) playerNameText.text = PhotonNetwork.NickName;
            else playerNameText.text = photonView.Owner.NickName == "" ? MissingOtherName : photonView.Owner.NickName;
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
            }
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + _moveVelocity * Time.fixedDeltaTime);
        }

        #endregion
    }
}