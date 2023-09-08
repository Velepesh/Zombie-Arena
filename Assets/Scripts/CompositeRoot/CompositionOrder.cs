using System.Collections.Generic;
using UnityEngine;

public class CompositionOrder : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private List<CompositeRoot> _order;


    private void OnEnable()
    {
        _game.GameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _game.GameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        Compose();
    }


    private void Compose()
    {
        foreach (var compositionRoot in _order)
        {
            compositionRoot.Compose();
            compositionRoot.enabled = true;
        }
    }
}