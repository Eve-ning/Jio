using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIPProject
{
    public class ExpeditionAnimationHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject intro;

        [SerializeField]
        private ExpeditionHandler expeditionHandler;


        /// <summary>
        /// This simply just closes the intro canvas once it's done animating.
        /// Otherwise the canvas will be blocking the clickables.
        /// </summary>
        public void DeactivateIntro()
        {
            intro.SetActive(false);
        }

        public void Campfire()
        {
            expeditionHandler.Campfire();
        }
    }
}