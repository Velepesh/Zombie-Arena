using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TMP_Text _walletText;

    public void SetWalletValue(int value)
    {
        _walletText.text = value.ToString();
    }
}