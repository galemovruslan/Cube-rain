using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Timer))]
public class RainSpawner : MonoBehaviour
{
    [SerializeField] private Particle _prefab;
    [SerializeField] private BoxCollider _spawnVolume;
    [SerializeField] int _spawnSpeed = 5;

    private Timer _timer;
    private ObjectPool<Particle> _pool;

    private float _spawnTime => 1f / _spawnSpeed;

    private void Awake()
    {
        _timer = GetComponent<Timer>(); 
        _pool = new ObjectPool<Particle>(
            createFunc: CreateFunction,
            actionOnGet: ActionOnGet,
            actionOnRelease: ActionOnRelease);
    }

    private void OnEnable()
    {
        _timer.Finished += Spawn;
    }

    private void OnDisable()
    {
        _timer.Finished -= Spawn;
    }

    private void Start()
    {
        _timer.Set(_spawnTime);
    }

    private void Spawn()
    {
        _pool.Get();
        _timer.Set(_spawnTime);
    }

    private void ActionOnRelease(Particle particle)
    {
        particle.gameObject.SetActive(false);
    }

    private void ActionOnGet(Particle particle)
    {
        particle.SetPosition(GetPosition());
        particle.ResetState();
        particle.gameObject.SetActive(true);
    }

    private Particle CreateFunction()
    {
        Particle particle = Instantiate(_prefab);
        particle.Initialize(_pool.Release);
        return particle;
    }

    private Vector3 GetPosition()
    {
        float xPosition = _spawnVolume.transform.position.x + (UnityEngine.Random.value * 2 - 1) * _spawnVolume.size.x / 2;
        float yPosition = _spawnVolume.transform.position.y + (UnityEngine.Random.value * 2 - 1) * _spawnVolume.size.y / 2;
        float zPosition = _spawnVolume.transform.position.z + (UnityEngine.Random.value * 2 - 1) * _spawnVolume.size.z / 2;

        return new Vector3(xPosition, yPosition, zPosition);
    }
}
