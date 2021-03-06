﻿using UnityEngine;
using System.Collections;

public class Door : CharaterBase {

    public DashCase m_Dash = new DashCase();

    protected void Awake()
    {
        
    }
    private void OnDestory()
    {
        
    }
    // Use this for initialization
    protected override void Start ()
    {
        base.Start ();
    }
    //    int i = 1;
    // Update is called once per frame
    public override void MonoUpdate ()
    {
        if (m_CharaterParameter.GetIsDeath)
        {
//            DeathEffect();
        }
    }   

//    public override void GetDamage (int _iDamage, int _iSide, Vector2 _ForceV2)
//    {
//        Debug.LogError("Door get Damage");
//        ProcessHealthPoint( - _iDamage );
//        ProcessGetDamageEffect(_iSide);
//    }

    public override void GetDamage (DamageClass _Data)
    {
        base.GetDamage (_Data);
        m_Animator.SetTrigger("GetDamage");
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {                
        base.OnTriggerEnter2D( other );
    }
}
