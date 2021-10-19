using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { GUARD, PATROL, CHASE, DEAD }
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Collider collider;
    private CharacterStates characterStates;
    private EnemyStates enemyStates;  //状态
    private EnemyStates baseStates;  //基础状态

    [Header("Basic Settings")]
    public bool isPatrol;  //不是巡逻就是站桩
    public float sightRadius;  //可视半径
    private GameObject attackTarget;  //攻击目标
    private float baseSpeed;  //移动速度

    [Header("Patrol State")]
    public float patrolRadius;  //巡逻半径
    private Vector3 basePosition;  //巡逻范围原点
    private Vector3 randomPatrolPoint; //随机巡逻点
    public float patrolInterval;  //原始巡逻间隔
    private float lastPatrolTime;  //最新巡逻时间
    private Quaternion baseRotation; //原始朝向

    //动画
    bool isWalk;  //走路
    bool isChase;  //追击
    bool isFollow;  //跟随

    private float lastAttactTime;  //最后攻击时间

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        characterStates = GetComponent<CharacterStates>();
        baseSpeed = agent.speed;
        basePosition = transform.position;  //获取原始位置
        baseRotation = transform.rotation;  //获取原始朝向
        lastAttactTime = 0;
        lastPatrolTime = 0;
    }

    private void Start()
    {
        //判断敌人类型, 巡逻还是站桩
        if (isPatrol)
        {
            baseStates = EnemyStates.PATROL;
            GetNewWayPoint();  //获取初始巡逻点
        }
        else
        {
            baseStates = EnemyStates.GUARD;
        }
    }

    private void Update()
    {
        SwitchStates();
        SwitchAnimation();
        lastAttactTime -= Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(basePosition, patrolRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }

    private void SwitchAnimation()
    {
        animator.SetBool("Walk", isWalk);
        animator.SetBool("Chase", isChase);
        animator.SetBool("Follow", isFollow);
        animator.SetBool("Critical", characterStates.isCritical);
        animator.SetBool("Dead", characterStates.isDead);
    }

    private void SwitchStates()
    {
        //判断是否死亡
        if (characterStates.isDead)
        {
            enemyStates = EnemyStates.DEAD;
        }
        //发现 Player 切换到 CHASE 状态
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
            isChase = true;  //开始追击
        }
        //脱战, 返回追击前的状态
        else
        {
            enemyStates = baseStates;
            isChase = false;  //退出追击
        }

        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                if (Vector3.Distance(transform.position, basePosition) > agent.stoppingDistance)
                {
                    isWalk = true;
                    agent.destination = basePosition;  //回到最初位置
                }
                else
                {
                    isWalk = false;
                    transform.rotation = Quaternion.Lerp(transform.rotation, baseRotation, 0.01f);  //缓慢复原朝向
                }
                break;
            case EnemyStates.PATROL:
                agent.destination = transform.position;  //停止移动
                agent.speed = baseSpeed * 0.5f;  //移动速度减半
                //判断是否到达巡逻点
                if (Vector3.Distance(transform.position, randomPatrolPoint) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    //巡逻间隔
                    if (lastPatrolTime > patrolInterval)
                    {
                        lastPatrolTime = 0;  //重置巡逻间隔
                        GetNewWayPoint();  //获取新的巡逻点
                    }
                    else
                    {
                        lastPatrolTime += Time.deltaTime;
                    }
                }
                else
                {
                    isWalk = true;
                    agent.destination = randomPatrolPoint;  //前往巡逻点
                }
                break;
            case EnemyStates.CHASE:
                agent.speed = baseSpeed;  //移动速度复原
                agent.destination = attackTarget.transform.position;  //追击攻击目标
                //是否进入攻击范围
                if (Vector3.Distance(transform.position, attackTarget.transform.position) <= characterStates.attackData.attackRange)
                {
                    isFollow = true;
                    //攻击CD
                    if (lastAttactTime < 0)
                    {
                        lastAttactTime = characterStates.attackData.coolDown;  //重置攻击CD
                        characterStates.isCritical = Random.value <= characterStates.attackData.criticalChance;  //暴击判断
                        transform.LookAt(attackTarget.transform);  //面朝攻击目标
                        animator.SetTrigger("Attack");
                    }
                }
                else
                {
                    isFollow = false;
                }
                break;
            case EnemyStates.DEAD:
                collider.enabled = false;  //关闭碰撞体，防止死亡状态还可以被攻击
                agent.enabled = false;  //关闭导航组件
                Destroy(gameObject, 2f);
                break;
        }
    }

    //检测敌人是否进入追击范围
    private bool FoundPlayer()
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

    //获取巡逻范围内的一个随机点
    private void GetNewWayPoint()
    {
        float randomX = Random.Range(-patrolRadius, patrolRadius);
        float randomZ = Random.Range(-patrolRadius, patrolRadius);
        NavMeshHit hit;
        Vector3 randomPoint = new Vector3(basePosition.x + randomX, basePosition.y, basePosition.z + randomZ);
        //随机巡逻点的 Areas 应该为 walkable
        randomPatrolPoint = NavMesh.SamplePosition(randomPatrolPoint, out hit, 1f, 1) ? randomPoint : transform.position;
    }

    //Animation Event
    void Hit()
    {
        if (attackTarget != null)
        {
            var targetStates = attackTarget.GetComponent<CharacterStates>();
            targetStates.TakeDamage(characterStates, targetStates);
        }
    }
}