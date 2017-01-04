using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharaterMaintainer : SystemBase {

    private static CharaterMaintainer m_Mono = null;
    public  static CharaterMaintainer Mono { get{ return m_Mono; } }

    public List<CharaterBase> m_CharaterList = null;

    //角色更新器
    private CharaterUpdater m_CharaterUpdater = null;
    //角色戰鬥控制器
    private CharaterBattler m_CharaterBattler = null;

    public override void Init()
    {
        Debug.Log(" == CharaterMaintainer Init == " , this);
        m_Mono = this;
        m_CharaterUpdater = gameObject.AddComponent< CharaterUpdater >();
        m_CharaterUpdater.Init();
        m_CharaterBattler = gameObject.AddComponent< CharaterBattler >();
    }

    #region Updater
    public bool IsCharaterUpdate 
    {
        set{ m_CharaterUpdater.IsUpdate = value; }
        get{ return m_CharaterUpdater.IsUpdate; }
    }

    public override void SysUpdate ()
    {
        if (m_CharaterUpdater.IsUpdate)
            m_CharaterUpdater.SysUpdate(m_CharaterList);
	}

    public override void SysFixedUpdate()
    {
        if (m_CharaterUpdater.IsUpdate)
            m_CharaterUpdater.SysFixedUpdate(m_CharaterList);
    }

    public void AddCharater( CharaterBase _Charater )
    {
        if (m_CharaterList == null)
            m_CharaterList = new List<CharaterBase>();

        m_CharaterList.Add(_Charater);
    }

    public void RemoveCharater( CharaterBase _Charater )
    {
        if (m_CharaterList == null) return;

        if (m_CharaterList.Contains( _Charater ))
        {
            m_CharaterList.Remove( _Charater );
        }
    }
    #endregion

    #region Battler




    public void PassDamageClassToCharater( CharaterBase _From , DamageClass _DamageClass , CharaterBase _Charater)
    {
        m_CharaterBattler.PassDamageClassToCharater(_From, _DamageClass , _Charater );
    }
    public void PassDamageClassToCharater(CharaterBase _From , DamageClass _DamageClass , GameObject _CharaterObj)
    {                
        CharaterBase _Charater = _CharaterObj.GetComponent< CharaterBase >();
        if (_Charater != null)
            m_CharaterBattler.PassDamageClassToCharater(_From, _DamageClass , _Charater );
    }
    #endregion
}