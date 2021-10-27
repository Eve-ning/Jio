using System;
using JetBrains.Annotations;
using Photon.Pun;
using Photon.Voice.Unity;
using UnityEngine;

namespace DIPProject
{
    public class PlayerVoiceHandler : MonoBehaviour
    {
        #region Variables

        [SerializeField] [Tooltip("The recorder/mic input of the player.")]
        private Recorder recorder;

        [SerializeField] [Tooltip("The icon that appears when the person is speaking. Shift + M to unmute & mute.")]
        private SpriteRenderer speakerIcon;

        private bool _isSpeaking;
        private bool _isDebugging;

        /// <summary>
        /// We write a prop so that it automatically sets the required
        /// settings on bool change
        /// </summary>
        public bool IsSpeaking
        {
            get => _isSpeaking;
            set
            {
                _isSpeaking = value;
                recorder.TransmitEnabled = _isSpeaking;
                speakerIcon.enabled = _isSpeaking;
            }
        }

        /// <summary>
        /// We write a prop so that it automatically sets the required
        /// settings on bool change
        /// </summary>
        public bool IsDebugging
        {
            get => _isDebugging;
            set
            {
                _isDebugging = value;
                recorder.DebugEchoMode = _isDebugging;
                debugIcon.enabled = _isDebugging;
            }
        }

        [SerializeField]
        [Tooltip("The icon that appears when Debug Echo is enabled. Shift + D to enable & disable debug.")]
        private SpriteRenderer debugIcon;

        [SerializeField] [Tooltip("Speaker Color Value when enabled")]
        private float speakerEnabledValue = 0.4f;

        [SerializeField] [Tooltip("Speaker Max Color Value when active")]
        private float speakerMaxActiveValue = 1f;

        [Tooltip("Visual Amp Level of the Speaker. This does not amplify the volume in any way. " +
                 "Only the value of the speaker color.")]
        private const float VisualLevelAmp = 15f;

        [SerializeField] [Tooltip("Current Level of the Speaker")]
        private float volumeLevel;

        /// <summary>
        /// This is required for RPC sending.
        /// </summary>
        private PhotonView _photonView;

        #endregion

        #region MonoBehaviour Callbacks

        // Start is called before the first frame update
        void Start()
        {
            // We can dynamically get the PhotonView here
            _photonView = PhotonView.Get(this);
            
            // As an init, the debug and speaking is off.
            IsDebugging = false;
            IsSpeaking = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!_photonView.IsMine) return;
            InputToVoiceState();
            // This just scales the Amp from [0,1] to [min, max]
            volumeLevel = recorder.LevelMeter.CurrentAvgAmp * VisualLevelAmp *
                (speakerMaxActiveValue - speakerEnabledValue) + speakerEnabledValue;
            speakerIcon.color = Color.HSVToRGB(0, 0, Math.Min(volumeLevel, speakerMaxActiveValue));
        }

        #endregion

        #region Voice State Handler

        /// <summary>
        /// This grabs the Inputs and Sets the Current state.
        /// Then it transmits this state to other players
        /// </summary>
        private void InputToVoiceState()
        {
            if (Input.GetKeyDown(KeyCode.M) &&
                (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                IsSpeaking = !IsSpeaking;
                SendRPCVoiceState();
            }
            if (Input.GetKeyDown(KeyCode.D) &&
                (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                IsDebugging = !IsDebugging;
                SendRPCVoiceState();
            }
        }

        /// <summary>
        /// Updates the Client Speaker and Debug Icon
        /// </summary>
        /// <param name="isSpeaking">Set the GO's Speaking Enable</param>
        /// <param name="isDebugging">Set the GO's Debugging Enable</param>
        private void VoiceStateIconUpdate(bool isSpeaking, bool isDebugging)
        {
            IsSpeaking = isSpeaking;
            IsDebugging = isDebugging;
        }

        /// <summary>
        /// Sends the RPC to others
        /// </summary>
        void SendRPCVoiceState()
        {
            _photonView.RPC("RPCVoiceState", RpcTarget.Others, IsSpeaking, IsDebugging);
        }

        #endregion
        
        #region RPC Callbacks
        /// <summary>
        /// Receives the RPC from sending clients.
        /// This will modify the other clients' myself instance 
        /// </summary>
        /// <param name="isSpeaking">Set the myself instance Speaking</param>
        /// <param name="isDebugging">Set the myself instance Debugging</param>
        /// <param name="info">Info for photonView matching</param>
        [PunRPC]
        [UsedImplicitly]
        void RPCVoiceState(bool isSpeaking, bool isDebugging, PhotonMessageInfo info)
        {
            if (_photonView == info.photonView) VoiceStateIconUpdate(isSpeaking, isDebugging);
        }
        #endregion
        
    }
}
