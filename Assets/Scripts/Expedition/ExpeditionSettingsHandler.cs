using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIPProject
{
    public class ExpeditionSettingsHandler : MonoBehaviour
    {
        [Tooltip("The slider to adjust duration of the expedition")]
        public Slider fishingDurationSlider;
        [Tooltip("The on-screen timer on the bottom right")]
        public Text fishingTimer;

        public void UpdateDurationText()
		{
            fishingTimer.text = TimeSpan.FromMinutes((int) fishingDurationSlider.value).ToString(ExpeditionFishingHandler.TIMER_FORMAT);
		}

    }
}