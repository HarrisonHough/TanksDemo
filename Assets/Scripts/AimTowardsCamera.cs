using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTowardsCamera : MonoBehaviour {

    [SerializeField]
    private Camera _camera;

	// Use this for initialization
	void Start () {
        if (_camera == null)
            _camera = Camera.main;
        transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
    }
	
// 	Update is called once per frame
// 		void Update () {
// 	        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
// 		}
}
