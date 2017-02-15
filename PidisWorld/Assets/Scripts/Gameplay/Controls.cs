using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public static Controls s_instance;

    [SerializeField] private GameObject m_goDrillPlacementVisual;

    public GameObject goDrillPlacementVisual
    {
        get
        {
            return m_goDrillPlacementVisual;
        }
    }
    [SerializeField] private GameObject m_goRefineryPlacementVisual;

    public GameObject goRefineryPlacementVisual
    {
        get { return m_goRefineryPlacementVisual; }
    }

    private void Awake()
    {
        if (s_instance != null)
        {
            Debug.LogErrorFormat("There is already an instance of type {0}. Destroying this object.", GetType().Name);
            Destroy(gameObject);
        }
    }
}
