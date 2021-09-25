using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIPProject
{
    public class ExpeditionFishingHandler : MonoBehaviour, IOnEventCallback
    {
		#region Variables
		/// <summary>
		/// Time are always in seconds.
		/// </summary>
		private TimeSpan fishingTotalTime;
		private TimeSpan fishingCurrentTime;

		[Tooltip("The slider to adjust duration of the expedition")]
        public Slider fishingDurationSlider;
        [Tooltip("The on-screen timer on the bottom right")]
        public Text fishingTimer;

        [Tooltip("This is the formatting for the timer.")]
        [SerializeField]
        public const string TIMER_FORMAT = @"hh\:mm\:ss";

        [Tooltip("This is to adjust how fast time flows. This is mainly used for debugging.")]
        [SerializeField]
        public uint DEBUG_TIME_MULTIPLIER = 1;

		#region Event Codes

		private const byte SyncFishingCurrentTimeEventCode = 1; 
        private const byte SyncFishingTotalTimeEventCode = 2;
        
        private const byte SyncFishingStartEventCode = 3;
        private const byte SyncFishingEndEventCode = 4;

		public TimeSpan FishingTotalTime { get => fishingTotalTime; set => fishingTotalTime = value; }
		public TimeSpan FishingCurrentTime { get => fishingCurrentTime; set => fishingCurrentTime = value; }

		#endregion


		#endregion


		#region Expedition Timer Loop

		public void StartExpedition()
		{
            FishingTotalTime = TimeSpan.FromMinutes(fishingDurationSlider.value);
            FishingCurrentTime = FishingTotalTime;
            Debug.Log("Expedition Timer has started at " + FishingTotalTime);
            FreezePlayers();
            if (PhotonNetwork.IsMasterClient) {
                SyncFishingStartEvent();
                StartCoroutine(ExpeditionTimer()); 
            }
		}
		public IEnumerator ExpeditionTimer()
        {
            // Waits for 1 second, if we have a DEBUG_TIME_MULTIPLIER, then it'll be faster.
            yield return new WaitForSeconds(1 / DEBUG_TIME_MULTIPLIER);
            FishingCurrentTime -= TimeSpan.FromSeconds(1);

            // While the current time is still positive, we loop the Coroutine until it isn't.
            if (FishingCurrentTime.TotalSeconds >= 0) {
                Debug.Log("Expedition Timer at " + CurrentTime());
                fishingTimer.text = CurrentTime();
                SyncFishingCurrentTimeEvent();
                StartCoroutine(ExpeditionTimer());
            } else EndExpedition();
        }
        void EndExpedition()
		{
            Debug.Log("Expedition has ended! Total Time " + FishingTotalTime);
            SyncFishingEndEvent();
            UnfreezePlayers();
            fishingTimer.text = TotalTime();
		}

		#endregion

		#region Sync Events

        /// <summary>
        /// This helps sync the total time, this happens when the host changes the total time value using settings.
        /// </summary>
		private void SyncFishingTotalTimeEvent()
        {
            double time = FishingTotalTime.TotalSeconds;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(SyncFishingCurrentTimeEventCode, time, raiseEventOptions, SendOptions.SendReliable);
        }

        /// <summary>
        /// This helps sync everyone's timer.
        /// </summary>
        private void SyncFishingCurrentTimeEvent()
        {
            double time = FishingCurrentTime.TotalSeconds;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(SyncFishingTotalTimeEventCode, time, raiseEventOptions, SendOptions.SendReliable);
        }
        
        /// <summary>
        /// This occurs when the host starts the timer
        /// </summary>
        private void SyncFishingStartEvent()
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(SyncFishingTotalTimeEventCode, null, raiseEventOptions, SendOptions.SendReliable);
        }
        
        /// <summary>
        /// This occurs when the host has finished the timer
        /// </summary>
        private void SyncFishingEndEvent()
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(SyncFishingTotalTimeEventCode, null, raiseEventOptions, SendOptions.SendReliable);
        }

		#endregion

		#region MonoBehavior Callbacks 

        /// <summary>
        /// This simply adds this instance as a Event Callback participant, so OnEvent will be triggered.
        /// </summary>
		private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        /// <summary>
        /// This removes the OnEvent Callback
        /// </summary>
        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

		#endregion

		#region Sync Event Callbacks

		public void OnEvent(EventData photonEvent)
        {

            Debug.Log(photonEvent);
            Debug.Log((double) photonEvent.CustomData);
			switch (photonEvent.Code)
			{
                case SyncFishingCurrentTimeEventCode:
                    FishingCurrentTime = TimeSpan.FromSeconds((double) photonEvent.CustomData);
                    break;
                case SyncFishingTotalTimeEventCode:
                    FishingTotalTime = TimeSpan.FromSeconds((double)photonEvent.CustomData);
                    break;
                case SyncFishingStartEventCode:
                    StartExpedition();
                    break;
                case SyncFishingEndEventCode:
                    FishingCurrentTime = TimeSpan.Zero;
                    EndExpedition();
                    break;
				default:
					break;
			}
        }

		#endregion

		#region Formatting Methods

		/// <summary>
		/// Gets the Current time in hh:mm:ss Format
		/// </summary>
		string CurrentTime()
		{
            return FishingCurrentTime.ToString(TIMER_FORMAT);
		}

        /// <summary>
        /// Gets the Total time in hh:mm:ss Format
        /// </summary>
        string TotalTime()
		{
            return FishingTotalTime.ToString(TIMER_FORMAT);
		}

        /// <summary>
        /// Updates the timer text on slider change
        /// </summary>
        public void UpdateTimerText()
        {
            fishingTimer.text = TimeSpan.FromMinutes((int)fishingDurationSlider.value).ToString(TIMER_FORMAT);
            SyncFishingTotalTimeEvent();
        }

        #endregion

        #region Player Freezing Methods

        public void FreezePlayers()
		{
            var players = GameObject.FindGameObjectsWithTag("Player");
            players[0].GetComponent<Rigidbody2D>().constraints |= RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }

        public void UnfreezePlayers()
		{
            var players = GameObject.FindGameObjectsWithTag("Player");
            players[0].GetComponent<Rigidbody2D>().constraints ^= RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }

		#endregion

	}
}