using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private CharacterStates characterStates;
    private GameObject attackTarget;  //攻击目标
    private float lastAttactTime;  //最后攻击时间
    private bool isDead;  //死亡
    private float stopDistance; //agent 停止距离

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStates = GetComponent<CharacterStates>();
        stopDistance = agent.stoppingDistance;
    }

    private void OnEnable()
    {
        //当人物出现的时候,在 OnMouseClicked 事件中注册 方法
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttact;
        GameManager.Instance.RigisterPlayer(characterStates);
    }

    private void Start()
    {
        SaveManager.Instance.LoadPlayerData();
    }

    private void OnDisable()
    {
        //当人物消失，取消订阅
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;
        MouseManager.Instance.OnEnemyClicked -= EventAttact;
    }

    private void Update()
    {
        SwitchAnimation();
        lastAttactTime -= Time.deltaTime;
        isDead = characterStates.CurrentHealth == 0;
        if (isDead)
        {
            GameManager.Instance.NotifyObservers();
        }
    }

    //移动到鼠标点击位置
    private void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        if (isDead) return;
        agent.isStopped = false;  //重置agent为可移动 
        agent.stoppingDistance = stopDistance;
        agent.destination = target;
    }

    //动画切换
    private void SwitchAnimation()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
        animator.SetBool("Dead", isDead);
    }

    //攻击事件
    private void EventAttact(GameObject target)
    {
        if (isDead) return;
        if (target != null)
        {
            attackTarget = target;
            characterStates.isCritical = UnityEngine.Random.value <= characterStates.attackData.criticalChance;  //暴击判断
            StartCoroutine(MoveToAttactTarget());
        }
    }

    //朝攻击目标移动
    IEnumerator MoveToAttactTarget()
    {
        agent.isStopped = false;  //可以移动
        agent.stoppingDistance = characterStates.attackData.attackRange;
        transform.LookAt(attackTarget.transform);
        //攻击范围判断
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStates.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        agent.isStopped = true;  //停止移动
        //攻击
        if (lastAttactTime < 0)
        {
            animator.SetBool("Critical", characterStates.isCritical);
            animator.SetTrigger("Attact");
            //重置攻击冷却时间
            lastAttactTime = characterStates.attackData.attackCoolDown;
        }
    }

    //Animation Event
    void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
            if (attackTarget.GetComponent<Golem_Rock>() && attackTarget.GetComponent<Golem_Rock>().rockStates == Golem_Rock.RockStates.HitNothing)
            {
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
                attackTarget.GetComponent<Golem_Rock>().rockStates = Golem_Rock.RockStates.HitEnemy;
            }
        }
        else
        {
            var targetStates = attackTarget.GetComponent<CharacterStates>();
            targetStates.TakeDamage(characterStates, targetStates);
        }
    }
}