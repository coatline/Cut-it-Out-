using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        Invoke("Die", 5);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
