using UnityEngine;
using System.Collections;

public class MainGameHost : MonoBehaviour {

    private static MainGameHost m_MonoRef   = null;
    public  static MainGameHost MoroRef     {get{return m_MonoRef;}}

    public GUIMonoSystem m_GUIMonoSystem = null;

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
        m_GUIMonoSystem.InitUISystem();
    }

    #region ============== GUIMonoSystem ==============

    public void UpdateHeroHPAndAP(int _iHP , int _iAP)
    {
        m_GUIMonoSystem.UpdateHeroGUI(_iHP , _iAP);
    }

    public void UpdateHeroHP(int _iHP)
    {
        m_GUIMonoSystem.UpdateHeroHP(_iHP);
    }

    public void UpdateHeroAP(int _iAP)
    {
        m_GUIMonoSystem.UpdateHeroAP(_iAP);
    }        

    #endregion


}
