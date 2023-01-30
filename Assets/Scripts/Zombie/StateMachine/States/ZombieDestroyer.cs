using System.Collections;
using UnityEngine;

public class ZombieDestroyer : State
{
    [SerializeField] private float _undergroundSpeed;
    [SerializeField] private float _time;
    [SerializeField] private float _delayBeforMoving;

    private bool _isMovingDown = false;

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
        if (_isMovingDown == false)
            return;

        if (_time > 0)
            MoveDown();
        else
            DestroyZombie();
    }

    private IEnumerator DelayBeforeMove()
    {
        yield return new WaitForSeconds(_delayBeforMoving);

        _isMovingDown = true;
    }

    private void MoveDown()
    {
        _time -= Time.deltaTime;

        transform.Translate(Vector3.down * _undergroundSpeed * Time.deltaTime);
    }


    private void DestroyZombie()//ObjectPool???
    {
        Destroy(gameObject);
    }
}