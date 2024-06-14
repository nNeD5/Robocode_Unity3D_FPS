using System;
using UnityEngine;
using UnityEngine.AI;

public class ZombieCharacterControl : MonoBehaviour, IDamageble
{
    [SerializeField] private Transform target;
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private float damageAmount = 2;
    [SerializeField] private float attackRange = 1;
    [SerializeField] private float fireRate = 2;

    private float nextFireTime = 0;
    private float currentHealth = 0;
    private bool isDead = false;

    private Animator animator = null;
    private NavMeshAgent navAgent = null;

    public void TakeDamage(float amount)
    {
        Debug.Log("Zombie Take Damage");
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        navAgent.SetDestination(transform.position);
        animator.SetTrigger("Dead");
        Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length + 2);
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

        navAgent.stoppingDistance = attackRange - navAgent.radius - 0.15f;
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        bool isAttackig = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "zombie_attack";
        if (isAttackig) return;

        float distToTarget = Vector3.Distance(transform.position, target.position);
        if (distToTarget <= attackRange)
        {
            AttackTarget();
        }
        else
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        navAgent.SetDestination(target.position);
        animator.SetFloat("MoveSpeed", navAgent.speed);
    }

    private void AttackTarget()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;
        navAgent.SetDestination(transform.position);
        animator.SetFloat("MoveSpeed", 0);
        animator.SetTrigger("Attack");

        RaycastHit hit;
        Vector3 pos = transform.position;
        pos.y += 1;
        if (Physics.Raycast(pos, transform.TransformDirection(Vector3.forward), out hit, attackRange))
        {
            Debug.Log("Hit");
            hit.collider.GetComponent<IDamageble>()?.TakeDamage(damageAmount);
        }
    }

}