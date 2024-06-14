using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : Item
{
    [SerializeField] private float damageAmount = 5f;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float fireRatePerSecond = 1;

    private Animator animator;

    private float nextFire = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        Debug.Assert(animator, "Animator missed");
    }

    public override void Use()
    {
        if (Time.time < nextFire)
            return;
        nextFire = Time.time + fireRatePerSecond;
        animator.SetTrigger("Shoot");
        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, muzzle.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(muzzle.position, muzzle.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            var damageble = hit.collider.GetComponent<IDamageble>();
            if (damageble != null)
            {
                damageble.TakeDamage(damageAmount);
            }
        }
        else
        {
            Debug.DrawRay(muzzle.position, muzzle.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
    }
}