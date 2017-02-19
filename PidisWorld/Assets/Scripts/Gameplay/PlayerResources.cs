using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MathSigns
{
    Add,
    Substract,
    Divide,
    Multiply,
    Equal
}

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources s_instance;
    public static CultureInfo s_cultureInfo;

    [SerializeField] private int m_iStartCredits;
    [SerializeField] private int m_iStartWorkers;

    [SerializeField] private ResourceCounter m_resourceCredits;
    [SerializeField] private ResourceCounter m_resourceWorkers;

    [SerializeField] private GameObject m_goFinishedWindow;

    [Header("Tooltips")]
    [SerializeField] private Tooltip p_tooltipSimple;
    [SerializeField]
    private Tooltip p_tooltipGoal;
    [SerializeField] private Sprite m_spriteOil;
    [SerializeField]
    private Sprite m_spriteWorker;
    [SerializeField]
    private Sprite m_spriteEnergy;

    [Header("Math Signs")] [SerializeField] private MathSign p_mathSign;
    private List<MathSign> _drawnMathSigns;
    private List<IngameBuildingPair> _mathSignPairs;
    public List<IngameBuildingPair> MathSignPairs
    {
        get
        {
            return _mathSignPairs;
        }

        set
        {
            //DeleteAllMathSigns();
            _mathSignPairs = value;
            //DrawAllMathSigns();
        }
    }

    private int m_iCredits;

    public int iCredits
    {
        get
        {
            return m_iCredits;
        }
    }

    public int iWorkers
    {
        get { return m_iWorkers; }
    }

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
        _mathSignPairs = new List<IngameBuildingPair>();
        _drawnMathSigns = new List<MathSign>();
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

    public void ShowFinishedWindow()
    {
        m_goFinishedWindow.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        Application.Quit();
#endif
    }

    public Tooltip CreateTooltip(Tooltip.Type _eType, Transform _target, int _iValue)
    {
        Tooltip tooltipPrefab = null;
        Sprite sprite = null;
        switch (_eType)
        {
            case Tooltip.Type.Oil:
                tooltipPrefab = p_tooltipSimple;
                sprite = m_spriteOil;
                break;
            case Tooltip.Type.Worker:
                tooltipPrefab = p_tooltipSimple;
                sprite = m_spriteWorker;
                break;
            case Tooltip.Type.Energy:
                tooltipPrefab = p_tooltipGoal;
                sprite = m_spriteEnergy;
                break;
            default:
                throw new ArgumentOutOfRangeException("_eType", _eType, null);
        }

        Tooltip newTooltip = Instantiate(tooltipPrefab, transform, false);
        newTooltip.transform.SetAsFirstSibling();
        newTooltip.Initialize(_target, sprite, _iValue);

        return newTooltip;
    }

    public Tooltip CreateTooltip(Tooltip.Type _eType, Transform _target, int _iValue, int _iOptionalValue)
    {
        Tooltip newTooltip = CreateTooltip(_eType, _target, _iValue);
        newTooltip.UpdateOptionalValue(_iOptionalValue);

        return newTooltip;
    }

    public void ToggleTooltips(bool _bActive)
    {
        foreach (Tooltip tooltip in Tooltip.s_liTooltips)
        {
            tooltip.gameObject.SetActive(_bActive);
        }
    }

    public MathSign CreateMathSign(MathSigns _eType, IngameBuilding _buildingA, IngameBuilding _buildingB)
    {
        Vector3 v3PosA = _buildingA.transform.position;
        Vector3 v3PosB = _buildingB.transform.position;
        Vector3 v3DirAB = v3PosB - v3PosA;
        Vector3 v3PosMidAB = v3PosA + v3DirAB*.5f;

        Vector3 v3TargetWorldPos = v3PosMidAB;
        Vector2 v2TargetViewportPos = Camera.main.WorldToViewportPoint(v3TargetWorldPos);
        Vector2 v2CanvasSize = GetComponent<RectTransform>().sizeDelta;
        Vector2 v2TargetGuiPos = new Vector2(v2CanvasSize.x * v2TargetViewportPos.x, v2CanvasSize.y * v2TargetViewportPos.y);

        MathSign sign = Instantiate(p_mathSign, transform, false);
        sign.GetComponent<RectTransform>().anchoredPosition = v2TargetGuiPos;
        sign.Initialize(_eType);

        return sign;
    }

    public void AddMathSignBuildingPair(IngameBuilding a, IngameBuilding b)
    {
        if (MathSignPairs.Count == 0)
        {
            MathSignPairs.Add(new IngameBuildingPair(a, b));
        }
        else
        {
            for (int i = MathSignPairs.Count-1; i >= 0; i--)
            {
                IngameBuildingPair buildingPair = MathSignPairs[i];
                if (!buildingPair.HasPair(a, b))
                {
                    MathSignPairs.Add(new IngameBuildingPair(a, b));
                }
            }
        }
        DrawAllMathSigns();
    }

    private void DrawAllMathSigns()
    {
        DeleteAllMathSigns();
        _drawnMathSigns = new List<MathSign>();
        foreach (IngameBuildingPair buildingPair in MathSignPairs)
        {
            _drawnMathSigns.Add(
                s_instance.CreateMathSign(buildingPair.HasRocket() ? MathSigns.Equal : MathSigns.Add, buildingPair.BuildingA, buildingPair.BuildingB));
        }
    }

    private void DeleteAllMathSigns()
    {
        if (_drawnMathSigns.Count == 0) return;
        foreach (MathSign sign in _drawnMathSigns)
        {
            Destroy(sign.gameObject);
        }
    }
}
