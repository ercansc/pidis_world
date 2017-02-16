using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmDestroyMenu : MonoBehaviour
{
    public delegate void OnConfirmDestroyCallback();
    public delegate void OnCancelDestroyCallback();

    private OnCancelDestroyCallback m_onCancelCallback;
    private OnConfirmDestroyCallback m_onConfirmCallback;

    private IngameBuilding m_building;

    public void Initialize(IngameBuilding _building, OnConfirmDestroyCallback _confirmCallback, OnCancelDestroyCallback _cancelCallback)
    {
        m_building = _building;
        m_onConfirmCallback = _confirmCallback;
        m_onCancelCallback = _cancelCallback;
    }

    public void Confirm()
    {
        if (m_onConfirmCallback != null)
        {
            m_onConfirmCallback.Invoke();
            m_onConfirmCallback = null;
        }
        Close();
    }

    public void Cancel()
    {
        if (m_onCancelCallback != null)
        {
            m_onCancelCallback.Invoke();
            m_onCancelCallback = null;
        }
    }

    private void Close()
    {
        m_building = null;
        gameObject.SetActive(false);
    }
}
