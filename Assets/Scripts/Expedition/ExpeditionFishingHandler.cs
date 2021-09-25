using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIPProject
{
    public class ExpeditionFishingHandler : MonoBehaviour
    {
        /// <summary>
        /// Time are always in seconds.
        /// </summary>
        public TimeSpan fishingTotalTime;
        public TimeSpan fishingCurrentTime;

        public Slider fishingDurationSlider;
        public Text fishingTimer;
        
        public const string TIMER_FORMAT = @"hh\:mm\:ss";

        [Tooltip("This is to adjust how fast time flows. This is mainly used for debugging.")]
        [SerializeField]
        public uint DEBUG_TIME_MULTIPLIER = 1;

		public void StartExpedition()
		{
            fishingTotalTime = TimeSpan.FromMinutes(fishingDurationSlider.value);
            fishingCurrentTime = fishingTotalTime;
            Debug.Log("Expedition Timer has started at " + fishingTotalTime);
            FreezePlayers();
            StartCoroutine(ExpeditionTimer());
		}
		public IEnumerator ExpeditionTimer()
        {
            // Waits for 1 second, if we have a DEBUG_TIME_MULTIPLIER, then it'll be faster.
            yield return new WaitForSeconds(1 / DEBUG_TIME_MULTIPLIER);
            fishingCurrentTime -= TimeSpan.FromSeconds(1);

            // While the current time is still positive, we loop the Coroutine until it isn't.
            if (fishingCurrentTime.TotalSeconds >= 0) {
                Debug.Log("Expedition Timer at " + CurrentTime());
                fishingTimer.text = CurrentTime();
                StartCoroutine(ExpeditionTimer());
            } else EndExpedition();
        }
        void EndExpedition()
		{
            Debug.Log("Expedition has ended! Total Time " + fishingTotalTime);
            UnfreezePlayers();
            fishingTimer.text = TotalTime();
		}

        string CurrentTime()
		{
            return fishingCurrentTime.ToString(TIMER_FORMAT);
		}

        string TotalTime()
		{
            return fishingTotalTime.ToString(TIMER_FORMAT);
		}

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

    }
}