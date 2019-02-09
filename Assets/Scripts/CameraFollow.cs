using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _yOffset;
    private Vector3 _targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetTarget(GameObject newTarget)
    {
        _target = newTarget.transform;
    }

    private void LateUpdate()
    {
        if (_target != null)
        {
            _targetPosition = _target.position;
            _targetPosition.y = _yOffset;
            transform.position = _targetPosition;
        }
    }
}
