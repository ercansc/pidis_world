using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerCountGrid : MonoBehaviour
{
    [SerializeField] private GameObject p_goNumberPrefab;
    [SerializeField] private Sprite[] m_arNumberSprites;

    [ContextMenu("Populate Grid")]
    private void PopulateGrid()
    {
        List<GameObject> liChildren = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                liChildren.Add(child.gameObject);
            }
        }
        foreach (GameObject child in liChildren)
        {
            DestroyImmediate(child);
        }

        foreach (Sprite numberSprite in m_arNumberSprites)
        {
            GameObject goNumber = Instantiate(p_goNumberPrefab, transform);
            goNumber.SetActive(true);
            goNumber.transform.GetComponentInChildren<Image>().sprite = numberSprite;
        }
    }
}