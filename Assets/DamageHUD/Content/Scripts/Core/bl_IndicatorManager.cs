using UnityEngine;
using System.Collections.Generic;

public class bl_IndicatorManager : MonoCache
{
    [Header("Settings")]
    [Range(1,15)]
    [Tooltip("Time before fade sprite indicator.")]
    [SerializeField] private float _timeToShow = 5;
    [Range(20, 100)] [SerializeField] private float _pivotSize = 20;
    [Range(0,70)] [SerializeField] private float _inclination = 10;
    [SerializeField] private Vector2 _spriteSize = new Vector2(80, 25);
    [SerializeField] private Color _spriteColor = Color.white;
    [Header("References")]
    [Tooltip("This can be the root of player or the camera player.")]
    [SerializeField] private GameObject _indicatorUI;
    [Tooltip("RectTransform where indicators will be instantiate (Default Root Canvas)")]
    [SerializeField] private Transform _panelIndicator;
    [SerializeField] private Canvas _canvas;

    private List<bl_Indicator> _indicatorsEntrys = new List<bl_Indicator>();
    private Player _player;
    private void OnEnable() => AddUpdate();
    
    private void OnDisable() => RemoveUpdate();


    private void Start()
    {
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            _panelIndicator.localEulerAngles = new Vector3(_inclination, 0, 0);
    }

    public void Init(Player player)
    {
        _player = player;
    }

    public void SetIndicator(Zombie zombie)
    {
        bl_IndicatorInfo info = new bl_IndicatorInfo(zombie.transform.position);
        info.Sender = zombie.gameObject;
        info.Color = _spriteColor;
        OnNewIndicator(info);
    }

    private void OnNewIndicator(bl_IndicatorInfo info)
    {
        //Apply globat settings
        info.PivotSize = _pivotSize;
        info.Size = _spriteSize;
        //Determine if need create on new indicator or just update one existing
        //this is determined based on whether there is an indicator of the same sender
        //so, first check if have the same sender
        if (bl_IndicatorUtils.CheckIfHaveSender(info.Sender, _indicatorsEntrys))
        {
            //if have a sender, then get it from list for update.
            int id = bl_IndicatorUtils.GetSenderInList(info.Sender, _indicatorsEntrys);
            //If is update just show the half of time.
            info.TimeToShow = _timeToShow / 2;
            UpdateIndicator(info, id);
        }
        else
        {
            info.TimeToShow = _timeToShow;
            
            if (info.Color == new Color(1, 1, 1, 0))
                info.Color = _spriteColor;

            CreateNewIndicator(info);
        }
    }

    private void CreateNewIndicator(bl_IndicatorInfo info)
    {
        GameObject newentry = Instantiate(_indicatorUI) as GameObject;
        bl_Indicator indicator = newentry.GetComponent<bl_Indicator>();
        indicator.GetInfo(info,this);
        newentry.transform.SetParent(_panelIndicator, false);
        //cache the new indicator
        _indicatorsEntrys.Add(indicator);
    }


    private void UpdateIndicator(bl_IndicatorInfo info,int id)
    {
        bl_Indicator indicator = _indicatorsEntrys[id];
        if(indicator == null)
        {
            Debug.LogWarning("Can't update indicator because this doesn't exit in list");
            return;
        }

        if (info.Color == new Color(1, 1, 1, 0))
            info.Color = _spriteColor;

        indicator.GetInfo(info, this, true);
    }

    public override void OnTick()
    {
        if(_indicatorsEntrys.Count > 0)
            ControllIndicators();
    }

    private void ControllIndicators()
    {
        for(int i = 0; i < _indicatorsEntrys.Count; i++)
        {
            bl_Indicator indicator = _indicatorsEntrys[i];
            //Remove nulls indicators in list
            if(indicator == null || indicator.Transform == null)
            {
                _indicatorsEntrys.Remove(indicator);
                return;
            }

            //If show distance
            if (indicator.Info.ShowDistance)
            {
                //Calculate distance from sender
                float d = Vector3.Distance(indicator.Info.Sender.transform.position, _player.transform.position);
                indicator.UpdateDistance(d);
            }
            Vector3 forward = _player.transform.forward;
          
            //Calculate direction 
            Vector3 rhs = indicator.Info.Direction - _player.transform.position;
            Vector3 offset = indicator.Transform.localEulerAngles;
            //Convert angle into screen space
            rhs.y = 0f;
            rhs.Normalize();
            //Get the angle between two positions.
            float angle = Vector3.Angle(rhs, forward);
            //Calculate the perpendicular of both vectors
            //More information about this calculation: https://unity3d.com/es/learn/tutorials/modules/beginner/scripting/vector-maths-dot-cross-products?playlist=17117
            Vector3 Perpendicular = Vector3.Cross(forward, rhs);
            //Calculate magnitude between two vectors
            float dot = -Vector3.Dot(Perpendicular, _player.transform.up);
            //get the horizontal angle in direction of target / sender.
            angle = bl_IndicatorUtils.AngleCircumference(dot, angle);
            //Apply the horizontal rotation to the indicator.
            offset.z = angle;
            
            indicator.Transform.localRotation = Quaternion.Slerp(indicator.Transform.localRotation, Quaternion.Euler(offset), 17 * Time.deltaTime);
        }       
    }

    public void RemoveIndicator(bl_Indicator indicator)
    {
        if (_indicatorsEntrys.Contains(indicator))
            _indicatorsEntrys.Remove(indicator);
        else
            Debug.LogWarning("This indicator " + indicator.gameObject.name + " is not in list.");
    }
}