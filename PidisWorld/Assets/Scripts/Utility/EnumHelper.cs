using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumHelper
{

    public static string[] GetObjectTypes()
    {
        string[] types = new string[(int)GridObjectType.Count];
        for (int i = 0; i < (int)GridObjectType.Count; i++)
        {
            types[i] = ((GridObjectType)i).ToString();
        }
        return types;
    }

    public static string GetObjectStrings()
    {
        string[] types = GetObjectTypes();
        string returnString = "";

        for (int i = 0; i < (int)GridObjectType.Count; i++)
        {
            returnString += types[i] + ", ";
        }
        return returnString;
    }

}
