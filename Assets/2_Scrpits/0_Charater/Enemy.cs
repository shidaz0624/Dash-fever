using UnityEngine;
using System.Collections;

public class Enemy : CharaterBase {

	// Use this for initialization
    protected override void Start ()
    {
        base.Start ();
    }
    int i = 1;
	// Update is called once per frame
    protected override void Update ()
    {
        base.Update ();

    }
    public Transform m_Right;
    public Transform m_Left;
    void FixedUpdate()
    {
        float _fHorizontal = .5f * Time.fixedDeltaTime * 10 ;
        Transform _tf = (i > 0)? m_Right : m_Left; 
//        Debug.LogError(_tf.gameObject.name);
//        RaycastHit2D _Hit = Physics2D.Raycast( _tf.position , new Vector2( i , 0f ) , 2f);
        RaycastHit2D[] _Hit = Physics2D.RaycastAll( _tf.position , new Vector2( i , 0f ) , 2f);
        bool _isFlip = false;
        for (int x = 0 ; x < _Hit.Length ; x++)
        {
        if (_Hit[x].collider != null)
        {   
            
//            Debug.LogError("GOD!!!" + _Hit[x].transform.gameObject.name);
            if ( _Hit[x].transform.gameObject.tag == "Ground")
            {
                i *= -1;
                    Debug.LogError("FLIP!!!!! : " + i);
                _isFlip = true;
                    FlipTrigger();
            }
        }
        }

//        if (_fHorizontal != 0)
//        {
//            if (_fHorizontal > 0 && GetFlip < 0 || _isFlip)
//            {
//                FlipTrigger();
//            }
//            else if (_fHorizontal < 0 && GetFlip > 0 || _isFlip)
//            {
//                FlipTrigger();
//            }
//        }



        Vector2 _v2 = new Vector2( _fHorizontal * i , 0);
        Move(_v2);
    } 

    void OnTriggerEnter2D(Collider2D other)
    {                
//        if ( other.gameObject.tag == "Player")
//            Debug.LogError("Fuck");
//        if ( other.gameObject.tag == "Player" && m_HitCase.m_isEnabled)
//        {
//            other.GetComponent<CharaterBase>().GetDamage( m_HitCase.m_iDamage , m_Rigidbody2D.velocity * 5 );
//        }
    }
}
