using System;
using UnityEngine;
using UnityEngine.UI;
using YG.Utils.Pay;
using InfimaGames.LowPolyShooterPack;
#if YG_TEXT_MESH_PRO
using TMPro;
#endif

namespace YG
{
    [HelpURL("https://www.notion.so/PluginYG-d457b23eee604b7aa6076116aab647ed#10e7dfffefdc42ec93b39be0c78e77cb")]
    public class PurchaseYG : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        [Serializable]
        public struct TextLegasy
        {
            public Text title, description, priceValue;
        }
        public TextLegasy textLegasy;

#if YG_TEXT_MESH_PRO
        [Serializable]
        public struct TextMP
        {
            public TextMeshProUGUI title, description, priceValue;
        }
        public TextMP textMP;
#endif

        public ImageLoadYG imageLoad;

        [Tooltip("Добавить Ян/Yan к строке цены")]
        public bool addYAN_toPrice = true;

        public Purchase data = new Purchase();
        public Weapon Weapon => _weapon;
        [ContextMenu(nameof(UpdateEntries))]
        public void UpdateEntries()
        {
            if (textLegasy.title) textLegasy.title.text = data.title;
            if (textLegasy.description) textLegasy.description.text = data.description;
            if (textLegasy.priceValue)
            {
                textLegasy.priceValue.text = data.priceValue;
                if (addYAN_toPrice) textLegasy.priceValue.text += Yan();
            }

#if YG_TEXT_MESH_PRO
            if (textMP.title) textMP.title.text = data.title;
            if (textMP.description) textMP.description.text = data.description;
            if (textMP.priceValue)
            {
                textMP.priceValue.text = data.priceValue;
                if (addYAN_toPrice) textMP.priceValue.text += Yan();
            }
#endif
            if (imageLoad) imageLoad.Load(data.imageURI);
        }

        public void BuyPurchase()
        {
            YandexGame.BuyPayments(data.id);
        }

        private string Yan()
        {
            if (YandexGame.savesData.language == "ru")
                return " Ян";
            else
                return " Yan";
        }
    }
}
