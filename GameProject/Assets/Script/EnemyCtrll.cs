using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrll : MonoBehaviour {

    public float Speed = 3.0f;
    public bool isRight = false;
    public GameObject BulletPrefab; //발사할 레이저를 저장

    const float shootDelay = 1.2f; //레이저를 쏘는 주기
    float shootTimer = 0; //시간을 잴 타이머

    public int hp = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            hp--;
            Destroy(other.gameObject);
            if (hp <= 0)
            {
                EnemyDirector.instance.score++;
                EnemyDirector.instance.scoreText.text = "Score : " + EnemyDirector.instance.score.ToString() + "점";

                Player_move.instance.ult ++;
                Player_move.instance.ultGage.fillAmount = Player_move.instance.ult * 0.1f;

                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        if (isRight)
        {
            transform.Translate(Speed * Time.deltaTime, -0.3f * Time.deltaTime, 0);
            if (transform.position.x > 17)
                isRight = false;
        }
        else
        {
            transform.Translate(-Speed * Time.deltaTime, -0.3f * Time.deltaTime, 0);
            if (transform.position.x < -17)
                isRight = true;
        }

        ShootControl();
    }

    void ShootControl() // 발사를 관리하는 메소드
    {
        if (shootTimer > shootDelay) //쿨타임이 지났는지와, 공격키인 스페이스가 눌려있는지 검사
        {
            GameObject newBullet = Instantiate(BulletPrefab, transform.position + Vector3.down, Quaternion.identity); //레이저를 생성해줍니다.
            newBullet.transform.SetParent(EnemyDirector.instance.transform);
            shootTimer = 0; //쿨타임을 다시 카운트 
        }
        shootTimer += Time.deltaTime; //쿨타임을 카운트
    }
}
