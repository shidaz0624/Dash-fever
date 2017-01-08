using UnityEngine;
using System.Collections;

public class MainGameHost : MonoBehaviour {

    private static MainGameHost m_MonoRef   = null;
    public  static MainGameHost MonoRef     {get{return m_MonoRef;}}

    public CharaterMaintainer m_CharaterMaintainer = null;
    public MainGUISystem m_MainGUISystem = null;
    public StageSystem m_StageSystem = null;

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
        //======== 遊戲 系統 ========
        
        //

        //======== UI 系統 ========
        m_MainGUISystem.SysUpdate();
        //========================

        //======== 角色 系統 ========
        m_CharaterMaintainer.SysUpdate();
        //========================

	}

    void FixedUpdate () 
    {
        m_CharaterMaintainer.SysFixedUpdate();
    }

    void OnDestory()
    {
        Debug.Log("== MainGameHost OnDestory ==" , this );
        m_MainGUISystem = null;
    }

    private void InitAllSystem()
    {
        m_CharaterMaintainer.Init();
        m_MainGUISystem.Init();
    }

    #region ============== GUIMainSystem ==============

    public void UpdateHeroHPAndAP(int _iHP , int _iAP)
    {
        m_MainGUISystem.UpdateHeroGUI(_iHP , _iAP);
    }

    public void UpdateHeroHP(int _iHP)
    {
        m_MainGUISystem.UpdateHeroHP(_iHP);
    }

    public void UpdateHeroAP(int _iAP)
    {
        m_MainGUISystem.UpdateHeroAP(_iAP);
    }
    /// <summary>
    /// 更新Combo UI By 增加的值
    /// </summary>
    public void UpdateComboByPlusValue(int _iValue)
    {
        m_MainGUISystem.m_ComboSys.UpdateComboByPlusValue(_iValue );
    }
    #endregion


}
