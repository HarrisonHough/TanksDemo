using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSpawnPoint : MonoBehaviour
{
    public bool IsOccupied { get; set; }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            IsOccupied = true;
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            IsOccupied = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            IsOccupied = false;
        }
    }

}
