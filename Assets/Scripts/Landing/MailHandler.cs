using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailHandler : MonoBehaviour
{
    public GameObject closedMail;
    public GameObject closedAlert;
    public GameObject openedMail;

    public int count=0;

    void OnMouseOver()
    {
        if (closedAlert.activeSelf)
        {
            closedAlert.SetActive(false);
        }
        else
        {
            closedMail.SetActive(false);
        }
        openedMail.SetActive(true);
    }

    void OnMouseDown()
    {
        count=1;
    }

    void OnMouseExit()
    {
        if (count==0)
        {
            openedMail.SetActive(false);
            closedAlert.SetActive(true);
        }
        else
        {
            openedMail.SetActive(false);
            closedMail.SetActive(true);
        }
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
