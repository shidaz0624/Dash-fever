using UnityEngine;
using System.Collections;

public class Hero : CharaterBase {

    //Dash Calss
    public DashCase m_Dash = new DashCase();
    //攝影機動畫效果
    public Animator m_CameraEffect = null;
    //按住Enter的持續時間
    private float m_HoldTimer = 0f;
    //按住ENTER的粒子效果
    public ParticleSystem m_HoldParticle = null;

    //輸入進來的橫向位移量
    private float m_fOriginHorozontal = 0f; 
    //計算後的橫向位移量
    private float m_fHorizontal = 0f;

    #region MonoBehaviour
    protected void Awake()
    {
        RegisterEvent();
    }

    protected override void  Start () 
    {               
        base.Start();
    }
        
    public override void MonoUpdate ()
    {
        //處理地板，死亡特效
        base.MonoUpdate();

        //更新Dash
        m_Dash.m_DashClass.Update();

        //處理玩家輸入
        this.ProcessInput();

        //計算現在的HP及AP
        m_CharaterParameter.SetHPAndAPByDelta
        ( m_RecoverParameter.GetHPRecoverValue , m_RecoverParameter.GetAPRecoverValue );

        //將HP及AP顯示在GUI上
        MainGameHost.MonoRef.UpdateHeroHPAndAP
            ( (int)m_CharaterParameter.m_fHealthPoint , (int)m_CharaterParameter.m_fActionPoint );
    }        

    private void OnDisable()
    {
        UnregisterEvent();
    }
    #endregion	

    #region Event
    /// <summary>
    /// 註冊事件
    /// </summary>
    private void RegisterEvent()
    {
        m_Dash.m_DashClass.OnDash       = OnDash;
        m_Dash.m_DashClass.OnDashStart  = OnDashStart;
        m_Dash.m_DashClass.OnDashFin    = OnDashFin;
    }
    /// <summary>
    /// 解註冊事件
    /// </summary>
    private void UnregisterEvent()
    {
        m_Dash.m_DashClass.OnDash       = null;
        m_Dash.m_DashClass.OnDashStart  = null;
        m_Dash.m_DashClass.OnDashFin    = null;
    }
    #endregion

    private void ProcessInput()
    {
        //取得目前輸入的橫向位移量
        m_fOriginHorozontal = Input.GetAxis("Horizontal");
        //換算要使用的位移量
        m_fHorizontal = m_fOriginHorozontal * Time.deltaTime * 15;

        //用輸入的位移量判斷是否需要轉向
        base.ProcessFlip(m_fHorizontal);

        //取得是否按下防禦鍵
        bool _isKeyDefence = Input.GetKey(KeyCode.LeftShift);

        if (_isKeyDefence)
        {
            //若按下防禦時，開啟防禦，並將橫向位移量歸零
            m_DefenceCase.SetDefence( true , 0 );
            m_fHorizontal = 0;
            m_fOriginHorozontal = 0;
        }
        else
        {
            m_DefenceCase.SetDefence(false);
        }       

        //角色移動
        Move(new Vector2(m_fHorizontal , 0));

        //若角色在地上，則將橫向位移量傳給Animator做跑步動畫
        //不在地上時歸零，Animator順利處理跳躍的動畫
        if ( m_Ground.IsGround )
            m_Animator.SetFloat( "fHorizontal" , Mathf.Abs( m_fOriginHorozontal ));
        else
            m_Animator.SetFloat( "fHorizontal" , 0);

        //將目前的垂直速度設定給Animator
        m_Animator.SetFloat( "fVertical" , m_Rigidbody2D.velocity.y);

        //處理角色是否觸發跳躍
        if (Input.GetKeyDown(KeyCode.Space) && m_Ground.m_isGround)
        {
            base.Jump();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerDash( new Vector2(-1f,0f) );
        }

        //當按下Return
        if (Input.GetKey(KeyCode.Return))
        {
            //計算按下的總時間
            m_HoldTimer += Time.deltaTime * 1.5f;
            //最小1f，最大1.5f
            m_HoldTimer = Mathf.Clamp(m_HoldTimer , 1f , 1.5f);
            if (!m_HoldParticle.isPlaying)
                m_HoldParticle.Play();
        }
        else if (m_HoldParticle.isPlaying)
        {
            m_HoldParticle.Stop();
        }            

        //Return起來時
        if (Input.GetKeyUp(KeyCode.Return))
        {
            //當角色行動點數 >= 20時
            if (m_CharaterParameter.m_fActionPoint >= 20)
            { 
                //觸發Dash
                TriggerDash();
                //行動點數減20
                m_CharaterParameter.m_fActionPoint -= 20;

                //若按下Return時間超過1f * 1.5f時觸發攝影機震動效果
                if (m_HoldTimer == 1.5f)
                    m_CameraEffect.SetTrigger("Shake");
            }
            m_HoldTimer = 0f;
        }            
    }   

    /// <summary>
    /// 觸發Dash
    /// </summary>
    private void TriggerDash(Vector2 _Force = default(Vector2))
    {
        Vector2 _DashV2;
        //計算這次要觸發Dash的值
        if (_Force == default(Vector2))
            _DashV2 = new Vector2( m_Dash.m_DashForceV2.x * GetFlip * (m_HoldTimer + 1f ) , m_Dash.m_DashForceV2.y);
        else
            _DashV2 = new Vector2( m_Dash.m_DashForceV2.x * GetFlip * 2 , m_Dash.m_DashForceV2.y);
        if (_Force != default(Vector2))
        {
            _DashV2.x = _DashV2.x * _Force.x ;
            _DashV2.y = _DashV2.y * _Force.y ;
        }
        m_Dash.m_DashClass.SetDashValue(_DashV2 ,m_Dash.m_fTime);
        SetHitCase(true ,  1 , m_Dash.m_DashForceV2);

        ShowDashShadow();
        ShowDashSmoke();
    }

    /// <summary>
    /// 打開Dash殘影
    /// </summary>
    private void ShowDashShadow()
    {
        GameObject _ShadowEffect = ObjectPool.m_MonoRef.GetObject(ObjectPool.ObjectPoolID.SHADOW_EFFECT);
        if (_ShadowEffect != null)
        {
            _ShadowEffect.SetActive(true);
            _ShadowEffect.GetComponent<DashShadowEffect>().StartEffect(GetNowSprite , transform.position , (GetFlip==1)? false:true , new Color(1f,.8f,0f,.5f));
        }
    }

    /// <summary>
    /// 打開Dash煙霧
    /// </summary>
    private void ShowDashSmoke()
    {
        GameObject _DashSmokeEffect = ObjectPool.m_MonoRef.GetObject(ObjectPool.ObjectPoolID.DASH_SMOKE);
        if (_DashSmokeEffect != null)
        {
            _DashSmokeEffect.SetActive(true);
            _DashSmokeEffect.GetComponent<DashSmokeEffect>().StartEffect(gameObject.transform , (GetFlip == 1)? true : false );
        }
    }

//    public override void OnTriggerEnter2D(Collider2D other)
//    {      
//        if (m_HitCase.m_isEnabled == false) return;
//        base.OnTriggerEnter2D( other );
//    }

    public override void GetDamage (DamageClass _Data)
    {
        base.GetDamage(_Data);
    }        

    public SpriteRenderer m_CharacterRenderer;
    private Material m_CharacterMaterial;
    private IEnumerator DamageEffect()
    {
        if (m_CharacterMaterial == null)
            m_CharacterMaterial = m_CharacterRenderer.material;            

        for (int i = 0 ; i < 4 ; i++)
        {
            yield return new WaitForSeconds(0.05f);
            m_CharacterMaterial.SetFloat("SingleColor" , 1f);    
            yield return new WaitForSeconds(0.05f);
            m_CharacterMaterial.SetFloat("SingleColor" , 0f);
        }
    }
}