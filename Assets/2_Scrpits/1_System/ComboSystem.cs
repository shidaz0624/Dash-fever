using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboSystem : SystemBase {

    private bool    m_isComboSystemEnable   = false; //Combo 系統開關
    private int     m_iComboValue           = 0;  //連擊次數
    private float   m_fComboContinueTime    = 3f; //連擊有效時間
    private float   m_fComboLastTime        = 0f; //連擊有效剩餘時間
    private float   m_fTextScaleSpeed       = 20f;//字的縮小動畫速度
    private float   m_fTextScale            = 0f; //字的 Vector 值
    private Vector3 m_TextScaleV3           = new Vector3(0f,0f,0f); //字的Scale Vector3，Catch起來，不用每次都宣告

    public CanvasGroup m_Panel  = null;         //Combo系統的主要Canvas，用來改變整個Alpha用
    public Image m_TimerBG      = null;         //Fill 底
    public Image m_TimerFront   = null;         //Fill 前
    public Text  m_ComboText    = null;         //Combo 字

    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init()
    {
        Reset();
        SetComboSysEnable(false);
    }

    /// <summary>
    /// 取得Combo的時間
    /// </summary>
    public float ComboContinueTime
    {
        get{ return m_fComboContinueTime;  }
        set{ m_fComboContinueTime = value; }
    }

    /// <summary>
    /// 更新Combo值 By 加的值
    /// </summary>
    public void UpdateComboByPlusValue(int _iValue)
    {
        m_iComboValue += _iValue;
        RefreshTimer();
        ResetTextScale();
        CheckSystemEnable();
    }

    /// <summary>
    /// 重置計時器
    /// </summary>
    private void RefreshTimer()
    {
        m_fComboLastTime = m_fComboContinueTime;
    }

    /// <summary>
    /// 系統 Update
    /// </summary>
    public override void SysUpdate()
    {
        //若是關閉狀態則直接return，不做其他事項
        if (!m_isComboSystemEnable) return;

        //開始倒數時間
        UpdateTimer();

        //更新UI
        UpdateTimerGUI();
        UpdateComboGUI();
        UpdateTextScale();
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

    #region Text Animation
    private void ResetTextScale()
    {
        m_fTextScale = 4f;
    }
    private void UpdateTextScale()
    {
        m_fTextScale = Mathf.Clamp(m_fTextScale -= Time.deltaTime * m_fTextScaleSpeed , 1f , 3f);
        m_TextScaleV3 = new Vector3(m_fTextScale , m_fTextScale , 1f);

        m_ComboText.transform.localScale = m_TextScaleV3;
    }
    #endregion

    /// <summary>
    /// 設置系統開關
    /// </summary>
    private void SetComboSysEnable(bool _isEnable)
    {
        m_TimerBG.enabled = _isEnable;
        m_TimerFront.enabled = _isEnable;
        m_ComboText.enabled = _isEnable;

        m_isComboSystemEnable = _isEnable;

        if (!_isEnable)
            Reset();

    }

    /// <summary>
    /// 設置Alpha
    /// </summary>
    private void SetAlpha()
    {
        float _alpha = m_fComboLastTime / m_fComboContinueTime;
        m_Panel.alpha = _alpha;
    }

    /// <summary>
    /// 重置系統數值
    /// </summary>
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
