using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Laser : MonoBehaviour
{
    public AudioClip sound;

    private Collider2D _cl;
    private Animator _am;
    private AnimationEvents _ae;

    private static readonly int LaserTrigger = Animator.StringToHash("Laser");

    private bool _rotated;

    private void Awake()
    {
        _cl = GetComponent<Collider2D>();
        _am = GetComponentInChildren<Animator>();

        _ae = GetComponentInChildren<AnimationEvents>();

        _ae.EventID += LaserStriking;
        _ae.Ended += LaserFinished;
    }

    private void OnDestroy()
    {
        _ae.EventID -= LaserStriking;
        _ae.Ended -= LaserFinished;
    }

    private void LaserStriking(int _)
    {
        AudioController.Instance.SetSound(sound, false, 5);
        _cl.enabled = true;
    }

    private void LaserFinished()
    {
        _cl.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _am.Rebind();
        transform.position += new Vector3(Random.Range(-.4f, .4f), 0, 0);

        if (_rotated)
        {
            transform.Rotate(Vector3.forward, 90);
            return;
        }

        if (Random.value > .5f) return;

        _rotated = true;
        transform.Rotate(Vector3.forward, 90);
    }
}