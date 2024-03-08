using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class T_BindCollection : MonoBehaviour
{
    #region =============== Instance =======================
    public static T_BindCollection Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;

    }
    #endregion
    #region ================ variables =============================

    [Serializable]
    public struct DebugBindCollection
    {
        public string BindName;
        public T_BindBase Bind;
    }
    public List<DebugBindCollection> DebugBindCollections;

    public Dictionary<string, T_BindBase> FriendBindDic = new();

    #region =============== Private ===========================



    #endregion
    #region ===================== Public =================

    public event Action Event_ActivateBindEffect;




    #endregion
    #endregion ==========================================
    private void Start()
    {
        var bindsArray = GetComponentsInChildren<T_BindBase>();
        foreach (var bind in bindsArray)
        {
            DebugBindCollection bindToCollection = new()
            {
                BindName = bind._BindName,
                Bind = bind
            };
            DebugBindCollections.Add(bindToCollection);

            FriendBindDic.Add(bind._BindName, bind);
        }


        //* update Friend units bind info

        //* get all BindInfo from Friend units
        List<T_UnitStats> friendList = T_LevelManager.Instance.G_GetFriendList();
        List<BindStruct> bindInfoList = new();
        foreach (T_UnitStats unit in friendList)
        {
            bindInfoList.Add(unit.G_BindInfoA());
        }

        //* Compare each BindInfo to BindDic to update BindLeve
        foreach (var bindInfo in bindInfoList)
        {
            if (FriendBindDic.TryGetValue(bindInfo._BindName, out T_BindBase bind))
            {
                bind._BindLevel += bindInfo._BindLevel;
                Debug.Log($"{bind},lv: {bind._BindLevel}");
            }
            else
            {
                Debug.LogError("??? Didn't find BIND ???");
                continue;
            }
        }

        //* Assign Bind back to each unit.
        foreach (T_UnitStats unit in friendList)
        {
            //* Find Bind from dictionary
            string unitBindAName = unit.G_BindInfoA()._BindName;
            if (FriendBindDic.TryGetValue(unitBindAName, out T_BindBase bind))
            {
                //* Assign Bind to unit
                unit.UnitBind.G_SetBindA(bind);
            }
            else
            {
                Debug.LogError("??? Didn't find BIND ???");
                continue;
            }
        }

        //* Debug: Update custom bind collection



    }




}
