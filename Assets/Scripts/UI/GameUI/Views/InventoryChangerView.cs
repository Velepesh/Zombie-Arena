using InfimaGames.LowPolyShooterPack;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryChangerView : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private int _countOfInventoryViews;

    private InventoryView[] _inventoryViews = new InventoryView[] { };

    private void OnValidate()
    {
        _countOfInventoryViews = Math.Clamp(_countOfInventoryViews, 0, int.MaxValue);
    }

    private void Awake()
    {
        _inventoryViews = GetComponentsInChildren<InventoryView>();

        if (_inventoryViews.Length > _countOfInventoryViews)
            throw new ArgumentException(nameof(_inventoryViews));
    }

    private void OnEnable()
    {
        _inventory.Setted += OnSetted;
    }

    private void OnDisable()
    {
        _inventory.Setted -= OnSetted;
    }

    private void OnSetted(List<Weapon> weapons)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            for (int j = 0; j < _inventoryViews.Length; j++)
            {
                if (weapons[i].Type == _inventoryViews[j].Type)
                {
                    _inventoryViews[j].UpdateView(weapons[i], j);
                    break;
                }

                if(j == _inventoryViews.Length - 1)
                    throw new ArgumentNullException($"There is no view for {weapons[i].Type}");
            }
        }
    }
}