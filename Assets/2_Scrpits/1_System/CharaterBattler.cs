using UnityEngine;
using System.Collections;

public class CharaterBattler : SystemBase 
{
    #region const
    private const string TAG_DOOR   = "Door";
    private const string TAG_PLAYER = "Player";
    private const string TAG_ENEMY  = "Enemy";
    #endregion

    public void PassDamageClassToCharater( CharaterBase _From , DamageClass _DamageClass , CharaterBase _To )
    {        
        if (_To.m_CharaterParameter.GetIsDeath) return;

        string _sToTag = _To.gameObject.tag;
        switch (_sToTag)
        {            
        case TAG_ENEMY:
            DamageToEnemy(_From , _DamageClass , _To);
            break;
        case TAG_PLAYER:
            DamageToPlayer(_From , _DamageClass , _To);
            break;
        case TAG_DOOR:
            DamageToDoor(_From , _DamageClass , _To);
            break;
        }
            
    }

    private void DamageToEnemy(CharaterBase _From , DamageClass _DamageClass , CharaterBase _To)
    {
        DamageToPlayer( _From , _DamageClass , _To );
    }

    private void DamageToPlayer(CharaterBase _From , DamageClass _DamageClass , CharaterBase _To)
    {
        if (_To.m_DefenceCase.IsDefence && (_To.GetFlip != _DamageClass.m_iSide))
        {
            //Do 防禦成功
            _To.ProcessDefence( _DamageClass );
        }
        else
        {
            //Do 受到傷害
            _To.ProcessHealthPoint( - _DamageClass.m_iDamage );

            if ( _To.m_CharaterParameter.GetHealthPoint <= 0 )
                _To.PlayDeathEffect();
            else
                _To.GetDamage(_DamageClass);

            CreateDamagePoint(_To.transform , _DamageClass.m_iDamage);

            if (_From.tag == TAG_PLAYER)
                this.UpdateComboByPlusValue( 1 );
        }
    }

    private void DamageToDoor(CharaterBase _From , DamageClass _DamageClass , CharaterBase _To)
    {
        //目前只有玩家可以對門造成傷害
        if (_From.tag == TAG_PLAYER)
            DamageToPlayer( _From , _DamageClass , _To );
    }

    private void CreateDamagePoint( Transform _Transform , int _iDamage )
    {
        ObjectPool.m_MonoRef.GetObject(ObjectPool.ObjectPoolID.DAMAGE_HUD).GetComponent<DamageHUD>().Play(_Transform , _iDamage.ToString());
    }       

    private void UpdateComboByPlusValue( int _iValue )
    {
        MainGameHost.MonoRef.UpdateComboByPlusValue( _iValue );
    }
}
