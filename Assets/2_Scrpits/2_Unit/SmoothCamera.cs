﻿using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour {


	public Transform m_Target = null;
	public float _fX = 0f ;
	private Vector3 m_DistanceV3 = Vector3.zero;

	// Use this for initialization
	void Start () {
		if (!m_Target)
			enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_DistanceV3 = ( m_Target.position - transform.position ) ;
		m_DistanceV3.y = m_DistanceV3.y /1.25f + 4f ;
		m_DistanceV3.x = m_DistanceV3.x * _fX ;
		m_DistanceV3.z = 0;

        transform.position += m_DistanceV3 * Time.deltaTime * 2f   ;

	}
}


