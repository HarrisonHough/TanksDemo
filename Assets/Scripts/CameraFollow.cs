using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float yOffset;
    private Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget.transform;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            targetPosition = target.position;
            targetPosition.y = yOffset;
            transform.position = targetPosition;
        }
    }
}
