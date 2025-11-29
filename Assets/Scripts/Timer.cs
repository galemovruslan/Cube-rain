using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private bool _isRunning = false;
    private float _elapsedTime = 0;

    public event Action Finished;

    public void Set(float time)
    {
        _elapsedTime = time;
        _isRunning = true;
    }

    private void Update()
    {
        if (_isRunning == false)
        {
            return;
        }

        _elapsedTime -= Time.deltaTime;
        
        if (_elapsedTime <= 0)
        {
            _isRunning = false;
            Finished?.Invoke();
        }
    }
}
