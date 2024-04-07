using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour
{
    public Sprite[] signalSprites;

    private SpriteRenderer _sr;
    private Transform _player;
    private Transform _portal;

    private float _initialDistance;

    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _portal = GameObject.FindWithTag("Portal").transform;
        _initialDistance = GetDistance();
    }

    private void LateUpdate()
    {
        var ratio = GetDistance() / _initialDistance;

        if (ratio > .75f)
            _sr.sprite = signalSprites[0];
        else if (ratio > .5f)
            _sr.sprite = signalSprites[1];
        else if (ratio > .25f)
            _sr.sprite = signalSprites[2];
        else
            _sr.sprite = signalSprites[3];
    }

    private float GetDistance() => (_player.position - _portal.position).magnitude;
}