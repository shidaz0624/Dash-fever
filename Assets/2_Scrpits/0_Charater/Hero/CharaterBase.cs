using UnityEngine;
using System.Collections;

[System.Serializable]
public class GroundCase
{
    public  Transform   m_GroundTag         = null; //角色偵測地板的射線起始點物件
    public  LayerMask   m_GroundLayerMask;              //地板圖層
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
public class CharaterParameter
{
    public  float   m_fHealthPoint  = 0f;
    public  float   m_fActionPoint  = 0f;
    public  bool    m_isDeath       = false;

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

[System.Serializable]
public class RecoverParameter
{   
    public  float   m_fHPRecoverSpeed = 0f; // 恢復速度
    public  float   m_fAPRecoverSpeed = 0f;

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

public class CharaterBase : MonoBehaviour {

    //角色參數class
    public CharaterParameter m_CharaterParameter = new CharaterParameter();
    public RecoverParameter  m_RecoverParameter  = new RecoverParameter();
    //地板class
    public GroundCase m_Ground = new GroundCase();
    //角色鋼體
    protected Rigidbody2D m_Rigidbody2D = null;
    //角色狀態機
    public Animator m_Animator = null;
    //角色攻擊class
    public HitCase m_HitCase = new HitCase();
    public ParticleSystem  m_DeathParticle  = null;
    public ParticleSystem m_RightSideHitParticle = null;
    public ParticleSystem m_LeftSideHitParticle = null;

    private int m_iFlip = 1; // -1 , 1
    private SpriteRenderer m_SpriteRenderer = null;


    protected virtual void Start () {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_Animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
    protected virtual void Update () {

        m_Ground.m_isGround = m_Ground.IsGround;
        if (m_Animator != null)
            m_Animator.SetBool("isGround" , m_Ground.m_isGround);

        if (m_CharaterParameter.m_isDeath)
        {
            DeathEffect();
        }

	}

    private void StartDeathEffect()
    {        
        m_DeathParticle.Play();
//        m_DeathParticle.externalForces = new ()
        m_CharaterParameter.m_isDeath = true;
    }

    private void DeathEffect()
    {
        //角色漸淡
        Color _c = m_SpriteRenderer.material.color;
        float _a = _c.a;
        _a -= Time.deltaTime * 2f;
        _c.a = _a;
        m_SpriteRenderer.material.color = _c;

        //角色死亡效果播放完畢
        if (_a < 0 && !m_DeathParticle.isPlaying)
        {
            gameObject.SetActive(false);
            _c.a = 1;
            m_SpriteRenderer.material.color = _c;
        }
    }

    private CharacterController m_CharacterController;
    /// <summary>
    /// 角色移動
    /// </summary>   
    protected void Move(Vector2 _MoveV2)
    {
        transform.Translate(_MoveV2);
    }

    /// <summary>
    /// 取得角色面向
    /// </summary>
    protected int GetFlip
    {
        get
        {return m_iFlip;}
    }

    /// <summary>
    /// 觸發角色左右翻轉
    /// </summary>
    protected int FlipTrigger()
    {
        m_iFlip = (m_iFlip == 1)? -1 : 1;
        m_SpriteRenderer.flipX = (m_iFlip == 1)? false : true;
        return m_iFlip;
    }

    /// <summary>
    /// 設置攻擊參數
    /// </summary>   
    protected void SetHitCase( bool _isEnabled , int _iDamage = 0, Vector2  _ForceV2 = default(Vector2))
    {
        m_HitCase.m_isEnabled = _isEnabled;
        if (_isEnabled)
        {
            m_HitCase.m_iDamage = _iDamage;
            m_HitCase.m_ForceV2 = _ForceV2;
        }
    }

    /// <summary>
    /// 處理角色是否翻轉
    /// </summary>
    protected void ProcessFlip(float _fHorizontal)
    {
        if (_fHorizontal != 0)
        {
            if (_fHorizontal > 0 && GetFlip < 0 )
            {
                FlipTrigger();
            }
            else if (_fHorizontal < 0 && GetFlip > 0)
            {
                FlipTrigger();
            }
        }
    }

    /// <summary>
    /// 角色生命 + Delta
    /// </summary>
    protected void ProcessHealthPoint(int _iDelta)
    {
        m_CharaterParameter.m_fHealthPoint += _iDelta;
        if(m_CharaterParameter.m_fHealthPoint <= 0)
        {
            //To do Dead
            StartDeathEffect();
        }
    }

    protected void ProcessGetDamageEffect(int _iSide)
    {
        if (_iSide < 0)
        {
            if (m_RightSideHitParticle != null)
                m_RightSideHitParticle.Play();
        }
        else
        {
            if (m_LeftSideHitParticle != null)
                m_LeftSideHitParticle.Play();
        }
    }
        
    /// <summary>
    /// 角色受到傷害
    /// </summary>
    public virtual void GetDamage( int _iDamage , int _iSide , Vector2 _ForceV2 = default(Vector2) )
    {
        ProcessHealthPoint( - _iDamage );
        ProcessGetDamageEffect(_iSide);

        if ( _ForceV2 != Vector2.zero ) 
            m_Rigidbody2D.velocity = _ForceV2;
    }

    #region Dash Event
    protected void OnDash(Vector2 _V2)
    {
        m_Rigidbody2D.velocity = _V2;
    }

    protected void OnDashFin()
    {
        SetHitCase(false);
    }
    #endregion
}
