﻿using UnityEngine;
using System.Collections;

public class HitCase
{
    public bool     m_isEnabled = false;
    public int      m_iDamage    = 0;
    public Vector2  m_ForceV2   = Vector2.zero;
}

public class Hero : CharaterBase {

    public DashCase m_Dash = new DashCase();

    private int m_iJumpPower = 1300;
    #region MonoBehaviour
    protected void Awake()
    {
        m_Dash.m_DashClass.OnDash += OnDash;
        m_Dash.m_DashClass.OnDashFin += OnDashFin;
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

        m_CharaterParameter.SetHPAndAPByDelta
        ( m_RecoverParameter.GetHPRecoverValue , m_RecoverParameter.GetAPRecoverValue );
        MainGameHost.MoroRef.UpdateHeroHPAndAP
            ( (int)m_CharaterParameter.m_fHealthPoint , (int)m_CharaterParameter.m_fActionPoint );
    }

//    protected void FixedUpdate()
//    {
//    }

    private void OnDestory()
    {
        m_Dash.m_DashClass.OnDash -= OnDash;
        m_Dash.m_DashClass.OnDashFin -= OnDashFin;
    }
    #endregion	

    private void ProcessInput()
    {
        float _fOriginHorozontal = Input.GetAxis("Horizontal");
        float _fHorizontal = _fOriginHorozontal * Time.deltaTime * 15;

        this.ProcessFlip(_fHorizontal);

        Move(new Vector2(_fHorizontal , 0));

        if ( m_Ground.IsGround )
            m_Animator.SetFloat( "fHorizontal" , Mathf.Abs( _fOriginHorozontal ));
        else
            m_Animator.SetFloat( "fHorizontal" , 0);
        
        m_Animator.SetFloat( "fVertical" , m_Rigidbody2D.velocity.y);


        if (Input.GetKeyDown(KeyCode.Space) && m_Ground.m_isGround)
        {
            m_Rigidbody2D.AddForce( new Vector2( 0 , m_iJumpPower));
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Return))
        {
            if (m_CharaterParameter.m_fActionPoint >= 20)
            {
                m_CharaterParameter.m_fActionPoint -= 20;
                m_Dash.m_DashClass.SetDashValue(m_Dash.m_DashForceV2 * GetFlip ,m_Dash.m_fTime);
                SetHitCase(true ,  1 , m_Dash.m_DashForceV2);
            }
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
            other.GetComponent<CharaterBase>().GetDamage( m_HitCase.m_iDamage, GetFlip , m_Rigidbody2D.velocity / 10 );
        }
    }

}