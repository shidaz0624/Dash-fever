using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharaterUpdater : SystemBase
{

    private bool m_isUpdate = true;

    private void OnGUI()
    {
        if(GUI.Button(new Rect(0 ,0 ,50,50) , m_isUpdate.ToString()))
        {
            m_isUpdate = !m_isUpdate;
        }
    }

    public override void Init()
    {
        Debug.Log(" == CharaterUpdater Init ==" , this);
    }
        
    public void SysUpdate (List<CharaterBase> _Charater)
    {
        if (_Charater == null || _Charater.Count == 0 ) return;
        if (!m_isUpdate) return;
        foreach (var item in _Charater) 
        {
            if (item != null) item.MonoUpdate();
        }
    }

    public void SysFixedUpdate(List<CharaterBase> _Charater)
    {
        if (_Charater == null || _Charater.Count == 0) return;
    }



    public bool IsUpdate 
    {
        set{ m_isUpdate = value; }
        get{ return m_isUpdate;  }
    }
}

