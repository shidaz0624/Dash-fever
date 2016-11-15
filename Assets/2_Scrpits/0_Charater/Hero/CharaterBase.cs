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
    {
        get
        {
            return m_SpriteRenderer.sprite;
        }
    }

    /// <summary>
    /// Get Rigidbody2D , Get SpriteRenderer
    /// </summary>
    protected virtual void Start () 
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}
		
    protected virtual void Update () 
    {
        //判斷角色是否在地板上
        m_Ground.m_isGround = m_Ground.IsGround;

        //將是否在地板資訊傳給Animator
        if (m_Animator != null)
            m_Animator.SetBool("isGround" , m_Ground.m_isGround);

        //若角色被設定死亡，則播放死亡效果
        if (m_CharaterParameter.m_isDeath)
            DeathEffect();

	}

    private void StartDeathEffect()
    {        
        m_CharaterParameter.m_isDeath = true;
        m_EffectCase.m_DeathParticle.Play();
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
            gameObject.SetActive(false);
            _c.a = 1;
            m_SpriteRenderer.material.color = _c;
        }
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
        m_Rigidbody2D.AddForce( new Vector2( 0 , m_CharaterParameter.m_iJumpPower));
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

    protected void ProcessDefence()
    {
        
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
    /// 角色受到傷害
    /// </summary>
    public virtual void GetDamage( DamageClass _Data )
    {
        if (m_CharaterParameter.m_isDeath) return;

        //當角色防禦且面對攻擊來的方向
        if (m_DefenceCase.IsDefence && (GetFlip != _Data.m_iSide))
        {
            //Do 防禦成功
            if ( _Data.m_ForceV2 != Vector2.zero ) 
                m_Rigidbody2D.velocity = _Data.m_ForceV2;
        }
        else
        {
            //Do 受到傷害
            ProcessHealthPoint( - _Data.m_iDamage );
            ProcessGetDamageEffect(_Data.m_iSide);

            CreateDamagePoint(transform , _Data.m_iDamage);

            if ( _Data.m_ForceV2 != Vector2.zero )
            {
                if ( m_Rigidbody2D != null)
                    m_Rigidbody2D.velocity = _Data.m_ForceV2;
                else if (GetComponent<Door>() == null)
                {
                    Debug.LogError(gameObject.name + "  Don't have rigibody2D!!!");
                }
            }
        }
    }

    /// <summary>
    /// 建立傷害值
    /// </summary>
    protected void CreateDamagePoint( Transform _Transform , int _iDamage )
    {
        ObjectPool.m_MonoRef.GetObject(ObjectPool.ObjectPoolID.DAMAGE_HUD).GetComponent<DamageHUD>().Play(_Transform , _iDamage.ToString());
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
