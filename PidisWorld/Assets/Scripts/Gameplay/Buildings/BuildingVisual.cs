using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    public void SetSprite(Sprite _sprite)
    {
        m_spriteRenderer.sprite = _sprite;
    }

    public void Highlight(Color _color)
    {
        m_spriteRenderer.color = _color;
    }

    public void OnEnterTile(GridTile _tile)
    {
            m_spriteRenderer.color = _tile.Blocked
                ? Colors.Instance.BuildingBlocked
                : Colors.Instance.BuildingFree;

            transform.position = _tile.transform.position;
    }
}
