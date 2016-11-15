using UnityEngine;
using System.Collections;

public class MainGameHost : MonoBehaviour {

    private static MainGameHost m_MonoRef   = null;
    public  static MainGameHost MonoRef     {get{return m_MonoRef;}}

    public MainGUISystem m_GUIMainSystem = null;

    void Awake()
    {
        m_MonoRef = this;
        this.InitAllSystem();
    }
	
    /// <summary>
    /// 主要Update 迴圈
    /// </summary>
	void Update () 
    {
        //======== UI 系統 ========
        m_GUIMainSystem.SysUpdate();
        //========================
	}

    void OnDestory()
    {
        m_GUIMainSystem = null;
        Debug.Log("== MainGameHost OnDestory ==" );
    }

    private void InitAllSystem()
    {
        m_GUIMainSystem.InitUISys();
    }

    #region ============== GUIMainSystem ==============

    public void UpdateHeroHPAndAP(int _iHP , int _iAP)
    {
        m_GUIMainSystem.UpdateHeroGUI(_iHP , _iAP);
    }

    public void UpdateHeroHP(int _iHP)
    {
        m_GUIMainSystem.UpdateHeroHP(_iHP);
    }

    public void UpdateHeroAP(int _iAP)
    {
        m_GUIMainSystem.UpdateHeroAP(_iAP);
    }
    /// <summary>
    /// 更新Combo UI By 增加的值
    /// </summary>
    public void UpdateComboByPlusValue(int _iValue)
    {
        m_GUIMainSystem.m_ComboSys.UpdateComboByPlusValue(_iValue );
    }
    #endregion


}
