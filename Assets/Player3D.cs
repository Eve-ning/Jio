using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class Player3D : MonoBehaviourPunCallbacks
{
	#region Variables

	public GameObject PlayerUI;
    public PhotonView photonView;
    public float speed = 5;

    public TextMeshPro txtNameTag;
    public Text txtRoomTag;

    private Rigidbody rb;
    private Vector3 moveVelocity;

	#endregion

	#region MonoBehavior Callbacks

	// Start is called before the first frame update
	void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (!photonView.IsMine)
		{
            /// Stop other cameras being active.
            PlayerUI.SetActive(false);
            txtNameTag.text = photonView.Owner.NickName;
        }

        if (photonView.IsMine)
        {
            txtNameTag.text = photonView.Owner.NickName;
            txtRoomTag.text = PhotonNetwork.CurrentRoom.Name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 inputVector = new Vector3(h, 0, v);
            inputVector.Normalize();

            moveVelocity = Quaternion.AngleAxis(45, Vector3.up) * inputVector * speed;
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

	#endregion

	#region MonoBehaviorPunCallbacks Callbacks

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
        base.OnPlayerEnteredRoom(newPlayer);
	}


	public override void OnJoinedRoom()
	{



        base.OnJoinedRoom();
	}

	#endregion
}
