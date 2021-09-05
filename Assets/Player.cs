using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PhotonView photonView;
    public Text playerNameText;
    public GameObject playerCamera;


    public float speed = 5;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;


    private void Awake()
    {
        PhotonNetwork.NickName = PhotonNetwork.NickName == "" ? "Unknown Name" : PhotonNetwork.NickName;

        playerCamera.SetActive(photonView.IsMine);
        if (photonView.IsMine)
        {
            playerNameText.text = PhotonNetwork.NickName;
        }
        else
        {
            playerNameText.text = photonView.Owner.NickName == "" ? photonView.Owner.NickName : "Unexpected Missing Name";
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            moveVelocity = moveInput * speed;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

}
