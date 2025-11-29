using System;
using System.Collections;
using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _onTouchColor;
    [SerializeField] private float _minLifeTime = 2f;
    [SerializeField] private float _maxLifeTime = 5f;

    private Action<Particle> _action;
    private MeshRenderer _renderer;
    private bool _wasTouched = false;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_wasTouched) return;

        if (collision.gameObject.TryGetComponent(out Platform _))
        {
            Touch();
        }
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void Initialize(Action<Particle> action)
    {
        _action = action;
    }

    public void ResetState()
    {
        _renderer.material.color = _startColor;
        _wasTouched = false;
    }

    private void Touch()
    {
        _renderer.material.color = _onTouchColor;
        _wasTouched = true;

        StartCoroutine(WaitDestroy());
    }

    private IEnumerator WaitDestroy()
    {
        float seconds = UnityEngine.Random.Range(_minLifeTime, _maxLifeTime);
        yield return new WaitForSeconds(seconds);
        _action?.Invoke(this);
    }
}
