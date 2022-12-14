using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootObject : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject target;
    private bool isShooting = false;
    [SerializeField] float shotsPerSecond = 1;
    [SerializeField] float projectileSpeed = 1;
    [SerializeField] float projectileLifetime = 1;
    [SerializeField] float initailWindupTime = 0.5f;
    [SerializeField] bool destroyDumbProjectiles = true;
    [SerializeField] float destroyDelaySeconds = .5f;
    EndLevelTrigger endLevelTrigger;

    private void Awake()
    {
        endLevelTrigger = FindObjectOfType<EndLevelTrigger>();
    }

    void Start()
    {
        new WaitForSeconds(initailWindupTime);
        StartShooting();
    }

    void Update()
    {
        if (endLevelTrigger != null)
        {
            if (endLevelTrigger.IsEndLevelReached())
            {
                StopShooting();
            }
        }
    }






    IEnumerator SpawnProjectile()
    {
        if (initailWindupTime > 0)
            yield return new WaitForSeconds(initailWindupTime);

        while (isShooting)
        {
            GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
            FollowObject projF = proj.GetComponent<FollowObject>();
            DumbProjectile projDummy = proj.GetComponent<DumbProjectile>();
            if (projF != null)
            {
                projF.objectToFollow = target;
                projF.follow3D = true;
                projF.moveSpeed = projectileSpeed;
                projF.rotateTowardsTarget = true;
                projF.stopsOnCollision = true;
                projF.objectStopsOnCollision[0] = target;
            }
            else if (projDummy != null)
            {
                projDummy.target = target;
                projDummy.moveSpeed = projectileSpeed;
                projDummy.destroyOnCollision = destroyDumbProjectiles;
                projDummy.destroyDelay = destroyDelaySeconds;
            }
            DestroyAfterSeconds projD = proj.GetComponent<DestroyAfterSeconds>();
            if (projD != null)
            {
                projD.secondsUntilDestroyed = projectileLifetime;
            }

            yield return new WaitForSeconds(1 / shotsPerSecond);
        }
    }

    public void StopShooting()
    {
        isShooting = false;
    }

    public void StartShooting()
    {
        if (!isShooting)
        {
            isShooting = true;
            StartCoroutine(SpawnProjectile());
        }
    }
}
