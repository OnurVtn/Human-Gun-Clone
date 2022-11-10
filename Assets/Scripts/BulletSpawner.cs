using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private float pistolDelay, smgDelay;
    private float timer = 0;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Stone"))
        {
            if (StickmanMerger.Instance.CompareTag("Pistol"))
            {
                CreateBullet(pistolDelay);
            }

            if (StickmanMerger.Instance.CompareTag("SMG"))
            {
                CreateBullet(smgDelay);
            }
        }
    }

    private void CreateBullet(float gunDelay)
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            timer = gunDelay;
        }
    }
}
