using UnityEngine;
using System.Collections;

[System.Serializable]
public class  DashCase
{
    public  Dash    m_DashClass     = new Dash();
    public  Vector2 m_DashForceV2   = Vector2.zero;
    public  float   m_fTime         = 0.5f;
        
}

public class Dash 
{
    public delegate void CharaterDash(Vector2 _V2);
    public CharaterDash OnDash;

    public delegate void CharaterDashStart();
    public CharaterDashStart OnDashStart;

    public delegate void CharaterDashFin();
    public CharaterDashFin OnDashFin;

    private Vector2 m_DashV2 = Vector2.zero;
    private float   m_fTime  = 0f;
    private bool    m_isFin = true;

    private float m_Time = 0;

    public bool GetIsFin
    {
        get{ return m_isFin; }
    }

    public void SetDashValue(Vector2 _V2 , float _fTime)
    {        
        m_DashV2 = _V2;
        m_fTime  = _fTime;
        m_Time = 0;
        SetFin(false);
    }

    private void SetFin(bool _isFin)
    {
        m_isFin = _isFin;
        if (m_isFin)
        {
            Vector2 _v2 = Vector2.zero;
            if (OnDash != null)
                OnDash(_v2);
            if (OnDashFin != null)
                OnDashFin();
        }
        else
        {
            if (OnDashStart != null)
                OnDashStart();
        }
    }

    private void CalculateValue()
    {
    }



    public void Update()
    {
        if (m_isFin) return;

        m_Time += Time.deltaTime / m_fTime;
        float _fValue =  CurveDataCenter.MonoRef.m_CurveZeroToOne.Evaluate(m_Time);

        Vector2 _v2 = m_DashV2 * _fValue;

        if (m_Time >= 1)
        {
            m_Time = 0 ;
            SetFin(true);
        }
            
        if (OnDash != null)
            OnDash(_v2);
    }
	
}
