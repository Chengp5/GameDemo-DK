using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem: EnemyController
{

    public float kickForce = 25;
    public GameObject rockPrefab;

    public Transform handPos;
    public void KickOff()
    {
        if (attackTarget != null && transform.isFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharaterStats>();
            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            targetStats.takeDamage(charaterStats, targetStats);
        }
    }

    public void ThrowRock()
    {
        if(attackTarget!=null)
        {
            var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
            rock.GetComponent<Rock>().target = attackTarget;
        }
    }
}
