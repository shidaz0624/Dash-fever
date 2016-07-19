//using UnityEngine;
//using System.Collections;
//
//public class Ai_Mono : MonoBehaviour 
//{
//	[System.Serializable]
//	//角色參數class
//	public class CharaterParameter
//	{
//		public int 		m_iOriginalHealthPoint 	= 0;
//		public float	m_fOriginalSpeedPoint	= 0f;
//		public int		m_iOriginalJumpPower	= 0;
//		public float	m_fOriginalSprintPower	= 0f;
//		public float	m_fOriginalSprintSpeed	= 0f;
//		public float	m_fOriginalDamagePower	= 30f;
//		public float	m_fOriginalDamageSpeed	= 5f;
//		public float    m_fOriginalHorizon		= 20f;
//		public float    m_fOriginalAttackDist	= 6f;
//
//		public CharaterParameter()
//		{}
//	}
//	[Header ("角色參數class")]
//	public CharaterParameter m_CharaterParameter = new CharaterParameter();
//	
//
//	protected Charater 		m_Charater 			= null;
//	public 	Ai_State_Base 	m_NowState 			= null;	//此角色目前的ai狀態容器
//	public 	Transform 		m_TargetTf;			 		//常用的目標transform
//
//	private int 			m_iMaxPluginCount	= 10;
//	private Ai_Plugin_Base[] m_PluginAry 		= null; //角色外掛模組插槽陣列
//	public 	Ai_Action_Base 	m_Action 		 	= null; //因角色攻擊動作較頻繁使用，故將此插件獨立出來
//	public  Ai_Plugin_AnimatorControl  m_Animator = null;
//
//	private bool 		m_isFacingRight 		= true;	//角色是否面向畫面右邊
//	private bool 		m_isGround 				= false;//角色是否接觸地板
//	private float 		m_fGroundRadius 		= 0.1f; //角色偵測地板的距離
//	public 	Transform 	m_GroundTag 			= null; //角色偵測地板的射線起始點物件
//	public 	LayerMask 	m_GroundLayerMask; 		 		//地板圖層
//
//	public 	Charater	CharaterSc		{ get{ return m_Charater;} }
//	public 	bool 		IsFacingRight	{ get{ return m_isFacingRight; }}
//	public 	bool 		IsGround		{ get{ return m_isGround; }  		set{ m_isGround = value; } }
//	public 	float 		fGroundRadius	{ get{ return m_fGroundRadius; } 	set{ m_fGroundRadius = value; }}
//
//	private Rigidbody2D m_Rigidbody2D = null;
//	public Rigidbody2D 	GetRigidbody2D	{ get{ return m_Rigidbody2D; } }
//
//	void Awake()
//	{
//		m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
//	}
//
//	public virtual void Start()
//	{
//
//	}
//
//	public Ai_State_Base NowState
//	{
//		get{ return m_NowState; }
//		set
//		{
//			m_NowState.OnAiDestory();
//			m_NowState = value;
//			m_NowState.OnAiStart();
//		}
//	}
//
//	public virtual void Flip()
//	{
//		m_isFacingRight = !m_isFacingRight;
//		Vector3 _v3 = new Vector3( (m_isFacingRight)? 1 : -1 , 1 , 1 );
//		transform.localScale = _v3;
//	}
//
//	public virtual void Update()
//	{
//
//
//		if (m_PluginAry != null)
//		{
//			for(int i = 0 ; i < m_PluginAry.Length ; i ++)
//			{
//				if (m_PluginAry[i] != null && m_PluginAry[i].m_isUpdate)
//				{
//					m_PluginAry[i].OnPluginUpdate();
//				}
//			}
//		}
//
//		if (m_Action != null)
//			m_Action.OnPluginUpdate();
//	}
//
//	public virtual void FixedUpdate()
//	{
//		if (m_NowState != null)
//			m_NowState.OnAiUpdate();
//
//		if (m_PluginAry != null)
//		{
//			for(int i = 0 ; i < m_PluginAry.Length ; i ++)
//			{
//				if (m_PluginAry[i] != null && m_PluginAry[i].m_isFixedUpdate)
//				{
//                    m_PluginAry[i].OnPluginUpdate();
//                }
//            }
//        }
//	}
//
//	/// <summary>
//	/// //判斷此角色是否在地板上，並回傳.
//	/// </summary>
//	public bool ProcessingIsGround()
//	{
//		m_isGround = Physics2D.OverlapCircle(m_GroundTag.position , fGroundRadius , m_GroundLayerMask);
//		return m_isGround;
//	}
//
//
//	public virtual void GetDamage(AttackData _AttackData)
//	{
//
//	}
//	
//	/// <summary>
//	/// 刪掉目前的攻擊動作
//	/// </summary>
//	public virtual void DelAction()
//	{
//		m_Action.OnPluginEnd();
//		m_Action = null;
//	}
//
//	/// <summary>
//	/// //在角色的插件插槽中，取得目標的插件.
//	/// </summary>
//	public  Ai_Plugin_Base FindPluginInPlugAryByPlugin(Ai_Plugin_Base _PluginValue)
//	{
//		if (m_PluginAry == null || m_PluginAry.Length == 0 || _PluginValue == null)
//			return null;
//		
//		for (int i = 0 ; i < m_PluginAry.Length ; i++)
//		{
//			if ( m_PluginAry[i] == _PluginValue)	
//			{
//				return m_PluginAry[i];
//			}
//		}
//		return null;
//	}
//
//	public bool AddPluginToPluginAryByPlugin(Ai_Plugin_Base _PluginValue)
//	{
//		//若插件插槽尚未被初始化，則初始化此角色限制的插槽數量
//		if (m_PluginAry == null)
//		{
//			m_PluginAry = new Ai_Plugin_Base[m_iMaxPluginCount];
//		}
//
//		for (int i = 0 ; i < m_PluginAry.Length ; i++)
//		{
//			if ( m_PluginAry[i] == null )
//			{
//				//成功找到空的插件擦槽，插入後回傳true
//				m_PluginAry[i] = _PluginValue;
//				return true;
//			}
//		}
//
//		//找不到空的插件插槽，插件插入失敗，回傳false
//		return false;
//	}
//
//	public bool RemovePluginInPluginAryByPlugin( Ai_Plugin_Base _PluginValue )
//	{
//
//		if (_PluginValue == null || m_PluginAry == null) return false;
//
//		bool _isAllNull = true;
//		bool _isRemoveThisTime = false;
//		for (int i = 0 ; i< m_PluginAry.Length ; i ++)
//		{
//			if (m_PluginAry[i] != null)
//			{
//				if ( m_PluginAry[i].Equals ( _PluginValue ))
//				{
//					m_PluginAry[i].OnPluginEnd();
//					m_PluginAry[i] = null;
//					_isRemoveThisTime = true;
//				}
//				else
//				{
//					_isAllNull = false;
//				}
//			}
//		}
//
////		if (_isAllNull)
////		{
////			m_PluginAry = null;
////		}
//
//		return _isRemoveThisTime;
//	}
//
//	public bool IsHasAnyPlugin()
//	{
//		if (m_PluginAry == null) return false;
//
//		for (int i = 0 ; i < m_PluginAry.Length ; i++)
//		{
//			if (m_PluginAry[i] != null)
//			{
//				return true;
//			}
//		}
//		return false;
//	}
//
////	[System.Serializable]
////	public class Ai_StateJob
////	{
////
////		public Ai_StateJob() 
////		{
////
////		}
////		public Ai_State_Base.Ai_State m_StateName;
////		private Ai_State_Base m_State = null;
////		[System.Serializable]
////		public class Exit
////		{
////			public Ai_State_Base.Ai_State m_TargetState;
////			public enum MoreOrLess
////			{
////				NONE,
////				MORE,
////				LESS,
////			}
////			public MoreOrLess m_MoreOrLess = MoreOrLess.NONE;
////			public int m_iCondition = 0;
////
////			public enum TrueOrFalse
////			{
////				NONE,
////				TRUE,
////				FALSE,
////			}
////			public bool m_TrueOrFalse = true;
////		}
////		public Exit[] m_ExitAry = null;
////	}
//}
