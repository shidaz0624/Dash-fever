using UnityEngine;
using System.Collections;



public class Hero : CharaterBase {

    public DashCase m_Dash = new DashCase();

    public Animator m_CameraEffect = null;

    private float m_HoldTimer = 0f;
    public ParticleSystem m_HoldParticle = null;

    private float m_fOriginHorozontal = 0f; //原始橫向位移參數
    private float m_fHorizontal = 0f;

    #region MonoBehaviour
    protected void Awake()
    {
        m_Dash.m_DashClass.OnDash = OnDash;
        m_Dash.m_DashClass.OnDashStart = OnDashStart;
        m_Dash.m_DashClass.OnDashFin = OnDashFin;
    }

    protected override void  Start () 
    {       
        base.Start();
    }
        
    protected override void Update ()
    {
        base.Update();
        m_Dash.m_DashClass.Update();
        this.ProcessInput();

        //計算現在的HP及AP
        m_CharaterParameter.SetHPAndAPByDelta
        ( m_RecoverParameter.GetHPRecoverValue , m_RecoverParameter.GetAPRecoverValue );
        //將HP及AP顯示在GUI上
        MainGameHost.MoroRef.UpdateHeroHPAndAP
            ( (int)m_CharaterParameter.m_fHealthPoint , (int)m_CharaterParameter.m_fActionPoint );
    }        

    private void OnDestory()
    {
        m_Dash.m_DashClass.OnDash = null;
        m_Dash.m_DashClass.OnDashStart = null;
        m_Dash.m_DashClass.OnDashFin = null;
    }
    #endregion	

    private void ProcessInput()
    {
        m_fOriginHorozontal = Input.GetAxis("Horizontal");
        m_fHorizontal = m_fOriginHorozontal * Time.deltaTime * 15;

        base.ProcessFlip(m_fHorizontal);

        bool _isKeyDefence = Input.GetKey(KeyCode.LeftShift);

        if (_isKeyDefence)
        {
            m_DefenceCase.SetDefence( true , 0 );
        }
        else
        {
            m_DefenceCase.SetDefence(false);
        }

        if (_isKeyDefence)
        {
            m_fHorizontal = 0;
            m_fOriginHorozontal = 0;
        }
            
        Move(new Vector2(m_fHorizontal , 0));
        
        if ( m_Ground.IsGround )
            m_Animator.SetFloat( "fHorizontal" , Mathf.Abs( m_fOriginHorozontal ));
        else
            m_Animator.SetFloat( "fHorizontal" , 0);

        //將目前的垂直速度設定給動作狀態機
        m_Animator.SetFloat( "fVertical" , m_Rigidbody2D.velocity.y);

        //處理角色是否觸發跳躍
        if (Input.GetKeyDown(KeyCode.Space) && m_Ground.m_isGround)
        {
            base.Jump();
        }

        if (Input.GetKey(KeyCode.Return))
        {
            m_HoldTimer += Time.deltaTime * 1.5f;
            m_HoldTimer = Mathf.Clamp(m_HoldTimer , 1f , 1.5f);
            if (!m_HoldParticle.isPlaying)
                m_HoldParticle.Play();
        }
        else if (m_HoldParticle.isPlaying)
            m_HoldParticle.Stop();
            

        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (m_CharaterParameter.m_fActionPoint >= 20)
            { 
                TriggerDash();

                m_CharaterParameter.m_fActionPoint -= 20;

                if (m_HoldTimer == 1.5f)
                    m_CameraEffect.SetTrigger("Shake");
            }
            m_HoldTimer = 0f;
        }            
    }   

    private void TriggerDash()
    {
        Vector2 _DashV2 = new Vector2( m_Dash.m_DashForceV2.x * GetFlip * (m_HoldTimer + 1f ) , m_Dash.m_DashForceV2.y);
        m_Dash.m_DashClass.SetDashValue(_DashV2 ,m_Dash.m_fTime);
        SetHitCase(true ,  1 , m_Dash.m_DashForceV2);

        ShowDashShadow();
        ShowDashSmoke();
    }

    private void ShowDashShadow()
    {
        GameObject _ShadowEffect = ObjectPool.m_MonoRef.GetObject(ObjectPool.ObjectPoolID.SHADOW_EFFECT);
        if (_ShadowEffect != null)
        {
            _ShadowEffect.SetActive(true);
            _ShadowEffect.GetComponent<DashShadowEffect>().StartEffect(GetNowSprite , transform.position , (GetFlip==1)? false:true , new Color(1f,.8f,0f,.5f));
        }
    }

    private void ShowDashSmoke()
    {
        GameObject _DashSmokeEffect = ObjectPool.m_MonoRef.GetObject(ObjectPool.ObjectPoolID.DASH_SMOKE);
        if (_DashSmokeEffect != null)
        {
            _DashSmokeEffect.SetActive(true);
            _DashSmokeEffect.GetComponent<DashSmokeEffect>().StartEffect(gameObject.transform , (GetFlip == 1)? true : false );
        }
    }
        
    void OnTriggerEnter2D(Collider2D other)
    {        
        if ( other.gameObject.tag == "Enemy" && m_HitCase.m_isEnabled)
        {
            other.GetComponent<CharaterBase>().GetDamage( m_HitCase.m_iDamage, GetFlip , m_Rigidbody2D.velocity / 10 );
        }
        else if (other.gameObject.tag == "Door" && m_HitCase.m_isEnabled)
        {
            Debug.LogError("Hit Door");
            other.GetComponent<CharaterBase>().GetDamage( m_HitCase.m_iDamage, GetFlip );
        }
    }

}
