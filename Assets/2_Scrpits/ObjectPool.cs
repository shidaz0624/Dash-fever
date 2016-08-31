using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {

    public enum ObjectPoolID
    {
        NONE,
        SHADOW_EFFECT,
        DASH_SMOKE,
    }


    public static ObjectPool m_MonoRef = null;

    [System.Serializable]
    public class PoolUnit
    {
        public ObjectPoolID m_ID            = ObjectPoolID.NONE;
        public GameObject   m_Prefab        = null;
        public int          m_iMaxCount     = 0;
        public int          m_iInitCount    = 0;
        public List<GameObject> m_PoolList  = null;
    }
    public List<PoolUnit> m_PoolUnitList = null;

	
    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        m_MonoRef = this;

        if (m_PoolUnitList == null && m_PoolUnitList.Count == 0)
            return;

        for(int i = 0 ; i < m_PoolUnitList.Count ; i++)
        {            
            if (m_PoolUnitList[i] != null)
                InitPoolUnit( m_PoolUnitList[i] );
        }
    }        

    /// <summary>
    /// 初始化單位物件池
    /// </summary>
    private void InitPoolUnit(PoolUnit _PoolUnit)
    {
        //使用異步線程來創建物件
        StartCoroutine(CreateInitObject(_PoolUnit));
    }

    /// <summary>
    /// 非同步創建物件池物件
    /// </summary>
    private IEnumerator CreateInitObject(PoolUnit _PoolUnit)
    {
        _PoolUnit.m_PoolList = new List<GameObject>();
        for (int i = 0 ; i < _PoolUnit.m_iInitCount ; i++)
        {
            GameObject _Unit = Instantiate( _PoolUnit.m_Prefab ) as GameObject;
            _Unit.transform.parent = this.transform;
            _Unit.SetActive(false);
            _PoolUnit.m_PoolList.Add(_Unit);
            yield return new WaitForEndOfFrame();
        }
            
    }        

    /// <summary>
    /// 取得指定的物件池物件
    /// </summary>
    public GameObject GetObject(ObjectPoolID _ID)
    {
        for(int i = 0 ; i < m_PoolUnitList.Count ; i++ )
        {
            if (m_PoolUnitList[i] != null && m_PoolUnitList[i].m_ID == _ID)
            {                
                List<GameObject> _objList =  m_PoolUnitList[i].m_PoolList;
                for (int x = 0 ; x < _objList.Count ; x ++)
                {
                    if (!_objList[x].activeInHierarchy)
                    {
                        return _objList[x];
                    }
                }

                if (m_PoolUnitList[i].m_PoolList.Count < m_PoolUnitList[i].m_iMaxCount)
                {
                    GameObject _Unit = Instantiate( m_PoolUnitList[i].m_Prefab ) as GameObject;
                    _Unit.transform.parent = this.transform;
                    _Unit.SetActive(false);
                    m_PoolUnitList[i].m_PoolList.Add(_Unit);
                    return _Unit;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 釋放物件池物件.
    /// </summary>
    public bool ReleaseObject(GameObject _Obj , ObjectPoolID _ID = ObjectPoolID.NONE)
    {
        if (_Obj != null)
        {
            _Obj.SetActive(false);
            return true;
        }
        else
            return false;
    }

}
