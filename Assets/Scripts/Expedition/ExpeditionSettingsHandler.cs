using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIPProject
{
    public class ExpeditionSettingsHandler : MonoBehaviour
    {

        public float duration;
        public Text durationText;
        public Slider durationSlider;
        private const string DURATION_FORMAT = @"hh\:mm\:ss";

        public void UpdateDurationText()
		{
            durationText.text = TimeSpan.FromMinutes((int) durationSlider.value).ToString(DURATION_FORMAT);
		}


        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}