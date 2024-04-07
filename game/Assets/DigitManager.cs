using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitManager : MonoBehaviour
{
    public Sprite[] numbers;

    private SpriteRenderer _tens;
    private SpriteRenderer _ones;

    private void Awake()
    {
        _tens = transform.Find("1").GetComponent<SpriteRenderer>();
        _ones = transform.Find("0").GetComponent<SpriteRenderer>();
    }

    public void RecreateDigits(int number)
    {
        var numStr = number.ToString();
        _tens.sprite = number >= 10 ? numbers[int.Parse($"{numStr[0]}")] : numbers[0];
        _ones.sprite = number >= 10 ? numbers[int.Parse($"{numStr[1]}")] : numbers[int.Parse($"{numStr[0]}")];
    }
}