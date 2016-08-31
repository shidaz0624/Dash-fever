using UnityEngine;
using System.Collections;

public class DashShadowEffect : MonoBehaviour {

    private SpriteRenderer m_Sprite = null;

	void Awake () {
        m_Sprite = GetComponent<SpriteRenderer>();
        if (m_Sprite == null)
        {
            m_Sprite = GetComponentInChildren<SpriteRenderer>();
        }
	}
	
    public void StartEffect(Sprite _Sprite , Vector3 _PosV3 , bool _isFlip , Color _Color)
    {        
        m_Sprite.sprite = _Sprite;
        transform.position = _PosV3;
        m_Sprite.flipX = _isFlip;
        m_Sprite.material.SetColor("_Color",_Color);
        Color _c = m_Sprite.material.color;
        m_Sprite.material.color = new Color(_c.r , _c.g , _c.b , 1f);
        enabled = true;
    }

    public void StartEffect(Sprite _Sprite , Vector3 _PosV3 , bool _isFlip)
    {        
        m_Sprite.sprite = _Sprite;
        transform.position = _PosV3;
        m_Sprite.flipX = _isFlip;
        Color _c = m_Sprite.material.color;
        m_Sprite.material.color = new Color(_c.r , _c.g , _c.b , 1f);
        enabled = true;
    }
        
	void Update () {
        Color _c = m_Sprite.material.color;
        _c.a -= Time.deltaTime * 1.5f;
        m_Sprite.material.color = _c;
        if (m_Sprite.material.color.a < 0)
        {
            Release();
        }
	}

    private void Release()
    {
        ObjectPool.m_MonoRef.ReleaseObject(gameObject , ObjectPool.ObjectPoolID.SHADOW_EFFECT);
        enabled = false;
    }
}
