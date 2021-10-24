using System;
using System.Collections;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

namespace DIPProject
{
    public partial class ExpeditionFishingHandler : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        #region Variables

        [Tooltip("The global light that will tune the day night cycle")]
        [SerializeField]
        private Light2D globalLight2D;

        [Tooltip("The background for the Day Night Cycle")] [SerializeField]
        private Image background;

        private const float backgroundStartX = 0f;
        private const float backgroundEndX = -4000f;

        private const float globalLightStart = 1f;
        private const float globalLightEnd = 0.25f;

        private int _dayNightCycleFrames = 50;
        private float _dayNightCycleDelay = 0.01f;

        #endregion

        private void UpdateDayNightCycle()
        {
            float progress = 1 - (float) (TimerTime.TotalSeconds / TotalTime.TotalSeconds);
            Debug.LogWarning(progress);
            var position = background.rectTransform.anchoredPosition;
            position = new Vector2(progress * (backgroundEndX - backgroundStartX) + backgroundStartX, position.y);
            background.rectTransform.anchoredPosition = position;
            globalLight2D.intensity = globalLightStart - (globalLightStart - globalLightEnd) * progress;
        }

        private IEnumerator ResetDayNightCycle()
        {
            for (var i = 0; i < _dayNightCycleFrames; i++)
            {
                float progress = (float) Math.Sqrt((float) i / _dayNightCycleFrames);
                var position = background.rectTransform.anchoredPosition;
                position = new Vector2((1 - progress) * (backgroundEndX - backgroundStartX) , position.y);
                background.rectTransform.anchoredPosition = position;
                globalLight2D.intensity = globalLightEnd + (globalLightStart - globalLightEnd) * progress;
                yield return new WaitForSeconds(_dayNightCycleDelay);
            }
        }




    }
}