using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementVisual : MonoBehaviour
{
    [SerializeField]
    private Color m_colTileFree = Color.green;
    [SerializeField]
    private Color m_colTileBlocked = Color.red;

    private SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite _sprite)
    {
        m_spriteRenderer.sprite = _sprite;
    }

    public void OnEnterTile(GridTile _tile)
    {
            m_spriteRenderer.color = _tile.Blocked
                ? m_colTileBlocked
                : m_colTileFree;

            transform.position = _tile.transform.position;
    }
}
