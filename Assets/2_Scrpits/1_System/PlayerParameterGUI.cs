using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerParameterGUI : SystemBase {

    [Serializable]
    public class HeroUI
    {
        public  Text    m_HPLabel   = null;
        public  Text    m_APLabel   = null;
        public  Image   m_HPBar     = null;
        public  Image   m_APBar     = null;
        public  HeroUI()
        {
            
        }
    }

    public HeroUI m_HeroUI = new HeroUI();

    public override void Init ()
    {
    }


    public override void SysUpdate ()
    {
    }

    public void UpdateHeroHP(int _iHP)
    {
        m_HeroUI.m_HPLabel.text = "HP : " + _iHP.ToString();
        m_HeroUI.m_HPBar.fillAmount = (float)_iHP / 5f;
    }

    public void UpdateHeroAP(int _iAP)
    {
        m_HeroUI.m_APLabel.text = "AP : " + _iAP.ToString();
        m_HeroUI.m_APBar.fillAmount = (float)_iAP / 500f;
    }

    public void UpdateHeroGUI(int _iHP , int _iAP)
    {
        m_HeroUI.m_HPLabel.text = "HP : " + _iHP.ToString();
        m_HeroUI.m_APLabel.text = "AP : " + _iAP.ToString();
        m_HeroUI.m_HPBar.fillAmount = (float)_iHP / 5f;
        m_HeroUI.m_APBar.fillAmount = (float)_iAP / 500f;
    }


//    private void Start()
//    {
////        InvokeRepeating("CreatEnemy" , 1f , 2f);
//    }
//    public GameObject m_Enemy = null;
//    public void CreatEnemy()
//    {
//        Vector3 _v3 = new Vector3( UnityEngine.Random.Range( -30 , 30  ) , 0 , 0 );
//        GameObject _e = Instantiate( m_Enemy );
//
//        _e.transform.localPosition = _v3;
//    }
//
//
}
