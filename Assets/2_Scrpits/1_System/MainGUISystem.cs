using UnityEngine;
using System.Collections;

public class MainGUISystem : MonoBehaviour {

    public PlayerParameterGUI m_PlayerParameterGUI = null;

    public void InitUISystem()
    {
        
    }

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
}
