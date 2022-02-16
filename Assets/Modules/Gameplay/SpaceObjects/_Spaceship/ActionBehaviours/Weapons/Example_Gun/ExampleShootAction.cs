using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleShootAction : SpaceshipActionBehaviour
{
    public float reloadTime = 2;
    public GameObject bulletPrefab;
    public float recoil = 0.2f;
    private bool _canShoot = true;
    
    public override void Act(Spaceship spaceship)
    {
        if (!_canShoot) return;

        var bullet = Pool.Spawn(bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;

        spaceship.ApplyLinearForce(-transform.up * recoil);

        _canShoot = false;
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        yield return new WaitForSecondsRealtime(reloadTime);
        _canShoot = true;
    }
}
