using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject pickup;
    public Vector3[] spots;

    private GameObject[] _pickups;

    private readonly WaitForSeconds _pickupTimer = new WaitForSeconds(15f);

    private bool _shouldSpawn = true;

    private void Awake()
    {
        _pickups = new GameObject[4];

        for (var i = 0; i < 4; i++)
        {
            _pickups[i] = Instantiate(pickup, spots[i], Quaternion.identity);
            _pickups[i].SetActive(false);
        }
    }

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (_shouldSpawn)
        {
            for (var i = 0; i < 4; i++)
            {
                if (_pickups[i].activeSelf)
                    continue;

                _pickups[i].SetActive(true);
                break;
            }

            yield return _pickupTimer;
        }
    }
}