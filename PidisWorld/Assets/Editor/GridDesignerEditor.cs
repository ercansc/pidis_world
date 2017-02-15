using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Grid))]
public class GridDesignerEditor : Editor
{
    public GridTile[,] MainGrid;


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Grid grid = (Grid) target;

        for (int x = (int) grid.SizeX - 1; x >= 0; x--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int y = 0; y < (int)grid.SizeY; y++)
            {
                if (GUILayout.Button(String.Format("{0} | {1}", x, y)))
                {
                    Debug.Log(String.Format("{0} | {1}", x, y));
                }
            }
            EditorGUILayout.EndHorizontal();
        }


        EditorGUILayout.Space();
        if (GUILayout.Button("Create Grid"))
        {

        }
    }
}
