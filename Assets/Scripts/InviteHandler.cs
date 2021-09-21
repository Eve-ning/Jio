using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIPProject { 
    public class InviteHandler : MonoBehaviour
    {
	    #region Variables
	    public GameObject uiInviteScreen;
	    #endregion

	    #region Public Methods
	    public void ShowScreen()
        {
            uiInviteScreen.SetActive(true);
        }

        public void HideScreen()
        {
            uiInviteScreen.SetActive(false);
        }
	    #endregion
    }
}