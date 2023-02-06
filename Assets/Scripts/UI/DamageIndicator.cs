using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DamageIndicator : MonoBehaviour
{
    //[SerializeField] private Player _player;
    //[Header("List")]
    //public List<bl_Indicator> _indicatorsEntrys = new List<bl_Indicator>();
    //[Header("Settings")]
    //[Range(1, 15)]
    //[Tooltip("Time before fade sprite indicator.")]
    //public float _timeToShow = 5;
    //[Range(20, 100)]
    //public float _pivotSize = 20;
    //[Range(0, 70)]
    //[SerializeField] private float _inclination = 10;
    //public Vector2 _spriteSize = new Vector2(80, 25);
    //public Color _spriteColor = Color.white;
    //[Tooltip("Use smooth movement rotation?.")]
    //public bool LerpMovement = true;
    //[Tooltip("Use local position of camera or localPlayer Object as reference?")]
    //public bool UseCameraReference = true;
    //public bool ShowDistance = true;
    //[Header("References")]
    //[Tooltip("This can be the root of player or the camera player.")]
    //[SerializeField] private Transform LocalPlayer;
    //[SerializeField] private GameObject _indicatorUI;
    //[Tooltip("RectTransform where indicators will be instantiate (Default Root Canvas)")]
    //[SerializeField] private Transform _panelIndicator;


    //private void Start()
    //{
    //    if (_panelIndicator.transform.root.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceCamera)
    //    {
    //        _panelIndicator.localEulerAngles = new Vector3(_inclination, 0, 0);
    //    }
    //}

    //private void OnEnable()
    //{
    //   // _player.Health.HealthChanged += OnHealthChanged;
    //    _player.DamageTaken += OnDamageTaken;
    //}

    //private void OnDisable()
    //{
    //    //_player.Health.HealthChanged -= OnHealthChanged;
    //    _player.DamageTaken -= OnDamageTaken;
    //}

    //private void OnDamageTaken(Vector3 contactPosition)
    //{
    //    SetIndicator(_spriteColor, contactPosition);
    //}

    //public void SetIndicator(Color customColor, Vector3 contactPosition)
    //{
    //    //Just in case that go is destroy
    //    bl_IndicatorInfo info = new bl_IndicatorInfo(contactPosition);
    //    info.Sender = contactPosition;
    //    info.Color = customColor;
    //    OnNewIndicator(info);
    //}

    ///// <summary>
    ///// when a new event called
    ///// </summary>
    ///// <param name="info">info of new indicator</param>
    //private void OnNewIndicator(bl_IndicatorInfo info)
    //{
    //    //Apply globat settings
    //    info._pivotSize = _pivotSize;
    //    info.Size = _spriteSize;
    //    info.ShowDistance = ShowDistance;
    //    //Determine if need create on new indicator or just update one existing
    //    //this is determined based on whether there is an indicator of the same sender
    //    //so, first check if have the same sender
    //    if (bl_IndicatorUtils.CheckIfHaveSender(info.Sender, _indicatorsEntrys))
    //    {
    //        //if have a sender, then get it from list for update.
    //        int id = bl_IndicatorUtils.GetSenderInList(info.Sender, _indicatorsEntrys);
    //        //If is update just show the half of time.
    //        info._timeToShow = _timeToShow / 2;
    //        UpdateIndicator(info, id);
    //    }
    //    else//if not have a sender, them create a new indicator and cache this sender.
    //    {
    //        info._timeToShow = _timeToShow;
    //        //If dont have asigne a color yet.
    //        if (info.Color == new Color(1, 1, 1, 0))
    //        {
    //            info.Color = _spriteColor;
    //        }
    //        CreateNewIndicator(info);
    //    }
    //}

    ///// Create a new indicator UI

    //private void CreateNewIndicator(bl_IndicatorInfo info)
    //{
    //    GameObject newentry = Instantiate(_indicatorUI) as GameObject;
    //    bl_Indicator indicator = newentry.GetComponent<bl_Indicator>();
    //    indicator.GetInfo(info, this);
    //    newentry.transform.SetParent(_panelIndicator, false);
    //    //cache the new indicator
    //    _indicatorsEntrys.Add(indicator);
    //}

    ///// <summary>
    ///// If have a indicator of a same sender and this is available yet.
    ///// them just update information.
    ///// </summary>
    ///// <param name="info"></param>
    ///// <param name="id"></param>
    //private void UpdateIndicator(bl_IndicatorInfo info, int id)
    //{
    //    bl_Indicator indicator = _indicatorsEntrys[id];
    //    if (indicator == null)
    //    {
    //        Debug.LogWarning("Can't update indicator because this doesn't exit in list");
    //        return;
    //    }
    //    if (info.Color == new Color(1, 1, 1, 0))
    //    {
    //        info.Color = _spriteColor;
    //    }

    //    indicator.GetInfo(info, this, true);
    //}

    //private void FixedUpdate()
    //{
    //    //Just call if have at least a indicator
    //    if (_indicatorsEntrys.Count > 0)
    //    {
    //        ControllIndicators();
    //    }
    //}

    ///// <summary>
    ///// Control direction of each Indicator in List
    ///// </summary>
    //private void ControllIndicators()
    //{
    //    for (int i = 0; i < _indicatorsEntrys.Count; i++)
    //    {
    //        bl_Indicator indicator = _indicatorsEntrys[i];
    //        //Remove nulls indicators in list
    //        if (indicator == null || indicator.Transform == null)
    //        {
    //            _indicatorsEntrys.Remove(indicator);
    //            return;
    //        }

    //        //If show distance
    //        if (indicator.Info.ShowDistance)
    //        {
    //            //Calculate distance from sender
    //            float d = Vector3.Distance(indicator.Info.Sender.transform.position, LocalPlayer.position);
    //            indicator.UpdateDistance(d);
    //        }
    //        Vector3 forward = Vector3.zero;
    //        //Get camera player or current camera
    //        if (UseCameraReference && bl_IndicatorUtils.UseCamera != null)
    //        {
    //            forward = bl_IndicatorUtils.UseCamera.transform.forward;
    //        }
    //        else
    //        {
    //            forward = LocalPlayer.forward;
    //        }
    //        //Calculate direction 
    //        Vector3 rhs = indicator.Info.Direction - LocalPlayer.position;
    //        Vector3 offset = indicator.Transform.localEulerAngles;
    //        //Convert angle into screen space
    //        rhs.y = 0f;
    //        rhs.Normalize();
    //        //Get the angle between two positions.
    //        float angle = Vector3.Angle(rhs, forward);
    //        //Calculate the perpendicular of both vectors
    //        //More information about this calculation: https://unity3d.com/es/learn/tutorials/modules/beginner/scripting/vector-maths-dot-cross-products?playlist=17117
    //        Vector3 Perpendicular = Vector3.Cross(forward, rhs);
    //        //Calculate magnitude between two vectors
    //        float dot = -Vector3.Dot(Perpendicular, LocalPlayer.up);
    //        //get the horizontal angle in direction of target / sender.
    //        angle = bl_IndicatorUtils.AngleCircumference(dot, angle);
    //        //Apply the horizontal rotation to the indicator.
    //        offset.z = angle;
    //        if (LerpMovement)
    //        {
    //            indicator.Transform.localRotation = Quaternion.Slerp(indicator.Transform.localRotation, Quaternion.Euler(offset), 17 * Time.deltaTime);
    //        }
    //        else
    //        {
    //            indicator.Transform.localRotation = Quaternion.Euler(offset);
    //        }
    //    }
    //}
}
