using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;

	public void SpawnPlayer()
	{
        float randomValue = Random.Range(-1f, 1f);

        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(this.transform.position.x + randomValue, this.transform.position.y), Quaternion.identity, 0);
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
