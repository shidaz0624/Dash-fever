using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageHUD : MonoBehaviour {

    #region const 
    private Vector3 RANDOM_POS_V3{ get{return new Vector3(Random.Range(0f,1.5f),Random.Range(0f,3f),0f);} }
    #endregion

    public Animator m_Animator = null;
    public Text m_Text = null;

    public void Play(Transform _Pos , string _sText)
    {
        transform.position = _Pos.position + RANDOM_POS_V3;
        m_Text.text = _sText;
        gameObject.SetActive(true);
        m_Animator.SetTrigger("Trigger");
    }

    public void AnimationFin()
    {
        this.ReleaseObject();
    }

    private void ReleaseObject()
    {
        ObjectPool.m_MonoRef.ReleaseObject(gameObject , ObjectPool.ObjectPoolID.DAMAGE_HUD);
    }
}
