using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSingleton<T> : GComponent where T : GSingleton<T>
{
    private static T _instance = default;

    public static T Instance
    {
        get
        {
            if (GSingleton<T>._instance == default || _instance == default)
            {
                GSingleton<T>._instance =
                    GFunc.CreateObj<T>(typeof(T).ToString());
            }
            
            
            return _instance;
        }
    }
   
}
