using UnityEngine;
using System.Collections;

public class SpriteMaskEffectExample : MonoBehaviour {

    public Renderer m_Renender;
    Material m_Material;
    float m_time;
    void Start()
    {        
        if (m_Renender == null)
            m_Renender = GetComponent<Renderer>();
        if (m_Renender == null)
        {
            enabled = false;
            return;
        }
        m_Material = m_Renender.material;

    }
    void Update ()
    {
        m_time += Time.deltaTime * 0.5f;
        float _a = Mathf.PingPong(m_time  , 1f);
        m_Material.SetFloat("_MaskAlphaValue" , _a);
    }
}
