using System;
using UnityEngine;
using UnityEngine.AI;

public class ZombieCharacterControl : MonoBehaviour, IDamageble
{
    [SerializeField] private Transform target;
    [SerializeField] private float maxHealth;

    private float currentHealth;

    private Animator animator = null;
    private NavMeshAgent navAgent = null;

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Dead");
        Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }

    private void Awake()
    {
        currentHealth = maxHealth;
        if (!target) target = GameObject.Find("Player").transform;
        animator = gameObject.GetComponent<Animator>();
        navAgent = gameObject.GetComponent<NavMeshAgent>();

        Debug.Assert(target, "Target missed");
        Debug.Assert(animator, "Aimator missed");
        Debug.Assert(navAgent, "Nav Mesh Agent missed");
    }

    private void FixedUpdate()
    {
        navAgent.SetDestination(target.position);
        animator.SetFloat("MoveSpeed", navAgent.speed);
        Debug.Log(target.position);
        Debug.Log(target);
    }
}