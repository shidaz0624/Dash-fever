using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GUIMonoSystem : MonoBehaviour {

    [Serializable]
    public class HeroUI
    {
        public  Text    m_HPLabel   = null;
        public  Text    m_APLabel   = null;
        public HeroUI()
        {
            
        }
    }

    public HeroUI m_HeroUI = null;

    private void Awake()
    {
        InitUISystem();
    }

    private void InitUISystem()
    {
        m_HeroUI = new HeroUI();
    }


    public void SetHealthPoint(int _iValue)
    {
        
    }

}
