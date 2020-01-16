using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggCtrl : MonoBehaviour
{
    public Transform ShellL;
    public Transform ShellR;

    public Projectile yellow;

    void Start()
    {
        StartCoroutine("BrokenEgg");
    }

    IEnumerator BrokenEgg()
    {
        for (int i = 0; i < 15; i++)
        {
            ShellL.Rotate(0, 0, -2);
            ShellR.Rotate(0, 0, 2);
            yield return new WaitForSeconds(0.003f);
        }
        yield return new WaitForSeconds(0.2f);
        yellow.enabled = true;
        yellow.transform.parent = null;

        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}