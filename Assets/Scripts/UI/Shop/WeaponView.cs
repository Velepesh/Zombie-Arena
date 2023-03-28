using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Unity.VisualScripting;

public class WeaponView : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private WeaponButton _button;

    [Title(label: "Colors")]

    [Tooltip("Color applied to all images.")]
    [SerializeField]
    private Color _imageColor = Color.white;

    [Title(label: "Settings")]

    [Tooltip("Weapon Body Image.")]
    [SerializeField]
    private Image _imageWeaponBody;

    [Tooltip("Weapon Grip Image.")]
    [SerializeField]
    private Image _imageWeaponGrip;

    [Tooltip("Weapon Laser Image.")]
    [SerializeField]
    private Image _imageWeaponLaser;

    [Tooltip("Weapon Silencer Image.")]
    [SerializeField]
    private Image _imageWeaponMuzzle;

    [Tooltip("Weapon Magazine Image.")]
    [SerializeField]
    private Image _imageWeaponMagazine;

    [Tooltip("Weapon Scope Image.")]
    [SerializeField]
    private Image _imageWeaponScope;

    [Tooltip("Weapon Scope Default Image.")]
    [SerializeField]
    private Image _imageWeaponScopeDefault;

    [SerializeField] private TMP_Text _label;

    private WeaponAttachmentManagerBehaviour _attachmentManagerBehaviour;

    public Weapon Weapon => _weapon;

    public event UnityAction<Weapon, WeaponView> Clicked;

    private void Start()
    {
        UpdateView();
    }

    private void OnEnable()
    {
        _weapon.Inited += OnInited;
        _button.Clicked += OnClicked;
    }

    private void OnDisable()
    {
        _weapon.Inited -= OnInited;
        _button.Clicked -= OnClicked;
    }

    public void SelectView()
    {
        _button.SetSelectedColor();
    }

    public void UpdateView()
    {
        if (_weapon.IsEquip)
            _button.SetChoosedColor();
        else
            _button.SetDefaultColor();
    }

    private void OnClicked()
    {
        Clicked?.Invoke(_weapon, this);
    }

    private void OnInited()
    {
        Color toAssign = _imageColor;
        foreach (Image image in GetComponents<Image>())
            image.color = toAssign;

        //Get Attachment Manager.
        _attachmentManagerBehaviour = _weapon.GetAttachmentManager();
        //Update the weapon's body sprite!
        _imageWeaponBody.sprite = _weapon.GetSpriteBody();

        //Sprite.
        Sprite sprite = default;

        //Scope Default.
        ScopeBehaviour scopeDefaultBehaviour = _attachmentManagerBehaviour.GetEquippedScopeDefault();
        //Get Sprite.
        if (scopeDefaultBehaviour != null)
            sprite = scopeDefaultBehaviour.GetSprite();
        //Assign Sprite!
        AssignSprite(_imageWeaponScopeDefault, sprite, scopeDefaultBehaviour == null);

        //Scope.
        ScopeBehaviour scopeBehaviour = _attachmentManagerBehaviour.GetEquippedScope();
        //Get Sprite.
        if (scopeBehaviour != null)
            sprite = scopeBehaviour.GetSprite();
        //Assign Sprite!
        AssignSprite(_imageWeaponScope, sprite, scopeBehaviour == null || scopeBehaviour == scopeDefaultBehaviour);

        //Magazine.
        MagazineBehaviour magazineBehaviour = _attachmentManagerBehaviour.GetEquippedMagazine();
        //Get Sprite.
        if (magazineBehaviour != null)
            sprite = magazineBehaviour.GetSprite();
        //Assign Sprite!
        AssignSprite(_imageWeaponMagazine, sprite, magazineBehaviour == null);

        //Laser.
        LaserBehaviour laserBehaviour = _attachmentManagerBehaviour.GetEquippedLaser();
        //Get Sprite.
        if (laserBehaviour != null)
            sprite = laserBehaviour.GetSprite();
        //Assign Sprite!
        AssignSprite(_imageWeaponLaser, sprite, laserBehaviour == null);

        //Grip.
        GripBehaviour gripBehaviour = _attachmentManagerBehaviour.GetEquippedGrip();
        //Get Sprite.
        if (gripBehaviour != null)
            sprite = gripBehaviour.GetSprite();
        //Assign Sprite!
        AssignSprite(_imageWeaponGrip, sprite, gripBehaviour == null);

        //Muzzle.
        MuzzleBehaviour muzzleBehaviour = _attachmentManagerBehaviour.GetEquippedMuzzle();
        //Get Sprite.
        if (muzzleBehaviour != null)
            sprite = muzzleBehaviour.GetSprite();
        //Assign Sprite!
        AssignSprite(_imageWeaponMuzzle, sprite, muzzleBehaviour == null);

        _label.text = _weapon.Lable;
    }

    private static void AssignSprite(Image image, Sprite sprite, bool forceHide = false)
    {
        //Update.
        image.sprite = sprite;
        //Disable image if needed.
        image.enabled = sprite != null && !forceHide;
    }
}