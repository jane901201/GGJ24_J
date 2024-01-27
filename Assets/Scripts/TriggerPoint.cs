using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPoint : MonoBehaviour
{
    public event Action<String> OnTrigger;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CheckPoint")
        {
            Debug.Log("Trigger");
            OnTrigger.Invoke(other.gameObject.name);
        }
    }
}
