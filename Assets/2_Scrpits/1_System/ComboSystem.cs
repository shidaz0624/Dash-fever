using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboSystem : SystemBase {

    private bool    m_isComboSystemEnable   = false;
    private int     m_iComboValue           = 0; //連擊次數
    private float   m_fComboContinueTime    = 3f;//連擊有效時間
    private float   m_fComboLastTime        = 0f;//連擊有效剩餘時間

    public CanvasGroup m_Panel  = null;         //Combo系統的主要Canvas，用來改變整個Alpha用
    public Image m_TimerBG      = null;         
    public Image m_TimerFront   = null;
    public Text  m_ComboText    = null;
    public Animator m_Animator = null;

    public override void Init()
    {
        Reset();
        SetComboSysEnable(false);
    }

    public float ComboContinueTime
    {
        get{ return m_fComboContinueTime;  }
        set{ m_fComboContinueTime = value; }
    }

    public void UpdateComboByPlusValue(int _iValue)
    {
        m_iComboValue += _iValue;
        RefreshTimer();
        CheckSystemEnable();
        m_Animator.CrossFade("UpdateComboValue" , 0);
//        m_Animator.Play("UpdateComboValue");
    }

    private void RefreshTimer()
    {
        m_fComboLastTime = m_fComboContinueTime;
    }

    public override void SysUpdate()
    {
        //若是關閉狀態則直接return，不做其他事項
        if (!m_isComboSystemEnable) return;

        //開始倒數時間
        UpdateTimer();

        //更新UI
        UpdateTimerGUI();
        UpdateComboGUI();
        SetAlpha();

        //檢查連擊系統是否關閉
        CheckSystemEnable();
    }

    /// <summary>
    /// 檢查連擊系統關閉條件
    /// </summary>
    private void CheckSystemEnable()
    {
        if (m_fComboLastTime <= 0f )
        {
            if (m_isComboSystemEnable)
                SetComboSysEnable(false);
        }
        else 
        {
            if (!m_isComboSystemEnable)
                SetComboSysEnable(true);
        }
    }

    /// <summary>
    /// 更新倒數時間
    /// </summary>
    private void UpdateTimer()
    {
        m_fComboLastTime -= Time.deltaTime;
    }

    /// <summary>
    /// 更新時間條畫面
    /// </summary>
    private void UpdateTimerGUI()
    {
        m_TimerFront.fillAmount = m_fComboLastTime / m_fComboContinueTime;   
    }

    private void SetComboSysEnable(bool _isEnable)
    {
        m_TimerBG.enabled = _isEnable;
        m_TimerFront.enabled = _isEnable;
        m_ComboText.enabled = _isEnable;

        m_isComboSystemEnable = _isEnable;

        if (!_isEnable)
            Reset();

    }

    private void SetAlpha()
    {
        float _alpha = m_fComboLastTime / m_fComboContinueTime;
        m_Panel.alpha = _alpha;
    }

    private void Reset()
    {
        m_iComboValue = 0;
        m_fComboLastTime = m_fComboContinueTime;
    }

    /// <summary>
    /// 更新連擊數畫面
    /// </summary>
    private void UpdateComboGUI()
    {
        m_ComboText.text = m_iComboValue.ToString();
       
    }        


	
}
