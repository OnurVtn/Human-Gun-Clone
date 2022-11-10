using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stone : MonoBehaviour
{
    [SerializeField] private float stoneHealth;

    void Start()
    {
        GetHealth();
    }

    void Update()
    {
        DestroyStone();
    }

    private void GetHealth()
    {
        stoneHealth = int.Parse(GetComponentInChildren<TMP_Text>().text);
    }

    private void DestroyStone()
    {
        if (stoneHealth <= 0)
        {
            Drop();

            Destroy(this.gameObject);
        }
    }

    private void Drop()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Collider>() != null)
            {
                child.GetComponent<Rigidbody>().isKinematic = false;               
                child.SetParent(null);                
            }
        }
    }

    public void DecreaseHealth(float damage)
    {
        stoneHealth -= damage;
        GetComponentInChildren<TMP_Text>().text = stoneHealth.ToString();
    }
}
