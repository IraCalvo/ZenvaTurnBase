using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float speed;

    private Character target;

    private UnityAction hitCallBack;

    public void Initialize(Character projectileTarget, UnityAction onHitCallBack)
    {
        target = projectileTarget;
        hitCallBack = onHitCallBack;
    }

    void Update()
    {
        if(target == null)
            return;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        if(transform.position == target.transform.position)
        {
            target.TakeDamage(damage);
            hitCallBack?.Invoke();
            Destroy(gameObject);
        }
    }
}
