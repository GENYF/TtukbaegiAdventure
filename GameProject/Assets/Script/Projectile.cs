using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed = 15.0f;//총알이 움직일 속도

    void Update()
    {
        float moveY = moveSpeed * Time.deltaTime;

        transform.position -= new Vector3(0, moveY, 0);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
