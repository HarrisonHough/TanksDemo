using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerControllerBasic : NetworkBehaviour
{

    public Rigidbody rigidbody;


    public float moveSpeed = 100f;

    bool canMove = false;

    Vector3 moveVector = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        moveVector = inputVector * moveSpeed * Time.deltaTime;
        //transform.position += moveVector;
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        rigidbody.velocity = moveVector;
    }


}
