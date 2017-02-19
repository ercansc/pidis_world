using System;
using UnityEngine;
using UnityEngine.UI;

public class MathSign : MonoBehaviour
{
    [SerializeField]
    private Image m_imgSign;

    [SerializeField]
    private Sprite m_spriteAdd;

    [SerializeField]
    private Sprite m_spriteEquals;

    public void Initialize(MathSigns _eType)
    {
        switch (_eType)
        {
            case MathSigns.Add:
                m_imgSign.sprite = m_spriteAdd;
                break;
            case MathSigns.Substract:
                break;
            case MathSigns.Divide:
                break;
            case MathSigns.Multiply:
                break;
            case MathSigns.Equal:
                m_imgSign.sprite = m_spriteEquals;
                break;
            default:
                throw new ArgumentOutOfRangeException("_eType", _eType, null);
        }
    }
}