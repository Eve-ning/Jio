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
    public class ExpeditionFishingHandler : MonoBehaviourPunCallbacks, IOnEventCallback
    {
		#region Variables
		/// <summary>
		/// Time are always in seconds.
		/// </summary>
		private TimeSpan fishingTotalTime;
		private TimeSpan fishingCurrentTime;

        [Tooltip("These Buttons will be disabled if the person is not the host.")]
        public Button[] nonMasterButtonDisable;

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

		public TimeSpan FishingTotalTime { get; set; }
		public TimeSpan FishingCurrentTime { get; set; }
		#endregion

		#endregion


		#region MonoBehaviorPunCallbacks Callbacks

		public override void OnPlayerLeftRoom(Player otherPlayer)
		{
            ValidateMasterButtonsUI();
			base.OnPlayerLeftRoom(otherPlayer);
		}

		public override void OnJoinedRoom()
		{
            ValidateMasterButtonsUI();
			base.OnJoinedRoom();
		}

		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
            SyncFishingCurrentTimeEvent();
            SyncFishingTotalTimeEvent();
			base.OnPlayerEnteredRoom(newPlayer);
		}

		#endregion

        /// In the Expedition Timer Loop, the host is the main controller
        /// That means, we will NOT call coroutines on participants.
        /// This will ensure that the synchronization is only based on the host.
		#region Expedition Timer Loop

        /// <summary>
        /// This is a host-only function.
        /// This means that this code will not be executed on participants.
        /// </summary>
		public void StartExpedition()
		{
            FishingTotalTime = TimeSpan.FromMinutes(fishingDurationSlider.value);
            FishingCurrentTime = FishingTotalTime;
            Debug.Log("Expedition Timer has started at " + FishingTotalTime);
            FreezePlayers();
            SyncFishingStartEvent();
            StartCoroutine(ExpeditionTimer()); 
		}

        /// <summary>
        /// This loops through the expedition timer.
        /// The host will regularly update the participants on the current time via Syncing.
        /// </summary>
        /// <returns></returns>
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

                // This simply calls itself if it's not done.
                StartCoroutine(ExpeditionTimer());
            } else EndExpedition();
        }

        /// <summary>
        /// This means that the host has detected the end of the expedition.
        /// The host will now tell all the participants.
        /// </summary>
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
            if (!PhotonNetwork.IsMasterClient) return;
            var sliderValue = fishingDurationSlider.value;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(SyncFishingTotalTimeEventCode, sliderValue, raiseEventOptions, SendOptions.SendReliable);
        }

        /// <summary>
        /// This helps sync everyone's timer.
        /// </summary>
        private void SyncFishingCurrentTimeEvent()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            double time = FishingCurrentTime.TotalSeconds;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(SyncFishingCurrentTimeEventCode, time, raiseEventOptions, SendOptions.SendReliable);
        }
        
        /// <summary>
        /// This occurs when the host starts the timer
        /// </summary>
        private void SyncFishingStartEvent()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(SyncFishingStartEventCode, null, raiseEventOptions, SendOptions.SendReliable);
        }
        
        /// <summary>
        /// This occurs when the host has finished the timer
        /// </summary>
        private void SyncFishingEndEvent()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(SyncFishingEndEventCode, null, raiseEventOptions, SendOptions.SendReliable);
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

		#region MonoBehavior Callbacks

		private void Start()
        {
            FishingTotalTime = TimeSpan.FromMinutes(fishingDurationSlider.value);
            FishingCurrentTime = FishingTotalTime;
        }

        #endregion

        #region Master UI Handler

        /// <summary>
        /// Disables host settings for those who are not host.
        /// Enables those for host.
        /// </summary>
        private void ValidateMasterButtonsUI()
        {
            foreach (var obj in nonMasterButtonDisable) obj.interactable = PhotonNetwork.IsMasterClient;
        }
		#endregion

		#region Sync Event Callbacks

		public void OnEvent(EventData photonEvent)
        {
			switch (photonEvent.Code)
			{
                case SyncFishingCurrentTimeEventCode:
                    FishingCurrentTime = TimeSpan.FromSeconds((double) photonEvent.CustomData);
                    fishingTimer.text = CurrentTime();
                    break;

                case SyncFishingTotalTimeEventCode:
                    /// This will yield the slider value made by the host.
                    /// Which in turn will trigger appropriate updates of time
                    fishingDurationSlider.value = (float) photonEvent.CustomData;
                    break;

                case SyncFishingStartEventCode:
                    FreezePlayers();
                    break;

                case SyncFishingEndEventCode:
                    FishingCurrentTime = FishingTotalTime;
                    fishingTimer.text = CurrentTime();

                    UnfreezePlayers();
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
            FishingTotalTime = TimeSpan.FromMinutes(fishingDurationSlider.value);
            FishingCurrentTime = FishingTotalTime;
            Debug.Log("updating");
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