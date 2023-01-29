using System.Collections;
using UnityEngine;

public class JumpAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve _yAnimation;
    [SerializeField] private float _length;
    [SerializeField] private float _duration;

    public void Jump(Vector3 direction)
    {
        Vector3 target = transform.position + (direction * _length);

        StartCoroutine(JumpByTime(target));
    }

    private IEnumerator JumpByTime(Vector3 target)
    {
        float expiredSeconds = 0;
        float progress = 0;

        Vector3 startPosition = transform.position;

        while (progress < 1)
        {
            expiredSeconds += Time.deltaTime;
            progress = expiredSeconds / _duration;

            transform.position = Vector3.Lerp(startPosition, target, progress) + new Vector3(0f, _yAnimation.Evaluate(progress), 0f);

            yield return null;
        }
    }
}
