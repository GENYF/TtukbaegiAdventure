using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_move : MonoBehaviour
{
    public float Speed = 3.0f; //플레이어 이동 속도
    public GameObject BulletPrefab; //발사할 레이저를 저장
    public bool canShoot = true; //레이저를 쏠 수 있는 상태인지 검사
    public Vector3 ppap = new Vector3(0, 1, 0);
    const float shootDelay = 0.5f; //레이저를 쏘는 주기
    float shootTimer = 0; //시간을 잴 타이머

    //효과음 
    private AudioSource Bulletaudio;
    public AudioClip BulletSound;
    private AudioSource Nugulaudio;
    public AudioClip NugulSound;

    public float hp = 10;
    public Image hpGage;
    public float ult = 0;
    const float maxUlt = 10;
    public Image ultGage;

    public Transform Nugul;

    public static Player_move instance;

    private void Awake()
    {
        instance = GetComponent<Player_move>();
    }

    void Start()
    {
        this.Bulletaudio = this.gameObject.AddComponent<AudioSource>();
        this.Nugulaudio = this.gameObject.AddComponent<AudioSource>();
        this.Bulletaudio.clip = this.BulletSound;
        this.Nugulaudio.clip = this.NugulSound;
        this.Bulletaudio.loop = false;
        this.Nugulaudio.loop = false;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Egg"))
        {
            hp -= 2;
            hpGage.fillAmount = hp * 0.1f;

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Zunho"))
        {
            hp--;
            hpGage.fillAmount = hp * 0.1f;

            Destroy(other.gameObject);
        }
    }

    void Update()
    {
        Move();
        ShootControl();

        if (Input.GetKeyDown(KeyCode.Q) && ult >= maxUlt)
        {
            this.Nugulaudio.Play();//효과음 재생
            ult = 0;
            ultGage.fillAmount = 0;
            StartCoroutine("Ult");
        }

        if (hp <= 0)
        {
            GameOver();
            EnemyDirector.instance.GameOver();
            gameObject.SetActive(false);
        }
    }

    IEnumerator Ult()
    {
        for (int i = 0; i < 60; i++)
        {
            Nugul.Translate(0, 1f, 0);
            yield return new WaitForSeconds(0.002f);
        }
        Nugul.position = new Vector3(0, -25.5f, 0);
    }

    private void Move() // 움직이는 기능을 하는 메소드
    {
        Vector2 pos = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.Translate(pos * Speed * Time.deltaTime);

        if (transform.position.x < -25)
            transform.position = new Vector3(-25, transform.position.y, 0);
        else if (transform.position.x > 25)
            transform.position = new Vector3(25, transform.position.y, 0);

        if (transform.position.y < -15.5f)
            transform.position = new Vector3(transform.position.x, -15.5f, 0);
        else if (transform.position.y > 7)
            transform.position = new Vector3(transform.position.x, 7, 0);
    }

    void ShootControl() // 발사를 관리하는 메소드
    {
        if (canShoot) // 쏠 수 있는 상태인지 검사
        {
            if (shootTimer > shootDelay && Input.GetKey(KeyCode.Space)) //쿨타임이 지났는지와, 공격키인 스페이스가 눌려있는지 검사
            {
                Instantiate(BulletPrefab, transform.position + ppap, Quaternion.identity); //레이저를 생성해줍니다.
                this.Bulletaudio.Play();//효과음 재생
                shootTimer = 0; //쿨타임을 다시 카운트 
            }
            shootTimer += Time.deltaTime; //쿨타임을 카운트
        }
    }

    public void GameOver()
    {
        ultGage.fillAmount = 0;
        hpGage.fillAmount = 1;
        shootTimer = 0; //시간을 잴 타이머
        hp = 10;
        ult = 0;
    }
}
