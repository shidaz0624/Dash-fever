using UnityEngine;
using System.Collections;

public class Hero : CharaterBase {

    public DashCase m_Dash = new DashCase();

    private int m_iJumpPower = 1000;
    #region MonoBehaviour
    public void Awake()
    {
        m_Dash.m_DashClass.OnDash += OnDash;
    }

    public override void  Start () 
    {       
        base.Start();
    }

    public override void Update () 
    {
    }

    public void FixedUpdate()
    {
        m_Dash.m_DashClass.Update();
        base.Update();
        this.ProcessInput();
    }

    private void OnDestory()
    {
        m_Dash.m_DashClass.OnDash -= OnDash;
    }
    #endregion	

    private void ProcessInput()
    {
        float _fOriginHorozontal = Input.GetAxis("Horizontal");
        float _fHorizontal = _fOriginHorozontal * Time.deltaTime * 15;

        this.ProcessFlip(_fHorizontal);

        Move(new Vector2(_fHorizontal , 0));
        m_Animator.SetFloat( "fHorizontal" , Mathf.Abs( _fOriginHorozontal  ) );



        if (Input.GetKeyDown(KeyCode.Space) && m_Ground.m_isGround)
        {
            m_Rigidbody2D.AddForce( new Vector2( 0 , m_iJumpPower));
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_Dash.m_DashClass.SetDashValue(m_Dash.m_DashForceV2 * GetFlip ,m_Dash.m_fTime);
        }            
    }

    private void ProcessFlip(float _fHorizontal)
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

    private void OnDash(Vector2 _V2)
    {
        m_Rigidbody2D.velocity = _V2;
    }


}
