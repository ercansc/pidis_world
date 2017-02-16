using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


[CustomEditor(typeof(Grid))]
public class GridDesignerEditor : Editor
{
    private GridObjectType _selectedGridObjectType;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();

        Grid grid = (Grid) target;

        if (grid.HasGrid)
        {
            _selectedGridObjectType = (GridObjectType) EditorGUILayout.Popup((int)_selectedGridObjectType, EnumHelper.GetObjectTypes());

            for (int x = 0; x < (int) grid.SizeX; x++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int y = 0; y < (int) grid.SizeY; y++)
                {
                    string buttonString = "";
                    if (grid[x, y].ContainedObject == null)
                    {
                        buttonString = "";
                    }
                    else
                    {
                        switch (grid[x, y].ContainedObject.name)
                        {
                            case "Rocket":
                                buttonString = "R";
                                break;
                            case "OilField":
                                buttonString = "O";
                                break;
                            case "Blocker":
                                buttonString = "X";
                                break;
                            default:
                                buttonString = "?";
                                break;
                        }
                    }

                    if (GUILayout.Button(buttonString, GUILayout.Height(40), GUILayout.Width(40)))
                    {
                        GameObject go =
                            PrefabUtility.InstantiatePrefab(GridObjectsManager.Instance.GetObjectData().GridTypePrefabs[(int) _selectedGridObjectType]) as
                                GameObject;
                        go.name = _selectedGridObjectType.ToString();
                        go.transform.position = grid[x, y].transform.position;
                        go.transform.SetParent(grid[x, y].transform);
                        grid[x, y].ContainedObject = go;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Create Grid", GUILayout.Height(40), GUILayout.Width(200)))
        {
            grid.InitNewGrid();
        }

        if (!grid.HasGrid) return;
        if (GUILayout.Button("Delete Grid", GUILayout.Width(200)))
        {
            grid.RemoveGrid();
        }
    }
}
