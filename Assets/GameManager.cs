using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

	public override void OnJoinedRoom()
	{
        PhotonNetwork.Instantiate("Player", new Vector3(0, 1, 0), Quaternion.identity);
        base.OnJoinedRoom();
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
