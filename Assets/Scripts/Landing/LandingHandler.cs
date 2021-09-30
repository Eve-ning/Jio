using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIPProject
{
    /// <summary>
    /// The purpose of this class is to set the landing intro to be active.
    /// By default, in the editor, it's inactive because it helps the devs see the scene.
    /// </summary>
    public class LandingHandler : MonoBehaviour
    {
        public GameObject landingIntro;

        // Start is called before the first frame update
        void Start()
        {
            landingIntro.SetActive(true);
        }
    }
}