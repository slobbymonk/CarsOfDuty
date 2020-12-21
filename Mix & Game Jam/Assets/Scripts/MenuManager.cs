using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject optionPanel, menuPanel, weaponPanel;

    private GameObject MG, PT, BZ;
    public GameObject car, enemy, go, win;
    public float enemyAmount;
    public float difficulty = 0;
    public bool MGb, PTb, BZb;
    public Camera camera;
    

    //SpawningEnemies
    public Vector3 center, size, size2, center2;

    public void Start()
    {
        MG = car.transform.Find("MG").gameObject;
        PT = car.transform.Find("PT").gameObject;
        BZ = car.transform.Find("BZ").gameObject;
        menuPanel.SetActive(true);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(center, size);
        Gizmos.DrawCube(center2, size2);
    }

    

    public void OnClickStart()
    {
        SceneManager.LoadScene("Map");
    }
    public void OnClickOpenOptions()
    {
        optionPanel.SetActive(true);
        menuPanel.SetActive(false);
    }
    public void OnClickOpenMenu()
    {
        optionPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
    public void OnClickQuitApp()
    {
        Application.Quit();
    }
    public void ChangeCar()
    {
        menuPanel.SetActive(false);
    }
    public void OnClickChangeGunBZ()
    {
        BZ.SetActive(true);
        BZb = true;
    }
    public void OnClickChangeGunPT()
    {
        PT.SetActive(true);
        PTb = true;
    }
    public void OnClickChangeGunMG()
    {
        MG.SetActive(true);
        MGb = true;
    }
    public void OnClickCloseChoice()
    {
        
        
        weaponPanel.SetActive(false);
        win.SetActive(true);
        go.SetActive(true);
        Instantiate(car);
        SpawnEnemy();
    }
    public void OnClickEasy()
    {
        difficulty = 0;
    }
    public void OnClickNormal()
    {
        difficulty = 1;
    }
    public void OnClickHard()
    {
        difficulty = 2;
    }

    public void OnClickRe()
    {
        go.SetActive(false);
        menuPanel.SetActive(true);
        SceneManager.LoadScene("Map");
    }

    public void OnClickTurn()
    {
        camera.transform.Rotate(0, 180, 0);
    }

    public void SpawnEnemy()
    {
        if(difficulty == 0)
        {
            enemyAmount = Random.Range(2, 4);
        }else if(difficulty == 1)
        {
            enemyAmount = Random.Range(4, 6);
        }
        else if(difficulty == 2)
        {
            enemyAmount = Random.Range(6, 9);
        }

        

        Debug.Log("hye");
        for (int i = 0; i < enemyAmount; i++)
        {
            Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
            Vector3 pos2 = center2 + new Vector3(Random.Range(-size2.x / 2, size2.x / 2), Random.Range(-size2.y / 2, size2.y / 2), Random.Range(-size2.z / 2, size2.z / 2));
            Instantiate(enemy, pos, Quaternion.identity);
            Instantiate(enemy, pos2, Quaternion.identity);
        }
        Debug.Log("l");
    }

    public void Update()
    {
        if (MGb)
        {
            PTb = false;
            BZb = false;
            BZ.SetActive(false);
            PT.SetActive(false);
        }else if (PTb)
        {
            MGb = false;
            BZb = false;
            BZ.SetActive(false);
            MG.SetActive(false);
        }else if (BZb)
        {
            PTb = false;
            MGb = false;
            PT.SetActive(false);
            MG.SetActive(false);
        }
    }
}
