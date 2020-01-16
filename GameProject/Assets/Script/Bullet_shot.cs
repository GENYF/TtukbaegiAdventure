using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_shot : MonoBehaviour
{
    public float moveSpeed = 5.0f;//총알이 움직일 속도

    void Update()
    {
        float moveY = moveSpeed * Time.deltaTime;//이동할 거리를 지정

        transform.position += new Vector3(0, moveY, 0); //위로 이동
        transform.Rotate(0, 0, 5); //옆으로 돔
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
