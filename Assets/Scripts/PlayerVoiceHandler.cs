using System;
using Photon.Voice.Unity;
using UnityEngine;

namespace DIPProject
{
    public class PlayerVoiceHandler : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        [Tooltip("The recorder/mic input of the player.")]
        private Recorder recorder;

        [SerializeField]
        [Tooltip("The icon that appears when the person is speaking. Shift + M to unmute & mute.")]
        private SpriteRenderer speakerIcon;
        
        [SerializeField]
        [Tooltip("The icon that appears when Debug Echo is enabled. Shift + D to enable & disable debug.")]
        private SpriteRenderer debugIcon;

        [SerializeField] [Tooltip("Speaker Color Value when enabled")]
        private float speakerEnabledValue = 0.4f;

        [SerializeField] [Tooltip("Speaker Max Color Value when active")]
        private float speakerMaxActiveValue = 1f;

        [Tooltip("Visual Amp Level of the Speaker. This does not amplify the volume in any way. " +
                 "Only the value of the speaker color.")]
        private float _visualLevelAmp = 15f;
        
        [SerializeField] [Tooltip("Current Level of the Speaker")]
        private float volumeLevel;
        
        #endregion

        #region MonoBehaviour Callbacks

        // Start is called before the first frame update
        void Start()
        {
            recorder.TransmitEnabled = false;
            speakerIcon.enabled = false;
            debugIcon.enabled = false;

            recorder.DebugEchoMode = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M) && 
                (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                // Flip the state
                recorder.TransmitEnabled = !recorder.TransmitEnabled;
                speakerIcon.enabled = recorder.TransmitEnabled;
                speakerIcon.color = Color.HSVToRGB(0,0, speakerEnabledValue);
            }
            
            if (Input.GetKeyDown(KeyCode.D) && 
                (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                // Flip the state
                // This will enable debugEchoMode.
                recorder.DebugEchoMode = !recorder.DebugEchoMode;
                debugIcon.enabled = recorder.DebugEchoMode;
            }

            // This just scales the Amp from [0,1] to [min, max]
            volumeLevel = recorder.LevelMeter.CurrentAvgAmp * _visualLevelAmp * 
                          (speakerMaxActiveValue - speakerEnabledValue) + speakerEnabledValue;
            speakerIcon.color = Color.HSVToRGB(0,0, Math.Min(volumeLevel, speakerMaxActiveValue));
        }
        #endregion
        
    }
}
