using System;
using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_camera == null)
            throw new NullReferenceException(nameof(_camera));

        LookAtCamera();
    }

    private void LookAtCamera()
    {
        transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward,
                _camera.transform.rotation * Vector3.up);
    }
}