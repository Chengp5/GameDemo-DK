using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private GameObject attackTarget;
    private float lastAttackTime;

    private bool isDead;
    private CharaterStats charaterStats;
    private float stopDistance;



    void Awake()
    {
        agent=GetComponent<NavMeshAgent>();
        animator=GetComponent<Animator>();
        charaterStats = GetComponent<CharaterStats>();
        isDead = false;
        stopDistance = agent.stoppingDistance;
    }
    private void OnEnable()
    {
        MouseManager.Instance.OnMouseClicked += moveToTarget;
        MouseManager.Instance.OnEnemyClicked += eventAttack;
        GameManager.Instance.registerPlayer(charaterStats);
    }
    private void Start()
    {
       
        SaveManager.Instance.loarPlayerData();
    }
    private void OnDisable()
    {
        MouseManager.Instance.OnMouseClicked -= moveToTarget;
        MouseManager.Instance.OnEnemyClicked -= eventAttack;
    }
    private void Update()
    {
        isDead = charaterStats.CurrentHealth == 0;
        if(isDead)
        {
            GameManager.Instance.NotifyAllObservers();
        }
        switchAnimations();
        lastAttackTime -= Time.deltaTime;
    }
    void switchAnimations()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
        animator.SetBool("Death", isDead);
    }
    void moveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        if (isDead) return;
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target;
    }
    void eventAttack(GameObject gameObject)
    {
        if (isDead) return;
        if (gameObject != null)
        {
            charaterStats.isCritical = Random.value < charaterStats.attackData.criticalChance;
            attackTarget = gameObject;
           // Debug.Log("attack");
            StartCoroutine(moveToAttack());
        }
    }

    IEnumerator moveToAttack()
    {
        //move
       
        agent.isStopped = false;
        agent.stoppingDistance = charaterStats.attackData.attackRange;
        while(Vector3.Distance(attackTarget.transform.position,transform.position)>charaterStats.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        //transform.rotation = Quaternion.Lerp(transform.rotation,,0.01f);
        agent.isStopped = true;
        //attack

        //Debug.Log("Iattack2");
        if (lastAttackTime < 0)
        {
            Debug.Log("Iattack3");
            //transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler((attackTarget.transform.position-transform.position).normalized),0.01f);
            //transform.LookAt(Vector3.Lerp(transform.position,(attackTarget.transform.position - transform.position).normalized,0.01f));
            //FIXME: 攻击时朝向不正常的BUG,使用lookAt 太突兀
            transform.LookAt(attackTarget.transform);
            animator.SetBool("Critical", charaterStats.isCritical);
            animator.SetTrigger("Attack");
            lastAttackTime = charaterStats.attackData.coolDown;
        }
    }

    //Animation event

    void hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
            if (attackTarget.GetComponent<Rock>() && attackTarget.GetComponent<Rock>().currentState == Rock.RockStates.HitNothing)
            {
                attackTarget.GetComponent<Rock>().currentState = Rock.RockStates.HitEnemy;

                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }
        }
        else
        {
            var targetStats = attackTarget.GetComponent<CharaterStats>();
            targetStats.takeDamage(charaterStats, targetStats);
        }
    }
   
}
