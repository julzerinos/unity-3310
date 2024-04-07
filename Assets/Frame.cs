using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class Frame : MonoBehaviour
{
    private Animator _am;
    private static readonly int Flash = Animator.StringToHash("Flash");

    private int _lastLoad;

    private void Awake()
    {
        _am = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        PlayerController.LoadChanged += HealthChanged;
    }

    private void OnDestroy()
    {
        PlayerController.LoadChanged -= HealthChanged;
    }

    private void HealthChanged(int load)
    {
        if (load >= _lastLoad)
        {
            _lastLoad = load;
            return;
        }

        _lastLoad = load;
        _am.SetTrigger(Flash);
    }
}