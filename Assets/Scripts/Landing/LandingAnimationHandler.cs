using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        private CreateRoomHandler createRoomHandler;
        [SerializeField]
        private CustomRoomHandler customRoomHandler;
        [SerializeField]
        private InputField uiRoomFieldInput;

        /// <summary>
        /// This simply just closes the intro canvas once it's done animating.
        /// Otherwise the canvas will be blocking the clickables.
        /// </summary>
        public void DeactivateIntro()
        {
            landingIntro.SetActive(false);
        }
        public void FocusRoomFieldInput()
		{
            EventSystem.current.SetSelectedGameObject(uiRoomFieldInput.gameObject, null);
        }

        public void CreateRoom()
        {
            createRoomHandler.CreateRoomAfterAnimation();
        }
        public void CustomRoom()
        {
            customRoomHandler.CustomRoomAfterAnimation();
        }
    }
}