using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

namespace DIPProject
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        #region Variables

        private static readonly int Up = Animator.StringToHash("Up");
        private static readonly int Right = Animator.StringToHash("Right");
        private static readonly int Left = Animator.StringToHash("Left");
        private static readonly int Down = Animator.StringToHash("Down");
        private static readonly int Stop = Animator.StringToHash("Stop");
        private static readonly int Active = Animator.StringToHash("Active");

        [SerializeField] private Animator animator;
        [SerializeField] private PhotonView photonView;

        private enum Movement
        {
            Stop = 0,
            Left = 1,
            Right = 2,
            Up = 3,
            Down = 4
        }

        #endregion
        #region MonoBehavior Callbacks

        // Update is called once per frame
        private void Start()
        {
            photonView = PhotonView.Get(this);
        }

        private void Update()
        {
            // On execute the input animation handler if the photon view is ours
            if (photonView.IsMine) InputToTrigger();
        }

        private void InputToTrigger()
        {
            // We will get the player's input and trigger the corresponding animation
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) 
                AnimationTrigger(Movement.Stop);
            else
            {
                if (Input.GetAxis("Horizontal") < 0)      AnimationTrigger(Movement.Left);
                else if (Input.GetAxis("Horizontal") > 0) AnimationTrigger(Movement.Right);
                if (Input.GetAxis("Vertical") < 0)        AnimationTrigger(Movement.Down);
                else if (Input.GetAxis("Vertical") > 0)   AnimationTrigger(Movement.Up);
            }
        }

        #endregion
        #region Animation Triggers

        private void AnimationTrigger(Movement movement)
        {
            // In addition to setting the animator triggers
            // We also send the RPC if we are in a room
            // Note that we shouldn't send RPCs in the lobby / camp
            if (PhotonNetwork.InRoom) photonView.RPC("RPCTrigger", RpcTarget.Others, (int) movement);
            
            // A Switch statement for the movement
            // Note that we explicitly reset some triggers just in case they get stuck
            switch (movement)
            {
                case Movement.Stop:
                    animator.SetTrigger(Stop);
                    animator.ResetTrigger(Left);
                    animator.ResetTrigger(Right);
                    animator.ResetTrigger(Down);
                    animator.ResetTrigger(Up);
                    animator.ResetTrigger(Active);
                    break;
                case Movement.Left:
                    animator.SetTrigger(Left);
                    animator.SetTrigger(Active);
                    animator.ResetTrigger(Stop);
                    break;
                case Movement.Right:
                    animator.SetTrigger(Right);
                    animator.SetTrigger(Active);
                    animator.ResetTrigger(Stop);
                    break;
                case Movement.Up:
                    animator.SetTrigger(Up);
                    animator.SetTrigger(Active);
                    animator.ResetTrigger(Stop);
                    break;
                case Movement.Down:
                    animator.SetTrigger(Down);
                    animator.SetTrigger(Active);
                    animator.ResetTrigger(Stop);
                    break;
            }
        }
        #endregion
        
        #region RPC Callbacks

        [PunRPC]
        [UsedImplicitly]
        void RPCTrigger(int movementId, PhotonMessageInfo info)
        {
            // If the photon view received is the same view as the one in the scene, then we proceed.
            if (photonView == info.photonView) AnimationTrigger((Movement) movementId);
        }

        #endregion
    }
}