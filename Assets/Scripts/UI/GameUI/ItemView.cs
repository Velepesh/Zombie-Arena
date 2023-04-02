﻿using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ItemView : MonoBehaviour
{
    [Tooltip("Color applied to all images.")]
    [SerializeField] protected Color ImageColor = Color.white;

    [Title(label: "Settings")]

    [Tooltip("Weapon Body Image.")]
    [SerializeField] protected Image ImageWeaponBody;

    [Tooltip("Weapon Grip Image.")]
    [SerializeField] protected Image ImageWeaponGrip;

    [Tooltip("Weapon Laser Image.")]
    [SerializeField] protected Image ImageWeaponLaser;

    [Tooltip("Weapon Silencer Image.")]
    [SerializeField] protected Image ImageWeaponMuzzle;

    [Tooltip("Weapon Magazine Image.")]
    [SerializeField] protected Image ImageWeaponMagazine;

    [Tooltip("Weapon Scope Image.")]
    [SerializeField] protected Image ImageWeaponScope;

    [Tooltip("Weapon Scope Default Image.")]
    [SerializeField] protected Image ImageWeaponScopeDefault;

    [SerializeField] protected TMP_Text Label;

    private WeaponAttachmentManagerBehaviour _attachmentManagerBehaviour;

    protected void SetImages(Weapon weapon)
    {
        Color toAssign = ImageColor;
        foreach (Image image in GetComponents<Image>())
            image.color = toAssign;

        _attachmentManagerBehaviour = weapon.GetAttachmentManager();
        ImageWeaponBody.sprite = weapon.GetSpriteBody();

        Sprite sprite = default;

        ScopeBehaviour scopeDefaultBehaviour = _attachmentManagerBehaviour.GetEquippedScopeDefault();

        if (scopeDefaultBehaviour != null)
            sprite = scopeDefaultBehaviour.GetSprite();

        AssignSprite(ImageWeaponScopeDefault, sprite, scopeDefaultBehaviour == null);

        ScopeBehaviour scopeBehaviour = _attachmentManagerBehaviour.GetEquippedScope();

        if (scopeBehaviour != null)
            sprite = scopeBehaviour.GetSprite();

        AssignSprite(ImageWeaponScope, sprite, scopeBehaviour == null || scopeBehaviour == scopeDefaultBehaviour);


        MagazineBehaviour magazineBehaviour = _attachmentManagerBehaviour.GetEquippedMagazine();

        if (magazineBehaviour != null)
            sprite = magazineBehaviour.GetSprite();

        AssignSprite(ImageWeaponMagazine, sprite, magazineBehaviour == null);

        LaserBehaviour laserBehaviour = _attachmentManagerBehaviour.GetEquippedLaser();

        if (laserBehaviour != null)
            sprite = laserBehaviour.GetSprite();

        AssignSprite(ImageWeaponLaser, sprite, laserBehaviour == null);

        GripBehaviour gripBehaviour = _attachmentManagerBehaviour.GetEquippedGrip();

        if (gripBehaviour != null)
            sprite = gripBehaviour.GetSprite();

        AssignSprite(ImageWeaponGrip, sprite, gripBehaviour == null);

        MuzzleBehaviour muzzleBehaviour = _attachmentManagerBehaviour.GetEquippedMuzzle();

        if (muzzleBehaviour != null)
            sprite = muzzleBehaviour.GetSprite();

        AssignSprite(ImageWeaponMuzzle, sprite, muzzleBehaviour == null);
    }

    protected void SetLabel(Weapon weapon)
    {
        Label.text = weapon.Lable.ToString();
    }

    private void AssignSprite(Image image, Sprite sprite, bool forceHide = false)
    {
        image.sprite = sprite;
        image.enabled = sprite != null && !forceHide;
    }
}