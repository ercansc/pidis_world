using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public enum Type
    {
        Oil,
        Worker,
        Energy
    }

    [SerializeField] private Image m_imgIcon;
    [SerializeField] private Text m_txtValue;
    [SerializeField] private Text m_txtOptionalValue;
    [SerializeField] private float m_fOffsetY;

    private Transform m_target;

    public static List<GameObject> s_liTooltips = new List<GameObject>();

    public void Initialize(Transform _target, Sprite _sprite, int _iValue)
    {
        s_liTooltips.Add(gameObject);
        m_target = _target;
        m_imgIcon.sprite = _sprite;
        UpdateValue(_iValue);

        UpdatePosition();
    }

    public void UpdatePosition()
    {
        Vector3 v3TargetWorldPos = m_target.transform.position;
        Vector2 v2TargetViewportPos = Camera.main.WorldToViewportPoint(v3TargetWorldPos);
        Vector2 v2CanvasSize = PlayerResources.s_instance.GetComponent<RectTransform>().sizeDelta;
        Vector2 v2TargetGuiPos = new Vector2(v2CanvasSize.x * v2TargetViewportPos.x, v2CanvasSize.y * v2TargetViewportPos.y + m_fOffsetY);
        GetComponent<RectTransform>().anchoredPosition = v2TargetGuiPos;
    }

    public void UpdateValue(int _iValue)
    {
        m_txtValue.text = _iValue.ToString();
    }

    public void UpdateOptionalValue(int _iValue)
    {
        m_txtOptionalValue.text = string.Format("/ {0}", _iValue);
    }

    private void OnDestroy()
    {
        s_liTooltips.Remove(gameObject);
    }
}