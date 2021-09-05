using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CreateRoomHandler : MonoBehaviour
{
    public GameObject uiCanvas;
    public GameObject uiCreateButton;
    public InputField uiRoomNameInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private bool isMineColliding(Collider2D collider)
	{
        return collider.gameObject.GetComponent<PhotonView>().IsMine;
	}


	private void OnTriggerEnter2D(Collider2D collider)
	{
        if (isMineColliding(collider))
        {
            showCanvas();
        }
    }

	private void OnTriggerExit2D(Collider2D collider)
	{
        if (isMineColliding(collider))
        {
            hideCanvas();
        }
    }

	public void showCanvas()
	{
        uiCanvas.SetActive(true);
	}

    public void hideCanvas()
	{
        uiCanvas.SetActive(false);
	}
    
    public void createRoom()
	{
        Debug.Log(PhotonNetwork.NickName + " is Creating Room " + uiRoomNameInput.text);
        return;
	}
}
