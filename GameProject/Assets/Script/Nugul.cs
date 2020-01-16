using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nugul : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyDirector.instance.score++;
                EnemyDirector.instance.scoreText.text = "Score : " + EnemyDirector.instance.score.ToString() + "점";
            }
            Destroy(other.gameObject);
        }
    }
}
