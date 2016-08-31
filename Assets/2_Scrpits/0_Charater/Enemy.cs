using UnityEngine;
using System.Collections;

public class Enemy : CharaterBase {

    public DashCase m_Dash = new DashCase();
    public Transform m_RightTag;
    public Transform m_LeftTag;
    public Transform m_Target;

    protected void Awake()
    {
        m_Dash.m_DashClass.OnDash += OnDash;
        m_Dash.m_DashClass.OnDashFin += OnDashFin;
    }
    private void OnDestory()
    {
        m_Dash.m_DashClass.OnDash -= OnDash;
        m_Dash.m_DashClass.OnDashFin -= OnDashFin;
    }
	// Use this for initialization
    protected override void Start ()
    {
        base.Start ();
        m_Target = GameObject.FindGameObjectWithTag("Player").transform;
        if (m_Target == null)
            enabled = false;
    }

	// Update is called once per frame
    protected override void Update ()
    {
        base.Update ();
        if (m_CharaterParameter.m_isDeath) return;

        float _fHorizontal = 0f;

        bool _isDefence = true;
        if (_isDefence)
        {
            m_DefenceCase.IsDefence = _isDefence;
            m_DefenceCase.SetDefenceEffect(_isDefence);
            m_Dash.m_DashClass.Update();
            return;
        }
            



        if (WallChecker)
            FlipTrigger();


        _fHorizontal = 1f * Time.fixedDeltaTime * 10 ;

        //
        if ( Mathf.Abs ( Vector3.Distance ( transform.position , m_Target.position ) )  < 4  )
        {
            if (m_Dash.m_DashClass.GetIsFin && Random.Range(0 , 10) < 1)
            {
                m_Dash.m_DashClass.SetDashValue(m_Dash.m_DashForceV2 * GetFlip ,m_Dash.m_fTime);
                SetHitCase(true ,  1 , m_Dash.m_DashForceV2);
            }
        }
        else
        {          
            Vector2 _v2 = new Vector2( _fHorizontal * GetFlip , 0);
            Move(_v2);

            if (Random.Range(0 , 100) < 1 && m_Ground.IsGround)
            {
                m_Rigidbody2D.AddForce(new Vector2(0 , 1300));
            }

            if (Random.Range(0 , 100) < 1 && m_Dash.m_DashClass.GetIsFin)
            {
                m_Dash.m_DashClass.SetDashValue(m_Dash.m_DashForceV2 * GetFlip ,m_Dash.m_fTime);
                SetHitCase(true ,  1 , m_Dash.m_DashForceV2);
            }
        }
        m_Dash.m_DashClass.Update();
    }
   
    private bool WallChecker
    {
        get
        {
            bool _isFlip = false;            
            Transform _tf = (GetFlip > 0)? m_RightTag : m_LeftTag; 
            RaycastHit2D[] _Hit = Physics2D.RaycastAll( _tf.position , new Vector2( GetFlip , 0f ) , 2f);
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

    public override void GetDamage (int _iDamage, int _iSide, Vector2 _ForceV2)
    {
        base.GetDamage (_iDamage, _iSide, _ForceV2);

        Invoke("Test" , 1f);
    }

    private void Test()
    {
        FlipTrigger();
        m_Dash.m_DashClass.SetDashValue(m_Dash.m_DashForceV2 * GetFlip ,m_Dash.m_fTime);
        SetHitCase(true ,  1 , m_Dash.m_DashForceV2);
    }

    void OnTriggerEnter2D(Collider2D other)
    {                
//        if ( other.gameObject.tag == "Player")
//            Debug.LogError("Enemy hit Player!!");
        if ( other.gameObject.tag == "Player" && m_HitCase.m_isEnabled)
        {
            Debug.Log("Enemy Hit Player");
            other.GetComponent<CharaterBase>().GetDamage( m_HitCase.m_iDamage, GetFlip , m_Rigidbody2D.velocity / 10 );
        }
    }
}
