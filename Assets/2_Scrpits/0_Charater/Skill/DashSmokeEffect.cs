using UnityEngine;
using System.Collections;

public class DashSmokeEffect : MonoBehaviour {

    private Animator m_Animator = null;
    private SpriteRenderer m_SpriteRenender = null;
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        if (m_Animator == null)
        {
            m_Animator = GetComponentInChildren<Animator>();
        }
        m_SpriteRenender = GetComponent<SpriteRenderer>();
        if (m_SpriteRenender == null)
        {
            m_SpriteRenender = GetComponentInChildren<SpriteRenderer>();
        }
    }

    public void StartEffect(Transform _Tf , bool _iRightSide)
    {
        Vector3 _FixV3 = (_iRightSide)? new Vector3(-3,0,0) : new Vector3(3,0,0);
        gameObject.transform.position = _Tf.position + _FixV3;
        m_SpriteRenender.flipX = (_iRightSide)? true : false;
        m_Animator.Play("SmokeStart");
        enabled = true;
    }

    private void Update()
    {
        if (m_SpriteRenender.enabled == false)
        {
            Release();
        }
    }

    private void Release()
    {
        ObjectPool.m_MonoRef.ReleaseObject(gameObject , ObjectPool.ObjectPoolID.DASH_SMOKE);
    }
	
}
