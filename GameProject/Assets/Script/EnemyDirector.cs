using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyDirector : MonoBehaviour
{

    public EggCtrl eggPrefab;
    public EnemyCtrll zunhoPrefab;

    public float waveTerm = 5.0f;
    public int wave = 0;
    public Text waveText;

    public int score = 0;
    public Text scoreText;

    int minEnemy = 1;
    int maxEnemy = 3;

    public bool canEgg = false;
    bool canStart = false;

    public GameObject GameOverScreen; //게임 오버하면 뜨는창
    public GameObject ScoreText; //점수창
    public GameObject InputName; //이름 입력하는 창
    public InputField InputField_; //이름 입력하는 창
    public Text[] scoreTexts = new Text[5];
    public Text inputscore;
    public Text inputFieldText;

    public int[] scores = new int[5]; //이때까지 입력된 점수들
    public string[] names = new string[5]; //이때까지 입력된 이름들

    public bool isGameOver = false;

    public static EnemyDirector instance;

    public void GameOver()
    {
        GameOverScreen.SetActive(true);
        InputName.SetActive(true);
        isGameOver = true;
        inputscore.text = "점수 : " + score.ToString() + "점";
        Time.timeScale = 0;
    }

    void Start()
    {
        instance = GetComponent<EnemyDirector>();
        StartCoroutine("NumCount");
    }

    private void Update()
    {
        if (canEgg)
        {
            if (Random.Range(0, 450) < 2)
            {
                Instantiate(eggPrefab, new Vector2(Player_move.instance.transform.position.x, 12.5f), Quaternion.identity);
            }
        }
        if (canStart)
        {
            if (transform.childCount == 0)
            {
                StartCoroutine("WaveCtrl");
                canStart = false;
            }
        }
    }

    IEnumerator NumCount()
    {
        waveText.text = "R U Ready?";
        yield return new WaitForSeconds(2);
        waveText.text = "3";
        yield return new WaitForSeconds(1);
        waveText.text = "2";
        yield return new WaitForSeconds(1);
        waveText.text = "1";
        yield return new WaitForSeconds(1);
        waveText.text = "Go!!";
        yield return new WaitForSeconds(1);

        StartCoroutine("WaveCtrl");
    }

    IEnumerator WaveCtrl()
    {
        if (isGameOver)
            yield return null;

        waveText.gameObject.SetActive(true);
        wave++;
        if (wave % 5 == 0)
        {
            canEgg = !canEgg;
            score += 5;
            scoreText.text = "Score : " + score.ToString() + "점";
        }
        if (wave % 3 == 0)
        {
            maxEnemy++;
            if (maxEnemy - minEnemy > 2)
                minEnemy = maxEnemy - 1;
        }

        waveText.color = Color.magenta;
        waveText.text = "Wave" + wave.ToString();
        yield return new WaitForSeconds(0.7f);
        waveText.gameObject.SetActive(false);

        int ECount = Random.Range(minEnemy, maxEnemy);

        for (int i = 0; i < ECount; i++)
        {
            EnemyCtrll newZunho = Instantiate(zunhoPrefab, new Vector2(Random.Range(-17, 17), 15), Quaternion.identity);
            newZunho.transform.SetParent(transform);

            yield return new WaitForSeconds(Random.Range(1.2f, 2.2f));
        }
        yield return new WaitForSeconds(waveTerm);
        canStart = true;
    }

    public void InputRank()
    {
        int i;
        string nameStr = inputFieldText.text;

        for (i = 0; i < 3; i++)
        {
            if (scores[i] < score)
                break;
        }
        if (i < 3)
        {
            for (int j = 2; j > i; j--)
            {
                scores[j] = scores[j - 1];
                names[j] = names[j - 1];
            }
            scores[i] = score;
            names[i] = nameStr;
        }

        for (i = 0; i < 3; i++)
        {
            scoreTexts[i].text = (i + 1).ToString() + ". " + names[i] + ": " + scores[i].ToString() + "점";
        }

        ScoreText.SetActive(true);
        InputName.SetActive(false);
    }

    public void Restart()
    {
        waveText.text = "";
        waveText.gameObject.SetActive(true);
        waveTerm = 5.0f;
        wave = 0;
        score = 0;
        minEnemy = 1;
        maxEnemy = 3;

        canEgg = false;
        canStart = false;
        isGameOver = false;

        for (int c = 0; c < transform.childCount; c++)
        {
            Destroy(transform.GetChild(c).gameObject);
        }
        GameOverScreen.SetActive(false);
        ScoreText.SetActive(false);
        InputName.SetActive(true);
        Player_move.instance.gameObject.SetActive(true);
        Player_move.instance.transform.position = new Vector3(0, -10, 0);
        scoreText.text = "Score : " + score.ToString() + "점";

        StopCoroutine("WaveCtrl");
        StartCoroutine("NumCount");

        InputField_.Select();
        InputField_.text = "";

        Time.timeScale = 1;
    }
}