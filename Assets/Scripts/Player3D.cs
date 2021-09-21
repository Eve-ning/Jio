using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace DIPProject
{
    /// <summary>
    /// This controls the Player in the 3D environments.
    /// For example, the:
    /// 1) Expedition
    /// 2) Fireside Chat
    /// </summary>
    public class Player3D : MonoBehaviourPunCallbacks
    {
        #region Variables

        /// <summary>
        /// This is the Empty parent of the player UI.
        /// This is to facilitate what to hide if other players instantiate in.
        /// </summary>
        [Tooltip("UI hidable of other players.")]
        public GameObject PlayerUI;

        [Tooltip("This is the player instance PhotonView")]
        public PhotonView photonView;

        [Tooltip("This controls how fast the player is moving")]
        [SerializeField]
        private float speed = 5;

        [Tooltip("Player floating name tag. This is NOT an overlay, it's attached locally.")]
        public TextMeshPro txtNameTag;

        [Tooltip("UI text, this is a global object.")]
        public Text txtRoomTag;

        private Rigidbody rb;
        private Vector3 moveVelocity;

        #endregion

        #region MonoBehavior Callbacks

        /// <summary>
        /// Start is called before the first frame update
        /// Note that for every player, including other players, Start() will still be called.
        /// 
        /// That means, you need to disable additional components brought forward by the additional
        /// initializations.
        /// 
        /// See below, where if the newly created player has a view not mine, it will suppress the 
        /// additional UI.
        /// </summary>
        void Start()
        {
            rb = GetComponent<Rigidbody>();

            if (!photonView.IsMine)
            {
                /// Stop other cameras being active.
                PlayerUI.SetActive(false);

                /// Set their TMP overhead name tag
                txtNameTag.text = photonView.Owner.NickName;
            }
            else
            {
                /// Set our name tag
                txtNameTag.text = photonView.Owner.NickName;

                /// Set room name on UI
                txtRoomTag.text = PhotonNetwork.CurrentRoom.Name;
            }
        }

        /// <summary>
        /// Update is called once per frame
        /// 
        /// For every frame, we check if the view is ours, then we process if we need to update 
        /// the movement.
        /// </summary>
        void Update()
        {
            // If it's our object
            if (photonView.IsMine)
            {
                // We do checks on these
                MovementInputCheck();
            }
        }

        /// <summary>
        /// We set the movement update to fixed for smoother movement (?)
        /// </summary>
        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Here, we check the inputs and set the moveVelocity accordingly.
        /// </summary>
        void MovementInputCheck()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 inputVector = new Vector3(h, 0, v);
            inputVector.Normalize();

            /// The Quaternion rotation is to account for the object being isometrically rotated.
            moveVelocity = Quaternion.AngleAxis(45, Vector3.up) * inputVector * speed;
        }

        #endregion

        #region MonoBehaviorPunCallbacks Callbacks

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.LogFormat("Player {0} has joined the room!", newPlayer.NickName);
            base.OnPlayerEnteredRoom(newPlayer);
        }

        #endregion
    }
}