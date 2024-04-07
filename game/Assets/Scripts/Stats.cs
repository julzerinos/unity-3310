using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour
{

    public static Stats Instance;

    public int PlayerHealth { get; set; } = 10;

    public string Realm => $"{_realm} - {_level}";

    
    private int _level = 1;
    private string _realm = "1";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public bool SaveStats(int h)
    {
        if (++_level == 7)
        {
            _realm = "b";
            _level = 1;

            SceneManager.LoadScene("PreBoss");
            return true;
        }

        PlayerHealth = h;
        return false;
    }
}