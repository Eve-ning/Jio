using Photon.Pun;
using UnityEngine;

namespace DIPProject
{
    /// <summary>
    /// Handles the splash screen interactions
    /// </summary>
    public class SplashAnimatorHandler : MonoBehaviourPunCallbacks
    {
        #region Variables

        [Tooltip("The splash handler for the Splash Scene")]
        public SplashHandler splashHandler;

		#endregion

		#region Public Methods

		public void JoinLobby()
        {
            splashHandler.JoinLobby();
        }

		#endregion
	}
}