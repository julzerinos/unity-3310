using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvents : MonoBehaviour
{
    public event Action ObjectEntered;
    public event Action ObjectLeft;

    public string objectTag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(objectTag))
            ObjectEntered?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(objectTag))
            ObjectLeft?.Invoke();
    }
}