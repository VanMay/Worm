using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private float damage;
    private float range;
    private float muzzleSpeed;
    private Vector3 shootDir;
    private ObjectsPool bulletsPool;

    [SerializeField]
    private AnimationCurve damageDecreaseCurve;
    [SerializeField]
    private AnimationCurve speedDecreaseCurve;
    [SerializeField]
    private TrailRenderer trail;

    private float damageDecreaseCurveTime;
    private float speedDecreaseCurveTime;

    void Start()
    {
        damageDecreaseCurveTime = damageDecreaseCurve.keys[damageDecreaseCurve.length - 1].time;
        speedDecreaseCurveTime = speedDecreaseCurve.keys[speedDecreaseCurve.length - 1].time;
    }

    public void Fire(float damage, float range, float muzzleSpeed, Vector3 shootDir, ObjectsPool bulletsPool)
    {
        this.damage = damage;
        this.range = range;
        this.muzzleSpeed = muzzleSpeed;
        this.shootDir = shootDir;
        this.bulletsPool = bulletsPool;

        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float distance = 0;
        Vector3 velocity = shootDir.normalized * muzzleSpeed;
        trail.Clear();
        while (distance < range)
        {
            float currentDamage = damageDecreaseCurve.Evaluate(distance / range * damageDecreaseCurveTime) * damage;
            float currentSpeed = speedDecreaseCurve.Evaluate(distance / range * speedDecreaseCurveTime) * muzzleSpeed;
            velocity = velocity.normalized * currentSpeed;
            transform.rotation = Quaternion.LookRotation(velocity);
            float deltaDistance = (velocity * Time.deltaTime).magnitude;
            if (distance + deltaDistance > range)
            {
                deltaDistance = range - distance;
            }
            Ray ray = new Ray(transform.position, velocity);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, deltaDistance))
            {
                transform.position = hit.point;
                StartCoroutine(Hit(hit.transform));
                break;
            }
            else
            {
                distance += deltaDistance;
                transform.position += velocity * Time.deltaTime;
            }
            yield return null;
        }
        if (distance >= range)
        {
            Debug.Log("Hit: Null");
            bulletsPool.Return(gameObject);
        }
        yield return null;
    }

    IEnumerator Hit(Transform hitTarget)
    {
        yield return null;
    }
}
