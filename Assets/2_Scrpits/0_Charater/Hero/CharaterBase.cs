using UnityEngine;
using System.Collections;


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

    protected virtual void Start () {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
//        m_Animator = GetComponentInChildren<Animator>();
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

    private CharacterController m_CharacterController;
    /// <summary>
    /// 角色移動
    /// </summary>   
    protected void Move(Vector2 _MoveV2)
    {
        transform.Translate(_MoveV2);
    }

    protected void Jump()
    {
        m_Rigidbody2D.AddForce( new Vector2( 0 , m_CharaterParameter.m_iJumpPower));
    }

    /// <summary>
    /// 取得角色面向
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
    public virtual void GetDamage( int _iDamage , int _iSide , Vector2 _ForceV2 = default(Vector2) )
    {
        if (m_CharaterParameter.m_isDeath) return;

        //當角色防禦且面對攻擊來的方向
        if (m_DefenceCase.IsDefence && (GetFlip != _iSide))
        {
            //Do 防禦成功
            if ( _ForceV2 != Vector2.zero ) 
                m_Rigidbody2D.velocity = _ForceV2;
        }
        else
        {
            //Do 受到傷害
            ProcessHealthPoint( - _iDamage );
            ProcessGetDamageEffect(_iSide);

            if ( _ForceV2 != Vector2.zero )
            {
                if ( m_Rigidbody2D != null)
                    m_Rigidbody2D.velocity = _ForceV2;
                else if (GetComponent<Door>() == null)
                {
                    Debug.LogError(gameObject.name + "  Don't have rigibody2D!!!");
                }
            }
        }
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

        SetHitCase(false);
        if (m_EffectCase.m_CharacterSpriteAnimator != null)
        {
            m_EffectCase.m_CharacterSpriteAnimator.Play("DashEnd");
        }
    }
    #endregion
}
