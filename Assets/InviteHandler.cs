using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InviteHandler : MonoBehaviour
{
    public GameObject uiInviteScreen;

    public void ShowScreen()
    {
        uiInviteScreen.SetActive(true);
    }

    public void HideScreen()
    {
        uiInviteScreen.SetActive(false);
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
