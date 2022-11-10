using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody bulletRigidbody;
    [SerializeField] private float bulletSpeed;

    [SerializeField] private float pistolDamage, smgDamage;

    void Update()
    {
        MoveBullet();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            if (StickmanMerger.Instance.CompareTag("Pistol"))
            {
                collision.gameObject.GetComponent<Stone>().DecreaseHealth(pistolDamage);
            }

            if (StickmanMerger.Instance.CompareTag("SMG"))
            {
                collision.gameObject.GetComponent<Stone>().DecreaseHealth(smgDamage);
            }

            Destroy(this.gameObject);
        }
    }

    private void MoveBullet()
    {
        bulletRigidbody.AddForce(0f, 0f, bulletSpeed);
    }
}
