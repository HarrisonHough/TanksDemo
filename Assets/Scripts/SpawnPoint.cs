using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {


    [SerializeField]
    private float detectionRadius = 1f;
    public LayerMask layerMask;

	// Use this for initialization
	void Start () {
		
	}

    public bool CheckIsOccupied()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);
        if (hitColliders.Length > 0)
        {
            return true;

        }

        return false;

    }

}
