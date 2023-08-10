using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static partial class GFunc
{
    public static bool IsValid<T>(this T component_ ) where T : Component
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

    public static IEnumerator PunchScale(Transform tr,float punchTime, float maxSize, float minSize)
    {
        float spendTime = 0f;
        while (spendTime < punchTime)
        {
            Debug.Log("1");
            spendTime  += Time.time;
            Vector3 scaleChange = new Vector3(maxSize, maxSize, 0f);
            float y = tr.localScale.y;

            tr.localScale = new Vector3(tr.localScale.x, y * maxSize * Mathf.Sin(spendTime));
           

            //tr.localScale = Vector3.Lerp(tr.localScale, scaleChange, 0.5f);



            yield return null;  
        }
    }
}
