using System.Collections;
using UnityEngine;

public class ZombieDestroyer : State
{
    [SerializeField] private float _undergroundSpeed;
    [SerializeField] private float _time;
    [SerializeField] private float _delayBeforMoving;
    [SerializeField] private ParticleSystem _dieEffect;

    private bool _isMoving = false;

    private void OnValidate()
    {
        _undergroundSpeed = Mathf.Clamp(_undergroundSpeed, 0f, float.MaxValue);
    }

    private void Start()
    {
        StartCoroutine(DelayBeforeMove());
    }

    private void Update()
    {
        if (_isMoving == false)
            return;

        if (_time > 0)
        {
            _time -= Time.deltaTime;
            Move();
        }
        else
        {
            DestroyZombie();
        }
    }

    private IEnumerator DelayBeforeMove()
    {
        float delayBeforEffect = _delayBeforMoving / 2;

        yield return new WaitForSeconds(delayBeforEffect);
        ShowEffect();

        yield return new WaitForSeconds(_delayBeforMoving);

        _isMoving = true;
    }

    private void Move()
    {
        transform.Translate(Vector3.down * _undergroundSpeed * Time.deltaTime);
    }

    private void ShowEffect()
    {
        Instantiate(_dieEffect.gameObject, transform.position, Quaternion.identity);
    }

    private void DestroyZombie()//ObjectPool???
    {
        Destroy(gameObject);
    }
}
