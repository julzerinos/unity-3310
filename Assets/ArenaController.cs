using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ArenaController : MonoBehaviour
{
    public AudioClip sound;

    public GameObject bug;

    private bool _started;

    private TriggerEvents _boundary;
    private TriggerEvents _portal;

    private DigitManager _dm;

    private int _timeLeft = 60;

    private readonly bool[] _stages = {false, false, false};

    private GameObject _laser;
    private GameObject _boltSpawner;

    private bool _playerInside;

    private readonly WaitForSeconds _fightTimer = new WaitForSeconds(1.0f);
    private readonly WaitForSeconds _laserTimer = new WaitForSeconds(7f);

    private void Awake()
    {
        _boundary = transform.Find("Boundary zone").GetComponent<TriggerEvents>();
        _portal = transform.Find("Portal").GetComponent<TriggerEvents>();

        _dm = GetComponentInChildren<DigitManager>();

        _laser = transform.Find("Laser").gameObject;
        _boltSpawner = transform.Find("BoltSpawner").gameObject;
    }

    private void Start()
    {
        _portal.ObjectEntered += StartBossFight;
        _boundary.ObjectEntered += EnteredZone;
        _boundary.ObjectLeft += LeftZone;

        _boundary.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _portal.ObjectEntered -= StartBossFight;
        _boundary.ObjectEntered -= EnteredZone;
        _boundary.ObjectLeft -= LeftZone;
    }

    private void StartBossFight()
    {
        AudioController.Instance.SetSound(sound, false);

        _portal.gameObject.SetActive(false);

        _boundary.gameObject.SetActive(true);

        StartCoroutine(FightLoop());
    }

    private IEnumerator FightLoop()
    {
        _started = true;
        while (_started)
        {
            ManageStages();

            _timeLeft = Mathf.Clamp(_timeLeft + (_playerInside ? -1 : 1), 0, 60);
            _dm.RecreateDigits(_timeLeft);

            if (_timeLeft == 0)
                End();

            yield return _fightTimer;
        }
    }

    private void EnteredZone()
    {
        _playerInside = true;
    }

    private void LeftZone()
    {
        _playerInside = false;
    }

    private void ManageStages()
    {
        if (_timeLeft < 60 && !_stages[0])
            StageOne();

        if (_timeLeft < 40 && !_stages[1])
            StageTwo();

        if (_timeLeft < 20 && !_stages[2])
            StageThree();
    }

    private void StageOne()
    {
        _stages[0] = true;
        _boltSpawner.SetActive(true);
    }

    private void StageTwo()
    {
        _stages[1] = true;

        for (var i = 0; i <= 5; i++)
        {
            var t = Instantiate(bug, transform).transform;
            t.position += .5f * (Vector3) Random.insideUnitCircle;
        }
    }

    private void StageThree()
    {
        _stages[2] = true;

        StartCoroutine(StageThreeLoop());
    }

    private IEnumerator StageThreeLoop()
    {
        while (_stages[2])
        {
            _laser.SetActive(true);
            yield return _laserTimer;
        }
    }

    private void End()
    {
        SceneManager.LoadScene("End");
    }
}