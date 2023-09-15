using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;

public class LocalizationSelector : MonoBehaviour
{
    private bool _active = false;

    public event UnityAction<int> LocalizationChanged;

    public void ChangeLocal(int localID)
    {
        //if (_active == true)
        //    return;

        //StartCoroutine(SetLocal(localID));
    }

    private IEnumerator SetLocal(int localID)
    {
        //_active = true;

        yield return LocalizationSettings.InitializationOperation;
        //LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localID];

        //LocalizationChanged?.Invoke(localID);
        //_active = false;
    }
}