using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageHUD : MonoBehaviour {

    public Animator m_Animator = null;
    public Text m_Text = null;

    public void Play(Transform _Pos , string _sText)
    {
        transform.position = _Pos.position + new Vector3(Random.Range(0f,1.5f),Random.Range(0f,3f),0f);
        m_Text.text = _sText;
        gameObject.SetActive(true);
        m_Animator.SetTrigger("Trigger");
    }

    public void AnimationFin()
    {
        Debug.Log("AnimationFin");

        ObjectPool.m_MonoRef.ReleaseObject(gameObject , ObjectPool.ObjectPoolID.DAMAGE_HUD);
    }
}
