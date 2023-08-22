using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static partial class GFunc
{
    


    public static bool IsValid<T>(this T component_) where T : Component
    {
        Component convert = (Component)(component_ as Component);
        bool isInValid = convert == null || convert == default;
        return !isInValid;
    }       // IsValid()

    public static bool IsValid(this GameObject gameobject_)
    {
        bool isInvalid = (gameobject_ == null || gameobject_ == default);
        return !isInvalid;
    }       // IsValid()

    public static bool IsValid<T>(this List<T> list_)
    {
        bool isInValid = (list_ == null || list_.Count < 1);
        return !isInValid;
    }       // IsValid()

    public static bool IsValid<T>(this Queue<T> queue_)
    {
        bool isInValid = (queue_ == null || queue_.Count < 1);
        return !isInValid;
    }       // IsValid()

}

