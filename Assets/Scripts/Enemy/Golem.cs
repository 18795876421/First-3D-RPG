using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("Skill")]
    public float kickForce = 10;
    public GameObject prefabRock;
    public Transform positionPalm;

    //击退
    public void KickOff()
    {
        //目标不为空，且在正面120°之内才造成伤害
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {

            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;

            var targetStates = attackTarget.GetComponent<CharacterStates>();
            targetStates.TakeDamage(characterStates, targetStates);

        }
    }

    //仍石头
    public void ThrowRock()
    {
        var rock = Instantiate(prefabRock, positionPalm.position, Quaternion.identity);
        rock.GetComponent<Golem_Rock>().target = attackTarget;
    }
}
