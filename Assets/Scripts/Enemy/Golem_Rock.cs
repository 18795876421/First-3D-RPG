using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Rock : MonoBehaviour
{
    public enum RockStates { HitPlayer, HitEnemy, HitNothing }
    public RockStates rockStates;
    private new Rigidbody rigidbody;

    [Header("Base Settings")]
    public float froce;
    public int damage;
    public GameObject target;
    public GameObject breakEffect;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.one;
        FlayToTarget();
        rockStates = RockStates.HitPlayer;
    }

    private void FixedUpdate()
    {
        if (rigidbody.velocity.sqrMagnitude < 1f)
        {
            rockStates = RockStates.HitNothing;
        }
    }

    private void FlayToTarget()
    {
        if (target == null)
        {
            target = FindObjectOfType<PlayerController>().gameObject;
        }
        Vector3 deriction = (target.transform.position - transform.position + Vector3.up).normalized;
        rigidbody.AddForce(deriction * froce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (rockStates)
        {
            case RockStates.HitPlayer:
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    other.gameObject.GetComponent<CharacterStates>().TakeDamage(damage, other.gameObject.GetComponent<CharacterStates>());
                    rockStates = RockStates.HitNothing;
                }
                break;
            case RockStates.HitEnemy:
                if (other.gameObject.GetComponent<Golem>())
                {
                    var otherStates = other.gameObject.GetComponent<CharacterStates>();
                    otherStates.TakeDamage(damage, otherStates);
                    Instantiate(breakEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
        }
    }
}