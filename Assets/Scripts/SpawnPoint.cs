using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {


    [SerializeField]
    private float _detectionRadius = 1f;
    public LayerMask LayerMask;

	// Use this for initialization
	void Start () {
		
	}

    public bool CheckIsOccupied()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _detectionRadius, LayerMask);
        if (hitColliders.Length > 0)
        {
            return true;

        }

        return false;

    }

}
