//using System;
//using UnityEngine;

//public class FileFinder<T> where T : ScriptableObject
//{
//    public static T FindByName(string name)
//    {
//        T[] assets = Resources.LoadAll<T>("");
//        foreach (T asset in assets)
//        {
//            if (asset.name == name)
//                return asset;
//        }

//        Debug.LogWarning($"No {typeof(T)} found with name {name}");
//        return null;
//    }
//}
