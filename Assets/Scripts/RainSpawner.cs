using UnityEngine;
using UnityEngine.Pool;

public class RainSpawner : MonoBehaviour
{
    [SerializeField] private Particle _prefab;
    [SerializeField] private BoxCollider _spawnVolume;
    [SerializeField] int _spawnSpeed = 5;

    private ObjectPool<Particle> _pool;
    private float _spawnOffset = 0f;

    private void Awake()
    {
        _pool = new ObjectPool<Particle>(
            createFunc: OnSpawn,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease);
    }

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), _spawnOffset, 1.0f / _spawnSpeed);
    }

    private void Spawn()
    {
        _pool.Get();
    }

    private void OnRelease(Particle particle)
    {
        particle.gameObject.SetActive(false);
    }

    private void OnGet(Particle particle)
    {
        particle.SetPosition(GetPosition());
        particle.ResetState();
        particle.gameObject.SetActive(true);
    }

    private Particle OnSpawn()
    {
        Particle particle = Instantiate(_prefab);
        particle.Initialize(_pool);
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
