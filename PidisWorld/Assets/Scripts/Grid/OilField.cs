using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilField : MonoBehaviour
{
    public int OilValue;

    private Tooltip m_tooltip;

    private void Start()
    {
        if (m_tooltip == null)
        {
            m_tooltip = PlayerResources.s_instance.CreateTooltip(Tooltip.Type.Oil, transform, OilValue);
        }
    }

	void Update ()
    {
		
	}
}
