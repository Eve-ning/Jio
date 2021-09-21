using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DIPProject
{
    /// <summary>
    /// The purpose of this handler is to handle events AFTER the animation has been completed.
    /// Take for example, CustomRoom()
    ///     Once the player has pressed Enter or Clicked Join
    ///     The OnClick of the button will initiate the animation
    ///     Moving to Random room SHOULD only happen after the animation has been completed
    ///     Thus, the animation completion will then initiate this call.
    ///     
    /// These calls are backdoor calls to RandomRoom & CustomRoom as they will skip the animation.
    /// Thus it is not recommended to use the TriggerTo___ functions directly.
    /// </summary>
    public class LandingAnimationHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject landingIntro;
        [SerializeField]
        private RandomRoomHandler randomRoomHandler;
        [SerializeField]
        private CustomRoomHandler customRoomHandler;
        /// <summary>
        /// This simply just closes the intro canvas once it's done animating
        /// </summary>
        public void DeactivateIntro()
        {
            landingIntro.SetActive(false);
        }
        /// <summary>
        /// This will call the rando
        /// </summary>
        public void RandomRoom()
        {
            randomRoomHandler.TriggerToRandomRoom();
        }
        public void CustomRoom()
        {
            customRoomHandler.TriggerToCustomRoom();
        }
    }
}