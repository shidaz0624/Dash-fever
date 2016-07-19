using UnityEngine;
using System.Collections;

[System.Serializable]
public class GroundCase
{
    public  Transform   m_GroundTag         = null; //角色偵測地板的射線起始點物件
    public  LayerMask   m_GroundLayerMask;              //地板圖層
    public  float       m_fGroundRay        = 0.2f;
    public   bool       m_isGround          = false;
    public bool IsGround
    {
        get {
            
        bool _isGround = Physics2D.OverlapCircle(m_GroundTag.position ,
            m_fGroundRay ,
            m_GroundLayerMask);
            return _isGround;
        }

    }
}

public class CharaterBase : MonoBehaviour {

    public GroundCase m_Ground = new GroundCase();
    protected Rigidbody2D m_Rigidbody2D = null;
	// Use this for initialization
    public Animator m_Animator = null;

    private int m_iFlip = 1; // -1 , 1
    private SpriteRenderer m_SpriteRenderer = null;
    public virtual void Start () {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_Animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
    public virtual void Update () {
        m_Ground.m_isGround = m_Ground.IsGround;
	}

    public void Move(Vector2 _MoveV2)
    {
        transform.Translate(_MoveV2);
    }

    protected int GetFlip
    {
        get
        {return m_iFlip;}
    }

    protected int FlipTrigger()
    {
        m_iFlip = (m_iFlip == 1)? -1 : 1;
        m_SpriteRenderer.flipX = (m_iFlip == 1)? false : true;
        return m_iFlip;
    }
}
