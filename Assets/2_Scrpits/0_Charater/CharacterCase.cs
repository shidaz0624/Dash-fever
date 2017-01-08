using UnityEngine;
using System.Collections;

/// <summary>
/// 地板
/// </summary>
[System.Serializable]
public class GroundCase
{
    public  Transform   m_GroundTag         = null; //角色偵測地板的射線起始點物件
    public  LayerMask   m_GroundLayerMask;          //地板圖層
    public  float       m_fGroundRay        = 0.2f;
    public  bool        m_isGround          = false;
    public bool IsGround
    {
        get {

            bool _isGround = Physics2D.OverlapCircle(m_GroundTag.position ,
                m_fGroundRay ,
                m_GroundLayerMask);
            return _isGround;
        }

    }
}
[System.Serializable]
public class DefenseCase
{
    private bool m_isDefence    = false;
    public int m_iDefecnePoint  = 0;
    public ParticleSystem m_DefenceParticle = null;

    public void SetDefence(bool _isEnable , int _iDefencePoint = -1)
    {
        IsDefence = _isEnable;
        DefencePoint = _iDefencePoint;
        SetDefenceEffect(_isEnable);
    }       

    public bool IsDefence
    {
        get{ return m_isDefence; }
        set{ m_isDefence = value; }
    }
    public int DefencePoint
    {
        get{ return m_iDefecnePoint; }
        set{ m_iDefecnePoint = value; }
    }
    public void SetDefenceEffect(bool _isEnable)
    {
        if (_isEnable && m_DefenceParticle.isStopped)
            m_DefenceParticle.Play();
        else if (!_isEnable && m_DefenceParticle.isPlaying)
            m_DefenceParticle.Stop();
    }
}

/// <summary>
/// 碰撞
/// </summary>
public class HitCase
{
    public bool     m_isEnabled = false;
    public int      m_iDamage    = 0;
    public Vector2  m_ForceV2   = Vector2.zero;
}

/// <summary>
/// 角色參數
/// </summary>
[System.Serializable]
public class CharaterParameter
{    
    [SerializeField] private float   m_fHealthPoint  = 0f;
    [SerializeField] private float   m_fActionPoint  = 0f;
    [SerializeField] private int     m_iJumpPower    = 0;
    [SerializeField] private float   m_fViewDistance = 0f;
    [SerializeField] private float   m_fMoveSpeed    = 0f;
    private bool    m_isDeath       = false;
    private bool    m_isCanHurt     = true;


    public  bool    SetIsCanHurt    {set{m_isCanHurt = value;     }}
    public  bool    SetIsDeath      {set{m_isDeath = value;     }}

    public  bool    GetCanHurt      {get{return m_isCanHurt;      }}
    public  bool    GetIsDeath      {get{return m_isDeath;      }}
    public  float   GetHealthPoint  {get{return m_fHealthPoint; }}
    public  float   GetActionPoint  {get{return m_fActionPoint; }}
    public  int     GetJumpPower    {get{return m_iJumpPower;   }}
    public  float   GetViewDistance {get{return m_fViewDistance;}}
    public  float   GetMoveSpeed    {get{return m_fMoveSpeed;   }}

    public void SetHPAndAP(float _fHP , float _fAP)
    {
        m_fHealthPoint = _fHP;
        m_fActionPoint = _fAP;
    }

    public void SetHPAndAPByDelta( float _fHPDelta , float _fAPDelta )
    {
        m_fHealthPoint += _fHPDelta;
        m_fActionPoint += _fAPDelta;
    }

    public void SetHP(float _fHP)
    {
        m_fHealthPoint = _fHP;
    }
    public void SetHPByDelta(float _fHPDelta)
    {
        m_fHealthPoint +=_fHPDelta;
    }
    public void SetAP(float _fAP)
    {
        m_fActionPoint = _fAP;
    }
    public void SetAPByDelta(float _fAPDelta)
    {
        m_fActionPoint += _fAPDelta;
    }
}    

/// <summary>
/// 角色恢復力
/// </summary>
[System.Serializable]
public class RecoverParameter
{   
    public  float   m_fHPRecoverSpeed = 0f; // HP恢復速度
    public  float   m_fAPRecoverSpeed = 0f; // AP恢復速度

    public float GetHPRecoverValue
    {
        get
        {
            float   _value = m_fHPRecoverSpeed * Time.deltaTime;
            return  _value;
        }
    }

    public float GetAPRecoverValue
    {
        get
        {
            float   _value = m_fAPRecoverSpeed * Time.deltaTime;
            return  _value;
        }
    }
}

[System.Serializable]
public class EffectCase
{
    public ParticleSystem m_DeathParticle  = null;
    public ParticleSystem m_RightSideHitParticle = null;
    public ParticleSystem m_LeftSideHitParticle = null;
    public Animator m_CharacterSpriteAnimator = null;  
}
