using UnityEngine;

[RequireComponent(typeof(CanvasFade))]
public class GameScreen : MonoBehaviour
{
    private CanvasFade _canvasFade;
    //private List<RectTransform> _childObjects = new List<RectTransform>();

    private void Awake()
    {
        _canvasFade = GetComponent<CanvasFade>();
    }

    private void OnEnable()
    {
        _canvasFade.Showed += OnShowed;
    }

    private void OnDisable()
    {
        _canvasFade.Showed -= OnShowed;
    }

    public void OnShowed()
    {
        Init();

    }
    //Инит игрока и башни, запихиываем в UI здоровье, Можно игоку броник добавить
    private void Init()
    {
        //Здесь нужно разрешить двигаться игроку
        //А также занести все данные во вью, потому что можно менять в меню
        //Еще так сделать с башней
    }
}