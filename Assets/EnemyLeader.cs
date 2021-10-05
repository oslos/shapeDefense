using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLeader : Enemy
{
     private bool _shieldbounceinprogress;
     protected override void HandleShieldCollision()
    {
      StartCoroutine(ShieldBounce());
    }

     private IEnumerator ShieldBounce() {
        
        if (_shieldbounceinprogress) {
            yield break;
        }
        
        _shieldbounceinprogress = true;
        BlowOffEffect();

        yield return new WaitForSeconds(2);

        _shieldbounceinprogress = false;

     }

    protected override bool CanMove()
    {
      return !_isFreezed && !_isTornadoed && !_shieldbounceinprogress; 
    }
}
