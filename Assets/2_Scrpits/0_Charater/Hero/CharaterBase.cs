using UnityEngine;
using System.Collections;


public class CharaterBase : MonoBehaviour {

    [Header("角色參數class")] 
    public CharaterParameter m_CharaterParameter = new CharaterParameter();
    [Header("角色自動回覆class")]
    public RecoverParameter  m_RecoverParameter  = new RecoverParameter();
    [Header("地板class")] 
    public GroundCase m_Ground = new GroundCase();
    //角色鋼體
    protected Rigidbody2D m_Rigidbody2D = null;
    public Rigidbody2D GetRigidbody2D{ get{ return m_Rigidbody2D; } }
    [Header("角色狀態機")]
    public Animator m_Animator = null;
    //角色攻擊碰撞的Flag及參數
    public HitCase m_HitCase = new HitCase();
    [Header("效果class")]
    public EffectCase     m_EffectCase = null;
    public DefenseCase m_DefenceCase = new DefenseCase();
    private int m_iFlip = 1; // -1 , 1
    private SpriteRenderer m_SpriteRenderer = null;

    protected Sprite GetNowSprite
    { get{ return m_SpriteRenderer.sprite; } }

    /// <summary>
    /// Get Rigidbody2D , Get SpriteRenderer
    /// </summary>
    protected virtual void Start () 
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        CharaterMaintainer.Mono.AddCharater(this);
	}
		
    public virtual void MonoUpdate () 
    {
        //判斷角色是否在地板上
        m_Ground.m_isGround = m_Ground.IsGround;

        //將是否在地板資訊傳給Animator
        if (m_Animator != null)
            m_Animator.SetBool("isGround" , m_Ground.m_isGround);

        //若角色被設定死亡，則播放死亡效果
        if (m_CharaterParameter.GetIsDeath)
            DeathEffect();

	}

    protected void Velocity(Vector2 _VectorV2)
    {
        if (m_Rigidbody2D != null)
            m_Rigidbody2D.velocity = _VectorV2;
        else
            Debug.LogWarning("Rigibody2D is null",this);
    }

    public void PlayDeathEffect()
    {        
        m_CharaterParameter.SetIsDeath = true;
        if (m_EffectCase.m_DeathParticle != null)
            m_EffectCase.m_DeathParticle.Play();
        else
            Debug.LogWarning("!!m_DeathParticle is null!!",this);
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
        if (_a < 0 && !m_EffectCase.m_DeathParticle.isPlaying)
        {
            this.CharaterDeactive();
            _c.a = 1;
            m_SpriteRenderer.material.color = _c;
        }
    }

    private void CharaterDeactive ()
    {
        gameObject.SetActive(false);
    }
        
    /// <summary>
    /// 角色移動
    /// </summary>   
    protected void Move(Vector2 _MoveV2)
    {
        transform.Translate(_MoveV2);
    }

    /// <summary>
    /// 角色跳躍
    /// </summary>
    protected void Jump()
    {
        m_Rigidbody2D.AddForce( new Vector2( 0 , m_CharaterParameter.GetJumpPower));
    }

    /// <summary>
    /// 取得角色面向 > 0 = 面右 ， < 0 = 面左
    /// </summary>
    public int GetFlip
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
    /// 設置攻擊參數，True = 直接觸發 ， False = 直接取消目前的攻擊
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

    public void ProcessDefence( DamageClass _DamageClass )
    {
        if ( _DamageClass.m_ForceV2 != Vector2.zero ) 
        {
            m_Rigidbody2D.velocity = _DamageClass.m_ForceV2;
        }
    }

    /// <summary>
    /// 角色生命 + Delta
    /// </summary>
    public void ProcessHealthPoint(int _iDelta)
    {
        m_CharaterParameter.SetHPByDelta ( _iDelta );
    }

    public void ProcessGetDamageEffect(int _iSide)
    {
        if (_iSide < 0)
        {
            if (m_EffectCase.m_RightSideHitParticle != null)
                m_EffectCase.m_RightSideHitParticle.Play();
        }
        else
        {
            if (m_EffectCase.m_LeftSideHitParticle != null)
                m_EffectCase.m_LeftSideHitParticle.Play();
        }
    }   
        
    /// <summary>
    /// 角色受到傷害後的動作
    /// </summary>
    public virtual void GetDamage( DamageClass _Data )
    {
        ProcessGetDamageEffect( _Data.m_iSide );

        if (_Data.m_ForceV2 != Vector2.zero)
        {
            SetVelocity( _Data.m_ForceV2 );
        }            
    }

    public void SetCanHurt(bool _isCanHurt)
    {
        m_CharaterParameter.SetIsCanHurt = _isCanHurt;
    }

    /// <summary>
    /// 設定角色目前的速度.
    /// </summary>
    protected void SetVelocity( Vector2 _VectorV2 )
    {
        this.Velocity( _VectorV2 );
    }

    public virtual void OnTriggerEnter2D(Collider2D _Other)
    {
        if (m_HitCase.m_isEnabled == false ) return;
        //設置攻擊參數
        DamageClass _DamageData = new DamageClass();
        _DamageData.m_iDamage = m_HitCase.m_iDamage;
        _DamageData.m_iSide = GetFlip;
        if (m_Rigidbody2D != null)
            _DamageData.m_ForceV2 = m_Rigidbody2D.velocity / 10;
        CharaterMaintainer.Mono.PassDamageClassToCharater(this , _DamageData , _Other.gameObject );
    }

    #region Dash Event
    protected void OnDash(Vector2 _V2)
    {
        m_Rigidbody2D.velocity = _V2;
    }
   
    protected void OnDashStart()
    {
        if (m_Animator != null)
            m_Animator.SetBool("isDash" , true);
        if (m_EffectCase.m_CharacterSpriteAnimator != null)
        {
            m_EffectCase.m_CharacterSpriteAnimator.Play("DashStart");
        }
    }

    protected void OnDashFin()
    {
        if (m_Animator)
            m_Animator.SetBool("isDash" , false);

        //關閉傷害
        SetHitCase(false);

        if (m_EffectCase.m_CharacterSpriteAnimator != null)
        {
            m_EffectCase.m_CharacterSpriteAnimator.Play("DashEnd");
        }
    }
    #endregion
}