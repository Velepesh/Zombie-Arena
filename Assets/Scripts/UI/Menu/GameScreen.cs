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
    //���� ������ � �����, ����������� � UI ��������, ����� ����� ������ ��������
    private void Init()
    {
        //����� ����� ��������� ��������� ������
        //� ����� ������� ��� ������ �� ���, ������ ��� ����� ������ � ����
        //��� ��� ������� � ������
    }
}