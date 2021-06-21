using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public enum enemyState
{
    GUARD,
    PATROL,
    CHASE,
    DEAD
};


[RequireComponent(typeof(NavMeshAgent))]

[RequireComponent(typeof(CharaterStats))]
public class EnemyController : MonoBehaviour ,IEndGameObserver
{
    private NavMeshAgent agent;
    public enemyState currentState;

    protected CharaterStats charaterStats;

    [Header("Basic Setting")]
    public float sightRadius;

    public float lookAtTime;

    private float remainLookAtTime;

    private float lastAttackTime;

    private Collider coll;

    

    [Header("Patrol State")]
    public float patrolRange;

    private Vector3 wayPoint;

    private Vector3 guardPos;

    public bool isGuard;

    private float speed;

    protected GameObject attackTarget;

    private Quaternion guardRotation;

    private Animator anim;

    bool isWalk;
    bool isChase;
    bool isFollow;
    bool isDead;
    bool isPlayerDead;


    //FIXME: 切换场景时使用
    void OnEnable()
    {
        if (!GameManager.IsIniatialed) return;
        GameManager.Instance.addObserver(this);
    }
    void OnDisable()
    {
        if (!GameManager.IsIniatialed) return;
            GameManager.Instance.removerObserver(this);
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        anim = GetComponent<Animator>();
        guardPos = transform.position;
        guardRotation = transform.rotation;
        remainLookAtTime = lookAtTime;
        charaterStats = GetComponent<CharaterStats>();
        coll = GetComponent<Collider>();
    }

    private void Start()
    {
        //FIXME: 在实现场景切换后，修改
        GameManager.Instance.addObserver(this);
        if (isGuard)
        {
            currentState = enemyState.GUARD;
        }
        else
        {
            currentState = enemyState.PATROL;
            getNewWayPoint();
        }
    }
    
    private void Update()
    {
        if (charaterStats.CurrentHealth == 0)
            isDead = true;

        if (!isPlayerDead)
        {
            switchStates();
            switchAnimation();
            //为所有计时器计时
            counting4AllTimer();
        }
           
    }
    void counting4AllTimer()
    {
        lastAttackTime -= Time.deltaTime;
    }
    void switchStates()
    {
        if (isDead)
            currentState = enemyState.DEAD;

        else if (findPlayer())
        {
            currentState = enemyState.CHASE;
        }

        switch(currentState)
        {
            case enemyState.GUARD:
                guardAction();
                break;
            case enemyState.CHASE:
                chaseAction();
                break;
            case enemyState.DEAD:
                deadAction();
                break;
            case enemyState.PATROL:
                patrolAction();
                break;
        }
    }

    void switchAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Critical", charaterStats.isCritical);
        anim.SetBool("Death", isDead);
    }

    void guardAction()
    {
        isChase = false;
        if (transform.position != guardPos)
        {
            isWalk = true;
            agent.isStopped = false;
            agent.destination = guardPos;
         
            if(Vector3.SqrMagnitude(guardPos-transform.position)<=agent.stoppingDistance)
            {   
                isWalk = false;
                //transform.rotation = guardRotation;
               transform.rotation = Quaternion.Lerp(transform.rotation,guardRotation,0.01f);
            }
        }
    }
    void patrolAction()
    {
        isChase = false;
        agent.speed = speed * 0.5f;
        if(Vector3.Distance(transform.position,wayPoint)<=agent.stoppingDistance)
        {
            isWalk = false;
            if (remainLookAtTime > 0)
                remainLookAtTime -= Time.deltaTime;
            else
            {
                getNewWayPoint();
            }
            
        }
        else
        {
            isWalk = true;
            agent.destination = wayPoint;
        }
    }
    //追击状态采取的行动
    void deadAction()
    {
        //FIXME: hou qi xiu gai 
        coll.enabled = false;
        agent.radius = 0;
       
        Destroy(gameObject, 2f);
    }
    bool targetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position)<=charaterStats.attackData.attackRange;
        return false;
    }
    bool targetInSkillRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= charaterStats.attackData.skillRange;
        return false;
    }
    void chaseAction()
    {

        agent.speed = speed;
        isWalk = false;
        isChase = true;
        if(!findPlayer())
        {
            isFollow = false;
            agent.destination = agent.transform.position;
            if (isGuard)
                currentState = enemyState.GUARD;
            else
                currentState = enemyState.PATROL;
        }
        else
        {
            agent.isStopped = false;
            isFollow = true; 
            agent.destination = attackTarget.transform.position;
        }
        if(targetInAttackRange()||targetInSkillRange())
        {
            isFollow = false;
            agent.isStopped = true;
            if(lastAttackTime<=0)
            {
                lastAttackTime = charaterStats.attackData.coolDown;

                charaterStats.isCritical = Random.value < charaterStats.attackData.criticalChance;
                attack();
            }
        }
    }
    void attack()
    {
        transform.LookAt(attackTarget.transform);
        if(targetInAttackRange())
        {
            //Debug.Log("in attack range");
            anim.SetTrigger("Attack");
        }
        if(targetInSkillRange())
        {
            //Debug.Log("in skill range");
            anim.SetTrigger("Skill");
        }
    }
    bool findPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;

            }
                
        }
        attackTarget = null;
        return false;
    }

    void getNewWayPoint()
    {
        remainLookAtTime = lookAtTime;
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);
        Vector3 randomPoint = new Vector3(guardPos.x + randomX,
            transform.position.y, 
            guardPos.z + randomZ);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1))
        {
            wayPoint = hit.position;
        }
        else
        {
            wayPoint = transform.position;
        }
    }


    //Animation event

    void hit()
    {
        if (attackTarget != null&&transform.isFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharaterStats>();
            targetStats.takeDamage(charaterStats, targetStats);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
        //Gizmos.DrawWireSphere(transform)
    }

    public void EndNotify()
    {
        //TODO: 游戏结束，停止enemy。。。。
        isChase = false;
        isPlayerDead = true;
        isWalk = false;
        attackTarget = null;
        anim.SetBool("Win", true);
    }
}
