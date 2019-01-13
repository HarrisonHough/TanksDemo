using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class NetworkPoolMember : NetworkBehaviour
{
    //
    public event Action OnDestroyEvent;

    /// <summary>
    /// 
    /// </summary>
    private void OnDisable()
    {
        if (OnDestroyEvent != null)
            OnDestroyEvent();
    }
}
