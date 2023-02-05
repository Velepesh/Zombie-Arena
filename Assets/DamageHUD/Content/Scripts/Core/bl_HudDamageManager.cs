using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class bl_HudDamageManager : MonoBehaviour {
    [SerializeField] private Player _player;
    [Header("Settings")]
    [Range(0,10)]
    [SerializeField]private float DelayFade = 0.25f;
    [Range(0.01f,5)]
    [SerializeField]private float FadeSpeed = 0.4f;
    [Range(0.1f,0.9f)]
    [SerializeField]private float MinAlpha = 0.4f;
    [SerializeField]private AnimationCurve CurveFade;
    [SerializeField]private bool AnimateHealthInfo = true;
    [SerializeField]private Color MaxHealthColor;
    [SerializeField]private Color MinHealthColor;

    [Header("Shake")]
    [SerializeField]private bool useShake = true;
    public Transform ShakeObject = null;
    private Vector3 originPosition;
    private Quaternion originRotation;
    [Range(0.001f, 0.01f)]
    [SerializeField]private float ShakeDecay = 0.002f;
    [Range(0.01f, 0.2f)]
    [SerializeField]private float ShakeIntensity = 0.02f;
    [Range(0.01f, 0.5f)]
    [SerializeField]private float ShakeAmount = 0.2f;
    private float shakeIntensity;

    [Header("References")]
    [SerializeField]private CanvasGroup m_canvasGroup;
    [SerializeField]private Slider HealthSlider = null;
    [SerializeField]private Text HealthText = null;
    [SerializeField]private GameObject DeathHUD;
    [SerializeField]private GameObject HealthInfo;

    private float Alpha = 0;
    private float Health = 100;
    private float MaxHealth = 100;
    private float NextDelay = 0;
    private int HealthValue;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        if (HealthSlider != null)
        {
            HealthSlider.maxValue = MaxHealth;
            HealthSlider.value = Health;
        }
        if (!AnimateHealthInfo) { HealthInfo.GetComponent<Animator>().enabled = false; }
        HealthValue = (int)Health;
        originPosition = ShakeObject.localPosition;
        originRotation = ShakeObject.localRotation;
    }

    /// <summary>
    /// Register all callbacks 
    /// </summary>
    void OnEnable()
    {
        _player.Health.HealthChanged += OnDamage;
        _player.Died += OnDie;
    }

    /// <summary>
    /// UnRegister all callbacks 
    /// </summary>
    void OnDisable()
    {
        _player.Health.HealthChanged -= OnDamage;
        _player.Died -= OnDie;
    }

    void OnDamage(int health)
    {
        //Calculate the diference in health for apply to the alpha.
        Alpha = (_player.Health.StartValue - health) / 100;
        //Ensure that alpha is never less than the minimum allowed
        Alpha = Mathf.Clamp(Alpha, MinAlpha, 1);
        //Update delay
        NextDelay = Time.time + DelayFade;
        if (AnimateHealthInfo)
            HealthInfoControll();

        if (useShake && ShakeObject != null) 
        { 
            StopAllCoroutines(); 
            StartCoroutine(Shake()); 
        }
    }

    void OnDie(IDamageable damageable)
    {
        //Active the death hud.
        DeathHUD.SetActive(true);
    }

    /// <summary>
    /// If the health default or maxHealth default of player no equal to 100
    /// then call this for update with this and keep it synchronized at the start
    /// NOTE: Call this in start / awake from your 'player health' script
    /// </summary>
    /// <param name="_health"></param>
    /// <param name="_maxHealth"></param>
    public void SetUp(float _health,float _maxHealth)
    {
        Health = _health;
        MaxHealth = _maxHealth;

        if (HealthSlider != null)
        {
            HealthSlider.maxValue = MaxHealth;
            HealthSlider.value = Health;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate()
    {
        //Apply fade effect to HUD.
        FadeRedScreen();
        HealthHUDControll();
    }

    /// <summary>
    /// 
    /// </summary>
    void FadeRedScreen()
    {
        if (m_canvasGroup.alpha != Alpha)
        {
            if (Time.time > NextDelay && Alpha > 0)
            {
                Alpha = Mathf.Lerp(Alpha, 0, Time.deltaTime);
                Alpha = CurveFade.Evaluate(Alpha);
            }
            m_canvasGroup.alpha = Mathf.Lerp(m_canvasGroup.alpha, Alpha, Time.deltaTime * FadeSpeed);
        }
    }

    void HealthHUDControll()
    {
        if(HealthSlider != null)
        {
            Image fillImage = HealthSlider.fillRect.GetComponent<Image>();
            HealthSlider.value = Mathf.Lerp(HealthSlider.value, Health, 7 * Time.deltaTime);
            fillImage.color = Color.Lerp(MinHealthColor, fillImage.color, HealthSlider.value / MaxHealth);
        }
        if(HealthText != null)
        {
            HealthValue = (int)Mathf.Lerp(HealthValue, Health, 5 * Time.deltaTime);
            HealthText.text = (Health > 0) ? HealthValue.ToString() : "Dead";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    void HealthInfoControll(float value = 0)
    {
        if (HealthInfo == null)
            return;

        Animator a = HealthInfo.GetComponent<Animator>();
        a.Play("HealthInfoHit", 0, 0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerator Shake()
    {
        shakeIntensity = ShakeIntensity;
        while (shakeIntensity > 0)
        {
            ShakeObject.localPosition = originPosition + Random.insideUnitSphere * shakeIntensity;
            ShakeObject.localRotation = new Quaternion(
                originRotation.x + Random.Range(-shakeIntensity, shakeIntensity) * ShakeAmount,
                originRotation.y + Random.Range(-shakeIntensity, shakeIntensity) * ShakeAmount,
                originRotation.z + Random.Range(-shakeIntensity, shakeIntensity) * ShakeAmount,
                originRotation.w + Random.Range(-shakeIntensity, shakeIntensity) * ShakeAmount);
            shakeIntensity -= ShakeDecay;
            yield return false;
        }
        ShakeObject.localPosition = originPosition;
        ShakeObject.localRotation = originRotation;
    }
    public float BloodFadeSpeed { get; set; }

    public float FadeDelay { get; set; }
}