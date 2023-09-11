using UnityEngine;
using UnityEngine.Localization.Components;

public class LocalizeStringEventrEFRESHER : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent _stringEvent;

    private void Awake()
    {
        _stringEvent.enabled = false;
    }

    private void Start()
    {
        _stringEvent.enabled = true;
    }
}
