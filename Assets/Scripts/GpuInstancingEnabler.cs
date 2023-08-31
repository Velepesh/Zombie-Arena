using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class GpuInstancingEnabler : MonoBehaviour
{
    private void Awake()
    {
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        SkinnedMeshRenderer meshRenderer = GetComponent<SkinnedMeshRenderer>();
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}