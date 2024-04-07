using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    private AudioSource _as;

    private int _latestPriority;

    private int _prevClip;

    private void Awake()
    {
        _as = GetComponent<AudioSource>();

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        StartCoroutine(ResetLock());
    }

    public void SetSound(AudioClip sound, bool repeat, int priority = 0)
    {
        if (priority < _latestPriority || _prevClip == sound.GetHashCode())
            return;

        _prevClip = sound.GetHashCode();

        if (priority > 1)
            StartCoroutine(PriorityUncheck(sound.length));

        _latestPriority = priority;

        _as.Stop();
        _as.clip = sound;
        _as.Play();

        _as.loop = repeat;
    }

    private IEnumerator ResetLock()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            _prevClip = 0;
        }
    }

    private IEnumerator PriorityUncheck(float sec)
    {
        yield return new WaitForSeconds(sec);
        _latestPriority = 0;
    }

    public void StopSound()
    {
        _as.Stop();
    }
}