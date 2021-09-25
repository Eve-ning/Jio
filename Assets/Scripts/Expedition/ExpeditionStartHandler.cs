using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DIPProject
{
    public class ExpeditionStartHandler : MonoBehaviour
    {
        /// <summary>
        /// Time are always in seconds.
        /// </summary>
        public int roomTotalTime = 0;
        public int roomCurrentTime = 0;

        [Tooltip("This is to adjust how fast time flows. This is mainly used for debugging.")]
        private const int TIME_MULTIPLIER = 1;

		public void StartExpedition()
		{
            Debug.Log("Expedition Timer has started at " + roomTotalTime);
		}
		public IEnumerator ExpeditionTimer()
        {
            yield return new WaitForSeconds(TIME_MULTIPLIER);
            roomCurrentTime -= TIME_MULTIPLIER;
            if (roomCurrentTime >= 0) {
                Debug.Log("Expedition Timer at " + roomCurrentTime);
                StartCoroutine(ExpeditionTimer());
            } else
			{
                EndExpedition();
			}
        }
        void EndExpedition()
		{
            Debug.Log("Expedition has ended! Total Time " + roomTotalTime);
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