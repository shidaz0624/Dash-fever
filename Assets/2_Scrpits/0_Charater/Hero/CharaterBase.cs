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

[System.Serializable]
public class CharaterParameter
{
    public int m_iHealthPoint = 0;
    public int m_iActionPoint = 0;
}

public class CharaterBase : MonoBehaviour {

    //角色參數class
    public CharaterParameter m_CharaterParameter = new CharaterParameter();
    //地板class
    public GroundCase m_Ground = new GroundCase();
    //角色鋼體
    protected Rigidbody2D m_Rigidbody2D = null;
    //角色狀態機
    public Animator m_Animator = null;
    //角色攻擊class
    public HitCase m_HitCase = new HitCase();

    private int m_iFlip = 1; // -1 , 1
    private SpriteRenderer m_SpriteRenderer = null;


    protected virtual void Start () {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_Animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
    protected virtual void Update () {
        m_Ground.m_isGround = m_Ground.IsGround;
        if (m_Animator != null)
            m_Animator.SetBool("isGround" , m_Ground.m_isGround);
	}

    protected void Move(Vector2 _MoveV2)
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

    protected void SetHitCase( bool _isEnabled , int _iDamage = 0, Vector2  _ForceV2 = default(Vector2))
    {
        m_HitCase.m_isEnabled = _isEnabled;
        Debug.LogError("Set " + _isEnabled);
        if (_isEnabled)
        {
            m_HitCase.m_iDamage = _iDamage;
            m_HitCase.m_ForceV2 = _ForceV2;
        }
    }

    protected void ProcessFlip(float _fHorizontal)
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

    private void ProcessHealthPoint(int _iDelta)
    {
        m_CharaterParameter.m_iHealthPoint += _iDelta;
        if(m_CharaterParameter.m_iHealthPoint <= 0)
        {
            //To do Dead
            Destroy(gameObject);
        }
    }

    public void GetDamage( int _iDamage , Vector2 _ForceV2 = default(Vector2) )
    {
        ProcessHealthPoint( - _iDamage );

        if ( _ForceV2 != Vector2.zero ) 
            m_Rigidbody2D.AddForce(_ForceV2);
    }

    #region Dash Event
    protected void OnDash(Vector2 _V2)
    {
        m_Rigidbody2D.velocity = _V2;
    }

    protected void OnDashFin()
    {
        SetHitCase(false);
    }
    #endregion
}
