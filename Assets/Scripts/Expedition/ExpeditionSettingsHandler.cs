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
        private const string durationPostfix = " Minutes";

        public void UpdateDurationText()
		{
            durationText.text = ((int) durationSlider.value).ToString() + durationPostfix;
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