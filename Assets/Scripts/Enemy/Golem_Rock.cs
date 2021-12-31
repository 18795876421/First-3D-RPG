using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Rock : MonoBehaviour
{
    private new Rigidbody rigidbody;

    [Header("Base Settings")]
    public float froce;
    public GameObject target;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        FlayToTarget();
    }

    private void FlayToTarget()
    {
        Vector3 deriction = (target.transform.position - transform.position + Vector3.up).normalized;
        rigidbody.AddForce(deriction * froce, ForceMode.Impulse);
    }
}
