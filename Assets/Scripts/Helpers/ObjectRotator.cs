using UnityEngine;
using DG.Tweening;

public static class ObjectRotator
{
    public static void RotateInfinity(Transform transform, Vector3 rotationAxis, float rotateDuration)
    {
        transform.DOLocalRotate(rotationAxis, rotateDuration, RotateMode.LocalAxisAdd)
            .SetLoops(-1)
            .SetEase(Ease.Linear)
            .Play();
    }
}