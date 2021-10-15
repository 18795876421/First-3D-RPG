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
    private GameObject attactTarget;  //攻击目标
    private float lastAttactTime;  //最后攻击时间

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterStates = GetComponent<CharacterStates>();
    }

    private void Start()
    {
        //在 OnMouseClicked 事件中注册 方法
        MouseManage.Instance.OnMouseClicked += MoveToTarget;
        MouseManage.Instance.OnEnemyClicked += EventAttact;
    }

    private void Update()
    {
        SwitchAnimation();
        lastAttactTime -= Time.deltaTime;
    }

    //移动到鼠标点击位置
    private void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        agent.isStopped = false;  //重置agent为可移动 
        agent.destination = target;
    }

    //动画切换
    private void SwitchAnimation()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    //攻击事件
    private void EventAttact(GameObject target)
    {
        if (target != null)
        {
            attactTarget = target;
            StartCoroutine(MoveToAttactTarget());
            characterStates.isCritical = UnityEngine.Random.value <= characterStates.attackData.criticalChance;  //暴击判断
        }
    }

    IEnumerator MoveToAttactTarget()
    {
        agent.isStopped = false;  //可以移动
        transform.LookAt(attactTarget.transform);
        //攻击范围判断
        while (Vector3.Distance(attactTarget.transform.position, transform.position) > characterStates.attackData.attackRange)
        {
            agent.destination = attactTarget.transform.position;
            yield return null;
        }
        agent.isStopped = true;  //停止移动
        //攻击
        if (lastAttactTime < 0)
        {
            animator.SetBool("Critical", characterStates.isCritical);
            animator.SetTrigger("Attact");
            //重置攻击冷却时间
            lastAttactTime = characterStates.attackData.coolDown;
        }
    }
}