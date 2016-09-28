using UnityEngine;
using System.Collections;

/// <summary>
/// Dash用的Data Class
/// </summary>
[System.Serializable]
public class  DashCase
{
    public  Dash    m_DashClass     = new Dash();   //Dash主要作用Class
    public  Vector2 m_DashForceV2   = Vector2.zero; //Dash時的力
    public  float   m_fTime         = 0.5f;         //Dash的作用時間
        
}

public class Dash 
{
    public delegate void CharaterDash(Vector2 _V2); //DASH中事件，通知註冊者此次Dash的量
    public CharaterDash OnDash;

    public delegate void CharaterDashStart();   //Dash開始事件
    public CharaterDashStart OnDashStart;

    public delegate void CharaterDashFin();     //Dash結束事件
    public CharaterDashFin OnDashFin;

    private Vector2 m_DashV2 = Vector2.zero;    //Dash作用力
    private float   m_fTime  = 0f;              //Dash作用時間
    private bool    m_isFin = true;             //是否結束Flag

    private float m_CurveTime = 0;              //曲線時間 (經過時間 / 總作用時間)
    private Vector2 m_DeltaV2 = Vector2.zero;   //每次的位移量，作為Catch用

    public bool GetIsFin
    {
        get{ return m_isFin; }
    }

    /// <summary>
    /// 設置Dash的力與作用時間
    /// </summary>
    public void SetDashValue(Vector2 _V2 , float _fTime)
    {        
        m_DashV2 = _V2;
        m_fTime  = _fTime;
        m_CurveTime = 0;
        SetFin(false);
    }

    /// <summary>
    /// 觸發Dash開始或結束事件
    /// </summary>
    private void SetFin(bool _isFin)
    {
        m_isFin = _isFin;
        if (m_isFin)
        {
            //Dash結束
            //通知註冊者把剩餘的作用力設成(0,0)，等同於取消
            Vector2 _v2 = Vector2.zero;
            if (OnDash != null)                
                OnDash(_v2);
            //通知註冊者Dash結束
            if (OnDashFin != null)
                OnDashFin();
        }
        else
        {
            //通知註冊者Dash開始
            if (OnDashStart != null)
                OnDashStart();
        }
    }
        
    public void Update()
    {
        //若是結束狀態就直接Return;
        if (m_isFin) return;

        //計算時間0~1 (經過時間 / 總作用時間)
        m_CurveTime += Time.deltaTime / m_fTime;
        //從 CurveDataCenter 取得0~1曲線中 m_Time 的值
        float _fValue =  CurveDataCenter.MonoRef.m_CurveZeroToOne.Evaluate(m_CurveTime);

        //計算這次的移動量
        m_DeltaV2 = m_DashV2 * _fValue;

        //若m_Time >= 1的話則結束
        if (m_CurveTime >= 1)
        {
            m_CurveTime = 0 ;    //恢復預設值
            SetFin(true);   //設置已經結束
        }
            
        if (OnDash != null)     
            OnDash(m_DeltaV2);  //通知註冊者這次的位移量
    }
	
}
