using UnityEngine;
using System.Collections;

public class MainGUISystem : SystemBase {

//    private static MainGUISystem MonoRef = null;
//    public static MainGUISystem m_MonoRef{ get{ return MonoRef;} }

    public PlayerParameterGUI   m_PlayerParameterGUI    = null; //角色面板 UI
    public ComboSystem          m_ComboSys              = null; //Combo系統 UI



    public override void Init()
    {
//        MonoRef = this;
        m_ComboSys.Init();
    }

    public override void SysUpdate()
    {
        m_ComboSys.SysUpdate();
        m_PlayerParameterGUI.SysUpdate();
    }

    #region Hero Parameter GUI
    public void UpdateHeroGUI(int _iHP , int _iAP)
    {
        m_PlayerParameterGUI.UpdateHeroGUI(_iHP , _iAP);
    }

    public void UpdateHeroHP(int _iHP)
    {
        m_PlayerParameterGUI.UpdateHeroHP(_iHP);
    }

    public void UpdateHeroAP(int _iAP)
    {
        m_PlayerParameterGUI.UpdateHeroAP(_iAP);
    }
    #endregion

    #region Combo sys GUI
    public void UpdateComboByPlusValue(int _iValue)
    {
        m_ComboSys.UpdateComboByPlusValue(_iValue );
    }
    #endregion
}
