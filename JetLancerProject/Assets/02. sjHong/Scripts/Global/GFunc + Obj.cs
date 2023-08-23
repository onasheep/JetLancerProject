using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static partial class GFunc
{

    //! 씬의 루트 오브젝트를 서치해서 찾아주는 함수
    public static GameObject GetRootObj(string objName_)
    {
        Scene activeScene_ = GetActiveScene();
        GameObject[] rootObjs_ = activeScene_.GetRootGameObjects();

        GameObject targetObj_ = default;
        foreach (GameObject rootObj in rootObjs_)
        {
            if (rootObj.name.Equals(objName_))
            {
                targetObj_ = rootObj;
                return targetObj_;
            }
            else { continue; }
        }       // loop

        return targetObj_;
    }       // GetRootObj()


    //! 씬의 루트 오브젝트로 부터 자식오브젝트의 컴퍼넌트를 이름으로 가져오는 함수
    //! 특정 오브젝트의 자식 오브젝트를 서치해서 컴포넌트를 찾아주는 함수
    public static T FindChildComponent<T>(
        this GameObject targetObj_, string objName_) where T : Component
    {
        T searchResultComponent = default(T);
        GameObject searchResultObj = default(GameObject);

        searchResultObj = targetObj_.FindChildObj(objName_);
        if (searchResultObj.IsValid())
        {
            searchResultComponent = searchResultObj.GetComponent<T>();
        }

        return searchResultComponent;
    }       // FindChildObj()

    //! 특정 오브젝트의 자식 오브젝트를 서치해서 찾아주는 함수
    public static GameObject FindChildObj(
        this GameObject targetObj_, string objName_)
    {
        GameObject searchResult = default;
        GameObject searchTarget = default;
        for (int i = 0; i < targetObj_.transform.childCount; i++)
        {
            searchTarget = targetObj_.transform.GetChild(i).gameObject;
            if (searchTarget.name.Equals(objName_))
            {
                searchResult = searchTarget;
                return searchResult;
            }
            else
            {
                searchResult = FindChildObj(searchTarget, objName_);

                // 방어로직
                if (searchResult == null || searchResult == default) { /* Pass */ }
                else { return searchResult; }
            }
        }       // loop

        return searchResult;
    }       // FindChildObj()

    //! 새로운 오브젝트를 만들어서 컴포넌트를 리턴하는 함수
    public static T CreateObj<T>(string objName) where T : Component
    {
        GameObject newObj = new GameObject(objName);
        return newObj.AddComponent<T>();
    }       // CreateObj()


    //! 현재 활성화 되어 있는 씬을 찾아주는 함수
    public static Scene GetActiveScene()
    {
        Scene activeScene_ = SceneManager.GetActiveScene();
        return activeScene_;
    }       // GetActiveScene()
}

