using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : Item
{
    [SerializeField] private float damageAmount = 5f;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float fireRatePerSecond = 1;
    [SerializeField] private GameObject shootFx = null;
    [SerializeField] private Transform shootFxPos = null;
    [SerializeField] private AudioSource fireSound = null;

    private Animator animator;

    private float nextFire = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        Debug.Assert(animator, "Animator missed");
        Debug.Assert(shootFx, "ShootFx missed");
        Debug.Assert(shootFxPos, "ShootFx Pos missed");
        Debug.Assert(fireSound, "Fire Sound missed");
    }

    public override void Use()
    {
        if (Time.time < nextFire)
            return;
        nextFire = Time.time + fireRatePerSecond;
        animator.SetTrigger("Shoot");
        fireSound.Play();
        Instantiate(shootFx, shootFxPos.position, shootFxPos.rotation);
        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, muzzle.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            hit.collider.GetComponent<IDamageble>()?.TakeDamage(damageAmount);
        }
    }
}