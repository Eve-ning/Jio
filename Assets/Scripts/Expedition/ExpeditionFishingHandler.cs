using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
		private TimeSpan totalTime;
		private TimeSpan timerTime;

        public bool isTimerRunning = false;

        [Tooltip("These Buttons will be disabled if the person is not the host.")]
        public Button[] nonHostButtonDisable;

		[Tooltip("The slider to adjust duration of the expedition")]
        public Slider totalTimeSlider;
        [Tooltip("The total timer on the bottom-right")]
        public TMP_Text totalTimeText;
        [Tooltip("The on-screen timer that pops up on start fishing")]
        public TMP_Text timerTimeText;

        [Tooltip("This is the formatting for the timer.")]
        [SerializeField]
        public const string TIMER_FORMAT = @"hh\:mm\:ss";

        [Tooltip("This is to adjust how fast time flows. This is mainly used for debugging.")]
        [SerializeField]
        public uint DEBUG_TIME_MULTIPLIER = 1;

        [Tooltip("This is used to trigger the start/end expedition.")]
        public Animator animator;

		#region Event Codes

		private const byte SyncTimerTimeEventCode = 1; 
        private const byte SyncTotalTimeEventCode = 2;
        
        private const byte SyncStartEventCode = 3;
        private const byte SyncEndEventCode = 4;

		public TimeSpan TotalTime 
        {
            get 
            {
                return totalTime;
            }
            set 
            {
                totalTime = value;
                totalTimeText.text = TotalTimeAsString();
                TimerTime = totalTime;
            }
        }
		public TimeSpan TimerTime 
        {
            get 
            {
                return timerTime;
            }
            set 
            {
                timerTime = value;
                timerTimeText.text = TimerTimeAsString();
            }
        }

		#endregion

		#endregion

		#region MonoBehaviorPunCallbacks Callbacks

		public override void OnPlayerLeftRoom(Player otherPlayer)
		{
            ValidateHostButtonsUI();
			base.OnPlayerLeftRoom(otherPlayer);
		}

		public override void OnJoinedRoom()
		{
            ValidateHostButtonsUI();
			base.OnJoinedRoom();
		}

		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
            SyncTimerTimeEvent();
            SyncTotalTimeEvent();
			base.OnPlayerEnteredRoom(newPlayer);
		}

		#endregion

		#region Expedition Timer Loop
        /// In the Expedition Timer Loop, the host is the main controller
        /// That means, we will NOT call coroutines on participants.
        /// This will ensure that the synchronization is only based on the host.
        
        /// <summary>
        /// This is a host-only function.
        /// This means that this code will not be executed on participants.
        /// </summary>
		public void StartExpeditionHost()
        {
            if (!isTimerRunning)
            {
                isTimerRunning = true;
                SyncStartEvent();
                StartCoroutine(LoopExpeditionHost());
            }
		}

        public void StartExpeditionChild()
        {
            Debug.Log("Expedition Timer has started at " + TotalTime);
            // Though called at Host, the child will need to affirm this running variable too
            isTimerRunning = true;

            // Just in case it's not synced.
            FreezePlayers();
            animator.SetTrigger("Start Expedition");
        }

        /// <summary>
        /// This loops through the expedition timer.
        /// The host will regularly update the participants on the Timer time via Syncing.
        /// </summary>
        /// <returns></returns>
		public IEnumerator LoopExpeditionHost()
        {
            // Waits for 1 second, if we have a DEBUG_TIME_MULTIPLIER, then it'll be faster.
            yield return new WaitForSeconds(1 / DEBUG_TIME_MULTIPLIER);
            timerTime -= TimeSpan.FromSeconds(1);

            // While the Timer time is still positive, we loop the Coroutine until it isn't.
            if (TimerTime.TotalSeconds >= 0)
            {
                // Sync with participants
                SyncTimerTimeEvent();

                // This simply calls itself (thus looping) if it's not done.
                StartCoroutine(LoopExpeditionHost());

            } else EndExpeditionHost();
        }

        public void LoopExpeditionChild(TimeSpan timerTime)
		{
            this.TimerTime = timerTime;
            if (TimerTime.Seconds % 10 == 0) Debug.Log("Expedition Timer at " + TimerTimeAsString());
        }


        /// <summary>
        /// This means that the host has detected the end of the expedition.
        /// The host will now tell all the participants.
        /// </summary>
        void EndExpeditionHost()
		{
            // Tells participants that the event has ended, will trigger unfreeze
            SyncEndEvent();
		}

        public void EndExpeditionChild()
		{
            Debug.Log("Expedition has ended! Total Time " + TotalTimeAsString());
            isTimerRunning = false;
            UnfreezePlayers();
            TimerTime = TotalTime;
            animator.SetTrigger("End Expedition");
        }

        #endregion

        #region Sync Events

        /// <summary>
        /// This helps sync the total time, this happens when the host changes the total time value using settings.
        /// </summary>
        private void SyncTotalTimeEvent()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(
                SyncTotalTimeEventCode, 
                totalTimeSlider.value, // Content
                raiseEventOptions, 
                SendOptions.SendReliable
                );
        }

        /// <summary>
        /// This helps sync everyone's timer.
        /// </summary>
        private void SyncTimerTimeEvent()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(
                SyncTimerTimeEventCode,
                TimerTime.TotalSeconds, // Content
                raiseEventOptions, 
                SendOptions.SendReliable
                );
        }
        
        /// <summary>
        /// This occurs when the host starts the timer
        /// </summary>
        private void SyncStartEvent()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(SyncStartEventCode, null, raiseEventOptions, SendOptions.SendReliable);
        }
        
        /// <summary>
        /// This occurs when the host has finished the timer
        /// </summary>
        private void SyncEndEvent()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(SyncEndEventCode, null, raiseEventOptions, SendOptions.SendReliable);
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

		private void Start()
        {
            TotalTime = TimeSpan.FromMinutes(totalTimeSlider.value);
            TimerTime = TotalTime;
        }

        #endregion

        #region Host UI Handler

        /// <summary>
        /// Disables host settings for those who are not host.
        /// Enables those for host.
        /// </summary>
        private void ValidateHostButtonsUI()
        {
            foreach (var obj in nonHostButtonDisable) obj.interactable = PhotonNetwork.IsMasterClient;
        }
		#endregion

		#region Sync Event Callbacks

		public void OnEvent(EventData photonEvent)
        {
			switch (photonEvent.Code)
			{
                case SyncStartEventCode:
                    StartExpeditionChild();
                    break;

                case SyncEndEventCode:
                    EndExpeditionChild();
                    break;

                case SyncTimerTimeEventCode:
                    LoopExpeditionChild(TimeSpan.FromSeconds((double)photonEvent.CustomData));
                    break;

                case SyncTotalTimeEventCode:
                    /// This will yield the slider value made by the host.
                    /// Which in turn will trigger appropriate updates of time
                    totalTimeSlider.value = (float)photonEvent.CustomData;
                    break;

                default:
					break;
			}
        }

		#endregion

		#region Formatting Methods

		/// <summary>
		/// Gets the Timer time in hh:mm:ss Format
		/// </summary>
		string TimerTimeAsString()
		{
            return TimerTime.ToString(TIMER_FORMAT);
		}

        /// <summary>
        /// Gets the Total time in hh:mm:ss Format
        /// </summary>
        string TotalTimeAsString()
		{
            return TotalTime.ToString(TIMER_FORMAT);
		}

        /// <summary>
        /// Updates the timer text on slider change
        /// </summary>
        public void UpdateTotalTimeText()
        {
            TotalTime = TimeSpan.FromMinutes(totalTimeSlider.value);
            SyncTotalTimeEvent();
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