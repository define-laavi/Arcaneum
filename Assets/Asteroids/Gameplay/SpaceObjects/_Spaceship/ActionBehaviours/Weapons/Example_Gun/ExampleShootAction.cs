using System.Collections;
using UnityEngine;
using Arcadeum.Common;

namespace Arcadeum.Asteroids.Core
{
    public class ExampleShootAction : SpaceshipActionBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float reloadTime = 2;
        [SerializeField] private float recoil = 0.2f;
        private bool _canShoot = true;

        public override void Act(Spaceship spaceship)
        {
            if (!_canShoot) return;

            var bullet = Pool.Spawn(bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;

            spaceship.ApplyLinearForce(-transform.up * recoil); //Apply recoil

            StartCoroutine(Reload());
        }

        IEnumerator Reload()
        {
            _canShoot = false;
            yield return new WaitForSecondsRealtime(reloadTime);
            _canShoot = true;
        }
    }
}
