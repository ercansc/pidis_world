﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


[CustomEditor(typeof(Grid))]
public class GridDesignerEditor : Editor
{
    private GridObjectType _selectedGridObjectType;
    private Grid grid;
    private int _selectedIntValue;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();

        grid = (Grid) target;

        if (grid.HasGrid)
        {
            EditorGUILayout.BeginHorizontal();

            _selectedGridObjectType =
                (GridObjectType) EditorGUILayout.Popup((int) _selectedGridObjectType, EnumHelper.GetObjectTypes(), GUILayout.Width(250));

            if (_selectedGridObjectType == GridObjectType.OilField)
            {
                _selectedIntValue = EditorGUILayout.IntField(_selectedIntValue);
            }

            EditorGUILayout.EndHorizontal();

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
                            case "Empty":
                                buttonString = "";
                                break;
                            default:
                                buttonString = "?";
                                break;
                        }
                    }

                    if (GUILayout.Button(buttonString, GUILayout.Height(40), GUILayout.Width(40)))
                    {
                        if (grid[x, y].ContainedObject == null)
                        {
                            grid[x, y].ContainedObject = CreateNewObject(grid, x, y);
                        }
                        else
                        {
                            if (grid[x, y].ContainedObject.name.Equals(_selectedGridObjectType.ToString()) ||
                                grid[x, y].ContainedObject.name.Equals("Empty"))
                            {
                                DestroyImmediate(grid[x, y].transform.GetChild(0).gameObject);
                                grid[x, y].ContainedObject = null;
                            }
                            else
                            {
                                DestroyImmediate(grid[x, y].transform.GetChild(0).gameObject);
                                grid[x, y].ContainedObject = CreateNewObject(grid, x, y);
                            }
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Create New Grid", GUILayout.Height(40), GUILayout.Width(200)))
        {
            grid.SetNewGrid(grid.ThisGrid);
        }

        if (!grid.HasGrid) return;
        if (GUILayout.Button("Delete Grid", GUILayout.Width(200)))
        {
            grid.RemoveGrid();
        }
    }

    private GameObject CreateNewObject(Grid grid, int x, int y)
    {
        if (_selectedGridObjectType.ToString().Equals("Empty")) return null;

        GameObject go =
            PrefabUtility.InstantiatePrefab(
                GridObjectsManager.Instance.GetObjectData().GridTypePrefabs[(int) _selectedGridObjectType]) as GameObject;
        go.name = _selectedGridObjectType.ToString();
        go.transform.position = grid[x, y].transform.position;
        go.transform.SetParent(grid[x, y].transform);

        if (_selectedGridObjectType == GridObjectType.OilField)
        {
            OilField of = go.AddComponent<OilField>();
            of.OilValue = _selectedIntValue;
        }

        return go;
    }
}
