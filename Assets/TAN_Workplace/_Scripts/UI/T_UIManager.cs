using System;
using UnityEngine;
using UnityEngine.UI;

public class T_UIManager : MonoBehaviour
{
    #region =============== Instance =======================
    public static T_UIManager Instance;
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
    #region ================== Variables =================
    [SerializeField] Button _debug_startBattle_btn;



    #endregion
    #region ===================== Public =======================
    public event Action Event_BattleStart;





    #endregion
    #region =================== MonoBehaviour =================
    private void Start()
    {

        _debug_startBattle_btn.onClick.AddListener(() => Event_BattleStart?.Invoke());
    }
    private void Update()
    {

    }
    #endregion
    #region =================== Methods =========================













    #endregion
}
