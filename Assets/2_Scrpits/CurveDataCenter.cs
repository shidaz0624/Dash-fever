using UnityEngine;
using System.Collections;

public class CurveDataCenter : MonoBehaviour {

    private static CurveDataCenter m_MonoRef = null;
    public  static CurveDataCenter MonoRef{ get{return m_MonoRef;} }

    public AnimationCurve m_CurveZeroToOne;

	// Use this for initialization
	void Start () {
        m_MonoRef = this;    
	}
	
}
