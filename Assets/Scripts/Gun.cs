using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : Item
{
    [SerializeField] private float damageAmount = 5f;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float fireRatePerSecond = 1;

    private float nextFire = 0f;

    public override void Use()
    {
        if (Time.time < nextFire)
            return;
        nextFire = Time.time + fireRatePerSecond;

        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, muzzle.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(muzzle.position, muzzle.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            Debug.Log("Did Hit");
            var damageble = hit.collider.GetComponent<IDamageble>();
            if (damageble != null)
            {
                damageble.TakeDamage(damageAmount);
            }
        }
        else
        {
            Debug.DrawRay(muzzle.position, muzzle.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }
}