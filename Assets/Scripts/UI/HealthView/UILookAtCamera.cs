using System;
using UnityEngine;

public class UILookAtCamera : MonoCache
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable() => AddUpdate();
   
    private void OnDisable() => RemoveUpdate();
   
    public override void OnTick()
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