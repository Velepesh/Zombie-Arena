using System;
using System.Collections.Generic;
using UnityEngine;

public class CompositionOrder : MonoBehaviour
{
    [SerializeField] private List<CompositeRoot> _order;

    public void Compose()
    {
        foreach (var compositionRoot in _order)
        {
            compositionRoot.Compose();
            compositionRoot.enabled = true;
        }
    }
}