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

    public HeroUI m_HeroUI = new HeroUI();

    public void InitUISystem()
    {
    
    }

    public void UpdateHeroHP(int _iHP)
    {
        m_HeroUI.m_HPLabel.text = "HP : " + _iHP.ToString();
    }

    public void UpdateHeroAP(int _iAP)
    {
        m_HeroUI.m_APLabel.text = "AP : " + _iAP.ToString();
    }

    public void UpdateHeroGUI(int _iHP , int _iAP)
    {
        m_HeroUI.m_HPLabel.text = "HP : " + _iHP.ToString();
        m_HeroUI.m_APLabel.text = "AP : " + _iAP.ToString();
    }


    private void Start()
    {
        InvokeRepeating("CreatEnemy" , 1f , 1f);
    }
    public GameObject m_Enemy = null;
    public void CreatEnemy()
    {
        Vector3 _v3 = new Vector3( UnityEngine.Random.Range( -30 , 30  ) , 0 , 0 );
        GameObject _e = Instantiate( m_Enemy );

        _e.transform.localPosition = _v3;
    }


}
