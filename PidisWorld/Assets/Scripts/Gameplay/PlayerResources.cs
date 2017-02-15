using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources s_instance;
    public static CultureInfo s_cultureInfo;

    [SerializeField] private int m_iStartCredits;
    [SerializeField] private int m_iStartWorkers;

    [SerializeField] private ResourceCounter m_resourceCredits;
    [SerializeField] private ResourceCounter m_resourceWorkers;

    private int m_iCredits;
    private int m_iWorkers;

    private void Awake()
    {
        s_cultureInfo = CultureInfo.CreateSpecificCulture("de-DE"); ;
        if (s_instance != null)
        {
            Debug.LogErrorFormat("There is already an instance of type {0}. Destroying this object.", GetType().Name);
            Destroy(gameObject);
        }
        else
        {
            s_instance = this;
        }
    }

    private void Start()
    {
        InitializeResources();
    }

    private void InitializeResources()
    {
        m_iCredits = m_iStartCredits;
        m_resourceCredits.SetValue(m_iCredits);
        m_iWorkers = m_iStartWorkers;
        m_resourceWorkers.SetValue(m_iWorkers);
    }

    public void AddCredits(int _iValue)
    {
        m_iCredits += _iValue;
        m_resourceCredits.SetValue(m_iCredits);
    }

    public void AddWorkers(int _iValue)
    {
        m_iWorkers += _iValue;
        m_resourceWorkers.SetValue(m_iWorkers);
    }
}
