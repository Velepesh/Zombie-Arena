using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

[RequireComponent(typeof(Image))]
public class SpriteFromAtlas : MonoBehaviour
{
    [SerializeField] private SpriteAtlas _atlas; 

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        SetSprite();
    }

    private void SetSprite()
    {
        Sprite sprite = _atlas.GetSprite(_image.sprite.name);

        if (sprite == null)
            throw new ArgumentNullException(nameof(sprite));

        _image.sprite = sprite;
    }
}