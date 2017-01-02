using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {

    /// <summary>
    /// 物件池中有的物件類型
    /// </summary>
    public enum ObjectPoolID
    {
        NONE,
        SHADOW_EFFECT,
        DASH_SMOKE,
        DAMAGE_HUD,
    }

    public static ObjectPool m_MonoRef = null;

    [System.Serializable]
    public class PoolUnit
    {
        //物件種類
        public ObjectPoolID m_ID            = ObjectPoolID.NONE;
        //物件使用的prefab
        public GameObject   m_Prefab        = null;
        public Transform    m_Parent        = default(Transform);
        //物件的最大數量
        public int          m_iMaxCount     = 0;
        //物件在初始化時的初始數量
        public int          m_iInitCount    = 0;
        //物件清單
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
                InitObjectPoolUnit( m_PoolUnitList[i] );
        }
    }        

    /// <summary>
    /// 初始化單位物件池
    /// </summary>
    private void InitObjectPoolUnit(PoolUnit _PoolUnit)
    {
        //使用非同步來創建物件
        StartCoroutine(CreateUnitObject(_PoolUnit));
    }

    /// <summary>
    /// 非同步創建物件池物件
    /// </summary>
    private IEnumerator CreateUnitObject(PoolUnit _PoolUnit)
    {
        _PoolUnit.m_PoolList = new List<GameObject>();
        for (int i = 0 ; i < _PoolUnit.m_iInitCount ; i++)
        {
            GameObject _Unit = Instantiate( _PoolUnit.m_Prefab ) as GameObject;
            if (_PoolUnit.m_Parent == default(Transform))
                _Unit.transform.parent = this.transform;
            else
                _Unit.transform.parent = _PoolUnit.m_Parent;
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
        //找出符合類型的物件池
        for(int i = 0 ; i < m_PoolUnitList.Count ; i++ )
        {
            if (m_PoolUnitList[i] != null && m_PoolUnitList[i].m_ID == _ID)
            {                     
                List<GameObject> _objList =  m_PoolUnitList[i].m_PoolList;
                for (int x = 0 ; x < _objList.Count ; x ++)
                {
                    //找出物件池中被關掉(未使用)的物件
                    if (!_objList[x].activeInHierarchy)
                    {
                        return _objList[x];
                    }
                }
                //若物件池中沒有空閒的物件

                //若此物件類型還未達物件上限，則新增一個物件，並回傳
                if (m_PoolUnitList[i].m_PoolList.Count < m_PoolUnitList[i].m_iMaxCount)
                {
                    GameObject _Unit = Instantiate( m_PoolUnitList[i].m_Prefab ) as GameObject;
                    if (m_PoolUnitList[i].m_Parent == default(Transform))
                        _Unit.transform.parent = this.transform;
                    else
                        _Unit.transform.parent = m_PoolUnitList[i].m_Parent;
//                    _Unit.transform.localScale = Vector3.one;
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
