using Plugins.Audio.Utils;
using UnityEngine;

public class GoodsSound : Audio
{
    [SerializeField] private Goods _goods;
    [SerializeField] private AudioDataProperty _goodsPickUpClip;

    private void OnEnable()
    {
        _goods.PickedUp += OnPickedUp;
    }

    private void OnDisable()
    {
        _goods.PickedUp -= OnPickedUp;
    }

    private void OnPickedUp(Goods goods)
    {
        SourceAudio.PlayOneShot(_goodsPickUpClip.Key);
    }
}
