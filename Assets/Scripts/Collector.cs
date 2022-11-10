using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dollar"))
        {
            GameManager.Instance.IncreaseDollar();

            Destroy(other.gameObject);
        }
    }
}
