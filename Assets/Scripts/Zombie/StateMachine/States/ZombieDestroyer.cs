using System.Collections;
using UnityEngine;

public class ZombieDestroyer : State
{
    [SerializeField] private float _undergroundSpeed;
    [SerializeField] private float _time;
    [SerializeField] private float _delayBeforMoving;

    private Zombie _zombie;
    private bool _isMovingDown;

    private void OnValidate()
    {
        _undergroundSpeed = Mathf.Clamp(_undergroundSpeed, 0f, float.MaxValue);
    }

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
    }

    private void OnEnable()
    {
        _isMovingDown = false;
        AddUpdate();
        StartCoroutine(DelayBeforeMove());
    }

    private void OnDisable() => RemoveUpdate();

    public override void OnTick()
    {
        if (_isMovingDown == false)
            return;

        if (_time > 0)
            MoveDown();
        else
            DestroyZombie();

        _time -= Time.deltaTime;
    }

    private IEnumerator DelayBeforeMove()
    {
        yield return new WaitForSeconds(_delayBeforMoving);

        _isMovingDown = true;
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _undergroundSpeed * Time.deltaTime);
    }

    private void DestroyZombie()
    {
        _zombie.DisableZombie();
    }
}