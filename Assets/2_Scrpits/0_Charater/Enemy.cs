using UnityEngine;
using System.Collections;

//AI 模式類型
public enum AIMode
{
    
}

public class Enemy : CharaterBase {
    
    [Header( "Dash參數" )]
    public DashCase m_Dash = new DashCase();

    public Transform m_RightTag;
    public Transform m_LeftTag;
    public Transform m_Target;

    [Header("牆壁偵測距離")]
    public float m_fWallCheckDistance = 2f;

    [Header("警戒時保持與目標的最遠距離")]
    public float m_fKeepDistanceToTargetMax = 20f;
    [Header("警戒時保持與目標的最近距離")]
    public float m_fKeepDistanceToTargetMin = 5f;

    public float m_fAttackRate = 1.5f;
    private float m_fAttackTimer = 0f;

    protected void Awake()
    {
        m_Dash.m_DashClass.OnDash       += OnDash;
        m_Dash.m_DashClass.OnDashFin    += OnDashFin;
    }
    private void OnDestory()
    {
        m_Dash.m_DashClass.OnDash       -= OnDash;
        m_Dash.m_DashClass.OnDashFin    -= OnDashFin;
    }
        
    protected override void Start ()
    {
        base.Start ();
        m_Target = GameObject.FindGameObjectWithTag("Player").transform;
        if (m_Target == null)
            enabled = false;

        FlipTrigger();
    }
        
    protected override void Update ()
    {
        base.Update ();
        if (m_CharaterParameter.m_isDeath) return;

        float _fHorizontal = 10f * Time.deltaTime;

        //先判斷目標是否在視野內
        float _fDisToTarget = Vector3.Distance(transform.position , m_Target.transform.position);
        if (_fDisToTarget <= m_CharaterParameter.m_fViewDistance)
        {
            //在視野內

            //往目標移動
            //保持距離 或 進行攻擊，防禦
        }
        else
        {
            //在視野外
            //持續巡邏
        }



        bool _isDefence = false;
        if (_isDefence)
        {
            m_DefenceCase.IsDefence = _isDefence;
            m_DefenceCase.SetDefenceEffect(_isDefence);
            m_Dash.m_DashClass.Update();
            return;
        }                     

        if (WallChecker || FlipChecker)
            FlipTrigger();



        if ( Mathf.Abs ( Vector3.Distance ( transform.position , m_Target.position ) )  < m_fKeepDistanceToTargetMax  )
        {
            if (Mathf.Abs ( Vector3.Distance ( transform.position , m_Target.position ) )  < m_fKeepDistanceToTargetMin )
            {
                _fHorizontal *= -1f;
                Vector2 _v2 = new Vector2( _fHorizontal * GetFlip , 0);
                Move(_v2);
            }
            else
            {
                m_fAttackTimer += Time.deltaTime;
                if (m_fAttackTimer > m_fAttackRate)
                {
                    Vector2 _DashV2 = new Vector2( m_Dash.m_DashForceV2.x * GetFlip  , m_Dash.m_DashForceV2.y);
                    m_Dash.m_DashClass.SetDashValue(_DashV2 ,m_Dash.m_fTime);
                    SetHitCase(true ,  1 , m_Dash.m_DashForceV2);  
                    m_fAttackTimer = 0f;
                }
            }
        }
        else
        {          
            Vector2 _v2 = new Vector2( _fHorizontal * GetFlip , 0);
            Move(_v2);
        }

        m_Dash.m_DashClass.Update();
    }        

    /// <summary>
    /// 取得是否背對目標物
    /// </summary>
    private bool FlipChecker
    {
        get
        {
            //_fDot < 0 ， 目標物在左邊
            float _fDot = Vector3.Dot(transform.TransformDirection(Vector3.right)
                , m_Target.transform.position - transform.position);


            bool _isNeedToFlip = ( _fDot > 0 != GetFlip > 0)? true : false;
            return _isNeedToFlip;
        }
    }

    /// <summary>
    /// 取得是否太接近牆壁
    /// </summary>
    private bool WallChecker
    {
        get
        {
            bool _isFlip = false;            
            Transform _tf = (GetFlip > 0)? m_RightTag : m_LeftTag; 
            RaycastHit2D[] _Hit = Physics2D.RaycastAll( _tf.position , new Vector2( GetFlip , 0f ) , m_fWallCheckDistance);
            for (int x = 0 ; x < _Hit.Length ; x++)
            {
                if (_Hit[x].collider != null)
                {                   
                    if ( _Hit[x].transform.gameObject.tag == "Ground")
                    {
                        _isFlip = true;
                    }
                }
            }
            return _isFlip;
        }
    } 

    public override void GetDamage (DamageClass _Data)
    {
        base.GetDamage (_Data);

//        Invoke("Test" , 0.1f);
    }

    private void Test()
    {        
        m_Dash.m_DashClass.SetDashValue(m_Dash.m_DashForceV2 * GetFlip ,m_Dash.m_fTime);
        SetHitCase(true ,  1 , m_Dash.m_DashForceV2);
    }

    void OnTriggerEnter2D(Collider2D other)
    {                
        if ( other.gameObject.tag == "Player" && m_HitCase.m_isEnabled)
        {
            //設置攻擊參數
            DamageClass _DamageData = new DamageClass();
            _DamageData.m_iDamage = m_HitCase.m_iDamage;
            _DamageData.m_iSide = GetFlip;
            _DamageData.m_ForceV2 = m_Rigidbody2D.velocity / 10;

            other.SendMessage( "GetDamage" , _DamageData );
        }
    }
}
