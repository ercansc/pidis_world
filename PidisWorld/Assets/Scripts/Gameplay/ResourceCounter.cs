using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCounter : MonoBehaviour
{
    [SerializeField] private bool m_bValuePrefix = true;
    [SerializeField] private bool m_bClampToPositive = false;
    [SerializeField] private Color m_colPositive = Color.green;
    [SerializeField] private Color m_colNegative = Color.red;

    private Text m_txtCounter;

    private void Awake()
    {
        m_txtCounter = GetComponent<Text>();
        if (m_txtCounter == null)
        {
            Debug.LogErrorFormat("Missing Text Component on ResourceCounter {0}.", name);
        }
    }

    public void SetValue(int _iValue)
    {
        if (m_bClampToPositive)
        {
            _iValue = Mathf.FloorToInt(Mathf.Clamp(_iValue, 0, Mathf.Infinity));
        }
        string strValuePrefix = "";
        if (m_bValuePrefix && _iValue >= 0)
        {
            strValuePrefix = "+";
        }
        string strValue = string.Format("{0}{1}", strValuePrefix, _iValue.ToString("N0", PlayerResources.s_cultureInfo));
        m_txtCounter.text = strValue;
        m_txtCounter.color = _iValue >= 0 ? m_colPositive : m_colNegative;
    }
}
