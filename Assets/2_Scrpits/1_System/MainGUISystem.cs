using UnityEngine;
using System.Collections;

public class MainGUISystem : MonoBehaviour {

    private static MainGUISystem MonoRef = null;
    public static MainGUISystem m_MonoRef{ get{ return MonoRef;} }

    public PlayerParameterGUI m_PlayerParameterGUI = null;
    public ComboSystem m_ComboSys = null;

    private void Awake()
    {
        InitUISys();
    }

    public void InitUISys()
    {
        MonoRef = this;
        m_ComboSys.Init();
    }

    private void Update()
    {
        m_ComboSys.SysUpdate();
    }
    #region Hero   
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

    #region Combo sys
    public void UpdateComboByPlusValue(int _iValue)
    {
        m_ComboSys.UpdateComboByPlusValue(_iValue );
    }
    #endregion
}
