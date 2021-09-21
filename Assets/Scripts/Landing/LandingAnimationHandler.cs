using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This simply just closes the intro canvas once it's done animating
/// </summary>
namespace DIPProject
{
    public class LandingAnimationHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject landingIntro;
        [SerializeField]
        private RandomRoomHandler randomRoomHandler;
        [SerializeField]
        private CustomRoomHandler customRoomHandler;
        
        public void DeactivateIntro()
        {
            landingIntro.SetActive(false);
        }
        public void RandomRoom()
        {
            randomRoomHandler.MoveToRandomRoom();
        }
        public void CustomRoom()
        {
            customRoomHandler.MoveToCustomRoom();
        }
    }
}