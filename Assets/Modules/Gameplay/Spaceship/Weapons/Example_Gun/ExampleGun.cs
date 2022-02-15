using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleGun : SpaceshipGunBehaviour
{
    public float reloadTime = 2;
    public GameObject bulletPrefab;
    
    private bool _canShoot = true;
    
    public override void Shoot(SpaceshipController spaceship)
    {
        if (!_canShoot) return;

        var bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        
        _canShoot = false;
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        yield return new WaitForSecondsRealtime(reloadTime);
        _canShoot = true;
    }
}
