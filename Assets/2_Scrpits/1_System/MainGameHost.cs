using UnityEngine;
using System.Collections;

public class MainGameHost : MonoBehaviour {

    private static MainGameHost m_MonoRef   = null;
    public  static MainGameHost MoroRef     {get{return m_MonoRef;}}

    public MainGUISystem m_GUIMainSystem = null;

    void Awake()
    {
        m_MonoRef = this;
        this.InitAllSystem();
    }
	
	void Update () 
    {
	    
	}

    void OnDestory()
    {
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

    #endregion


}
