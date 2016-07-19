//using UnityEngine;
//using System.Collections;
//
//public class Hero_Mono : Ai_Mono {
//
//	#region Plugin
//	private Ai_Plugin_InputHandler m_InputHandler = null;
//	#endregion
//
//	public bool m_isCanAirSprint = false;
//
//	// Use this for initialization
//	public override void Start () 
//	{
//		//將玩家操作的class掛入此物件
//		m_InputHandler 	= new Ai_Plugin_InputHandler(this);
//		//初始化 動畫元件
//		m_Animator 		= new Ai_Plugin_AnimatorControl(this);
//
//		m_Charater = new Charater(this);
//
//		Hero_State_Idle _Idle = new Hero_State_Idle(m_Charater);
//		m_NowState = _Idle;
//	}
//
//	public override void Update ()
//	{
//		base.Update ();
//
//	}
//
//	// Update is called once per frame
//	private void FixedUpdate () 
//	{
//		//設置角色是否觸碰地板
//		base.ProcessingIsGround();
//
//		//控制玩家操作反應
//		m_InputHandler.OnPluginUpdate();
//
//		if (m_InputHandler != null)
//		{
//			m_Animator.SetBool( "isJump" , m_InputHandler.GetIsJump );
//			m_Animator.SetBool( "isGround" , IsGround );
//			if (!m_InputHandler.GetIsJump && IsGround)
//				m_Animator.SetFloat( "fHorizontal" , Mathf.Abs( m_InputHandler.GetHorizontalValue ));
//			else
//				m_Animator.SetFloat( "fHorizontal" , 0);
//			m_Animator.SetFloat( "fVertical" , GetRigidbody2D.velocity.y );
//		}
//
//		base.FixedUpdate();
//	}
//
//	private void DebugLog()
//	{
//		Debug.LogError("m_isGroung = " + base.IsGround);
//	}
//
//	public override void GetDamage (AttackData _AttackData)
//	{
//		//取得此物件與_Sender的方向 
//		int _iFacing = (_AttackData.Sender.transform.position.x > transform.position.x)? -1 : 1;
//		//依照攻擊的力矩,設置力道
//		Vector2 _MoveV2 = new Vector2(_AttackData.AttackForceV2.x * _iFacing, 0f);
//		Vector2 _ForceV2 = new Vector2(0f ,_AttackData.AttackForceV2.y);
//		//將強制移動的plugin掛入此物件
//		Ai_Plugin_Move _Move = new Ai_Plugin_Move(this , _MoveV2 , _ForceV2 , _AttackData.fMoveTime , true , false);
//		//開始使用plugin強制移動此物件
//		base.AddPluginToPluginAryByPlugin(_Move);
//
//		m_Animator.Play("Hero_Chan_Damage");
//		Invoke("CancleDamage" , _AttackData.fMoveTime*1.5f);
//	}
//	private void CancleDamage()
//	{
//		m_Animator.Play("Hero_Chan_Idle");
//	}
//
//
//#if GIZMOS
//	void OnDrawGizmosSelected() {
//		Gizmos.color = new Color(1, 0, 0, 0.5F);
//		
//		int _iFacing = (IsFacingRight)? 1: -1;
//		
//		Gizmos.DrawWireCube(transform.position + new Vector3(4,0,0) * _iFacing, new Vector3(4,4,0) * _iFacing);
//	}
//#endif
//}
//
//
//
//
//
