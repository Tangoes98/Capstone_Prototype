using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_SkillCollection : MonoBehaviour
{
    #region =============== Instance =======================
    public static T_SkillCollection Instance;
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

    #region ================= Private ========================

    [SerializeField] List<T_SkillBase> _skillList;

    Dictionary<int, T_SkillBase> _skillDic = new();


    #endregion
    #region ================== Public ========================
    public Dictionary<int, T_SkillBase> G_GetSkillDic() => _skillDic;

    #endregion

    private void Start()
    {
        var skills = GetComponentsInChildren<T_SkillBase>();

        for (int i = 0; i < skills.Length; i++)
        {
            _skillList.Add(skills[i]);
            _skillDic.Add(i, skills[i]);
        }
    }

}
