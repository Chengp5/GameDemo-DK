using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockStates
    {
        HitPlayer,
        HitEnemy,
        HitNothing
    }
    public GameObject rockBreakEffect;

    public RockStates currentState;
    private Rigidbody rb;
    public int damage;

    [Header("Basic Settings")]
    public float force;

    public GameObject target;

    private Vector3 direction;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;
        FlyToTarget();
        currentState = RockStates.HitPlayer;
    }
    private void FixedUpdate()
    {
        if(rb.velocity.sqrMagnitude<1f)
        {
            currentState = RockStates.HitNothing;
        }
    }
    public void FlyToTarget()
    {
        if (target == null)
            target = FindObjectOfType<PlayerController>().gameObject;
        direction = (target.transform.position - transform.position+Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch(currentState)
        {
            case RockStates.HitPlayer:
                hitPlayerAction(collision);
                break;
            case RockStates.HitEnemy:
                hitEnemyAction(collision);
                break;
            case RockStates.HitNothing: break;
        }
    }

    void hitPlayerAction(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            collision.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;
            collision.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
            collision.gameObject.GetComponent<CharaterStats>().takeDamage(damage, collision.gameObject.GetComponent<CharaterStats>());
            currentState = RockStates.HitNothing;
        }
    }
    void hitEnemyAction(Collision collision)
    {
        if(collision.gameObject.GetComponent<Golem>())
        {
            var otherStats = collision.gameObject.GetComponent<CharaterStats>();
            otherStats.takeDamage(damage, otherStats);
            Instantiate(rockBreakEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
