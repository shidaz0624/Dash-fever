using UnityEngine;
using System.Collections;

//AI 模式類型
public enum AIMode
{
    NONE,
    WANDER,         //遊蕩
    ATTACK,         //攻擊
    CAUTION,        //警戒
    AVOID,          //躲避
}

public class Enemy : CharaterBase {
    
    [Header( "Dash參數" )]
    public DashCase m_Dash = new DashCase();

    public Transform m_RightTag;
    public Transform m_LeftTag;
    public Transform m_Target;
//    public 

    [Header("牆壁偵測距離")]
    public float m_fWallCheckDistance = 2f;

    public AIMode m_AIMode = AIMode.NONE;
    [Header("警戒時保持與目標的最遠距離")]
    public float m_fKeepDistanceToTargetMax = 20f;
    [Header("警戒時保持與目標的最近距離")]
    public float m_fKeepDistanceToTargetMin = 5f;

    public float m_fAttackRate = 5f;
    private float m_fAttackTimer = 0f;

    private float m_fEscapeTime = 2f;   //角色卡點掙脫時間
    private float m_fEscapeTimer = 0f;  //角色卡點掙脫計時器

    #region Mono Life cycle
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
        m_AIMode = AIMode.WANDER;
//        m_Target = GameObject.FindGameObjectWithTag("Player").transform;
        if (m_Target == null)
            enabled = false;

        FlipTrigger();
    }
    #endregion
    public override void MonoUpdate ()
    {
        base.MonoUpdate ();
        if (m_CharaterParameter.GetIsDeath) return;

        float _fHorizontal = base.m_CharaterParameter.GetMoveSpeed * Time.deltaTime;


        //先判斷目標是否在視野內
        float _fDisToTarget = Vector3.Distance(transform.position , m_Target.transform.position);
        if (_fDisToTarget <= m_CharaterParameter.GetViewDistance)
        {
            //在視野內
            //往目標移動
            //保持距離 或 進行攻擊，防禦
            m_AIMode = AIMode.CAUTION;
        }
        else
        {
            //在視野外
            //持續巡邏
            m_AIMode = AIMode.WANDER;
        }            

        if (IsCloseWall || IsFaceToTarget == false)
            FlipTrigger();

        if (m_AIMode == AIMode.CAUTION)//警戒
        {            
            if ( Mathf.Abs ( Vector3.Distance ( transform.position , m_Target.position ) )  < m_fKeepDistanceToTargetMax  )
            {//若 距離 < 警戒最大距離                
                if (Mathf.Abs ( Vector3.Distance ( transform.position , m_Target.position ) )  < m_fKeepDistanceToTargetMin )
                {
                    m_fEscapeTimer += Time.deltaTime;

                    if (m_fEscapeTimer < m_fEscapeTime)
                    {
                        _fHorizontal *= -1f;
//                        Vector2 _v2 = new Vector2( _fHorizontal * GetFlip , 0);
//                        Move(_v2);
                    }
                    else
                    {
//                        if (IsFaceToTarget == false) FlipTrigger();
                        m_fEscapeTimer = 0f;
                        Vector2 _DashV2 = new Vector2( m_Dash.m_DashForceV2.x * - GetFlip  , m_Dash.m_DashForceV2.y);
                        m_Dash.m_DashClass.SetDashValue(_DashV2 ,m_Dash.m_fTime);
                    }
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
                        m_fAttackRate = Random.Range(0.2f,1f);
                        _fHorizontal = 0f;
                    }
                }
            }
            else
            {          
                //若 距離 > 警戒最大距離
//                Vector2 _v2 = new Vector2( _fHorizontal * GetFlip , 0);
//                Move(_v2);
            }
        }
        else if (m_AIMode == AIMode.WANDER)//巡邏
        {
            _fHorizontal *= .5f;
        }

//        _fHorizontal = 0f;

        Vector2 _v2 = new Vector2( _fHorizontal * GetFlip , 0);
        Move(_v2);

        if ( m_Ground.IsGround )
            m_Animator.SetFloat( "fHorizontal" , Mathf.Abs( _fHorizontal ));
        else
            m_Animator.SetFloat( "fHorizontal" , 0);


//        bool _isDefence = false;
//        if (_isDefence)
//        {
//            m_DefenceCase.IsDefence = _isDefence;
//            m_DefenceCase.SetDefenceEffect(_isDefence);
//            m_Dash.m_DashClass.Update();
//            return;
//        }                     
//
//        if (WallChecker || FlipChecker)
//            FlipTrigger();
//
//
//        if ( Mathf.Abs ( Vector3.Distance ( transform.position , m_Target.position ) )  < m_fKeepDistanceToTargetMax  )
//        {
//            if (Mathf.Abs ( Vector3.Distance ( transform.position , m_Target.position ) )  < m_fKeepDistanceToTargetMin )
//            {
//                _fHorizontal *= -1f;
//                Vector2 _v2 = new Vector2( _fHorizontal * GetFlip , 0);
//                Move(_v2);
//            }
//            else
//            {
//                m_fAttackTimer += Time.deltaTime;
//                if (m_fAttackTimer > m_fAttackRate)
//                {
//                    Vector2 _DashV2 = new Vector2( m_Dash.m_DashForceV2.x * GetFlip  , m_Dash.m_DashForceV2.y);
//                    m_Dash.m_DashClass.SetDashValue(_DashV2 ,m_Dash.m_fTime);
//                    SetHitCase(true ,  1 , m_Dash.m_DashForceV2);  
//                    m_fAttackTimer = 0f;
//                }
//            }
//        }
//        else
//        {          
//            Vector2 _v2 = new Vector2( _fHorizontal * GetFlip , 0);
//            Move(_v2);
//        }

        m_Dash.m_DashClass.Update();
    }        

    /// <summary>
    /// 取得是否背對目標物
    /// </summary>
    private bool IsFaceToTarget
    {
        get
        {
            //_fDot < 0 ， 目標物在左邊
            float _fDot = Vector3.Dot(transform.TransformDirection(Vector3.right)
                , m_Target.transform.position - transform.position);

            bool _isFaceToTarget = ( _fDot > 0 != GetFlip > 0)? false : true;
            return _isFaceToTarget;
        }
    }

    /// <summary>
    /// 取得是否太接近牆壁
    /// </summary>
    private bool IsCloseWall
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
}


