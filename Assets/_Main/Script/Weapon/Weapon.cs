using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [Header("Weapon Part")]
    public Transform parts;
    public Transform leftHandPoint;
    public Transform rightHandPoint;
    public Transform lookAtPoint;

    [Header("Weapon Parameter")]
    public float damage = 10; //伤害
    public float firingRate = 1; //射速
    public float range = 100; //射程
    public int Ammo = 20; //装弹量
    public float ballisticDiffusion = 0; //弹道扩散
    [SerializeField]
    private AnimationCurve diffusionAnimCurve;
    public float muzzleSpeed = 100; //出膛速度
    public float recoil = 0.05f; //后座力
    [SerializeField]
    private AnimationCurve recoilAnimCurve;
    [SerializeField]
    private ObjectsPool bulletsPool; //子弹对象池

    public int leftAmmo;
    public bool ReadyToShoot
    {
        get
        {
            return (readyShootTimer >= 1.0f / firingRate) && (leftAmmo > 0);
        }
    }
    private float readyShootTimer;
    private float shootTimer;
    public float CurrentBallisticDiffusion
    {
        get
        {
            return diffusionAnimCurve.Evaluate(shootTimer) * ballisticDiffusion;
        }
    }

    void Start () {
        leftAmmo = Ammo;
        readyShootTimer = 0;
    }

	void Update () {
        if (ReadyToShoot)
        {
            if (InputManager.instance.ShootBtn)
            {
                Shoot();
                readyShootTimer = 0;
            }
        }
        readyShootTimer += Time.deltaTime;

        if (leftAmmo > 0)
        {
            if (InputManager.instance.ShootBtn)
            {
                shootTimer += Time.deltaTime;
            }
            else
            {
                shootTimer = 0;
            }
        }
        else
        {
            shootTimer = 0;
        }

        if (InputManager.instance.ReloadBtn)
        {
            Reload();
        }
	}

    void Shoot()
    {
        //获得子弹
        GameObject bulletGO = bulletsPool.Get();
        leftAmmo--;
        bulletGO.transform.position = transform.position;
        bulletGO.transform.rotation = transform.rotation;
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        //检测目标
        Vector3 targetPoint;
        float randomAngle = Random.Range(0, 360);
        Vector3 bias = Camera.main.transform.right * Mathf.Cos(randomAngle) + Camera.main.transform.up * Mathf.Sin(randomAngle);
        bias *= Random.Range(0f, 1f);
        bias *= CurrentBallisticDiffusion;
        Vector3 rayDir = Camera.main.transform.forward + bias;
        Ray ray = new Ray(Camera.main.transform.position + Camera.main.transform.forward * 2, rayDir);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, range))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = Camera.main.transform.position + rayDir.normalized * range;
        }
        Vector3 shootDir = (targetPoint - transform.position).normalized;
        //发射子弹
        bullet.Fire(damage, range, muzzleSpeed, shootDir, bulletsPool);
        //枪支反弹
        StartCoroutine(Recoil());
    }

    IEnumerator Recoil()
    {
        float timer = 0;
        float animTime = 1.0f / firingRate;
        float animCurveTime = recoilAnimCurve.keys[recoilAnimCurve.length - 1].time;
        while (timer < animTime)
        {
            float recoilDist = recoilAnimCurve.Evaluate(timer / animTime * animCurveTime) * recoil;
            parts.localPosition = Vector3.back * recoilDist;
            timer += Time.deltaTime;
            yield return null;
        }
        parts.localPosition = Vector3.zero;
    }

    void Reload()
    {
        leftAmmo = Ammo;
    }

    public void SetActive(bool value)
    {
        parts.gameObject.SetActive(value);
    }
}
