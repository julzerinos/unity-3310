using System;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public event Action Ended;
    public event Action<int> EventID;

    public void OnEnd()
    {
        Ended?.Invoke();
    }

    public void OnEventID(int i)
    {
        EventID?.Invoke(i);
    }
}