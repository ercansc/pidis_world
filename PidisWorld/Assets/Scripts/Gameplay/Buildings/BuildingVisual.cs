using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    public Sprite Sprite
    {
        get { return m_spriteRenderer.sprite; }
    }

    public void SetSprite(Sprite _sprite)
    {
        m_spriteRenderer.sprite = _sprite;
    }

    public void Highlight(Color _color)
    {
        m_spriteRenderer.color = _color;
    }

    public void OnEnterTile(bool _bCanBePlaced, GridTile _tile)
    {
            m_spriteRenderer.color =_bCanBePlaced
                ? Colors.Instance.BuildingFree
                : Colors.Instance.BuildingBlocked;

            transform.position = _tile.transform.position;
    }
}
