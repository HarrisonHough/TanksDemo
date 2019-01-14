using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTowardsCamera : MonoBehaviour {

    public Camera camera;

	// Use this for initialization
	void Start () {
        if (camera == null)
            camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
	}
}
