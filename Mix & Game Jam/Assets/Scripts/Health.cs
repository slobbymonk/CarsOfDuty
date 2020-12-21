using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float  fireRate, magazineSize;
    [SerializeField] public float health, damage;
    private float canShoot, maxMag, reloadTime;
    public GameObject mm;
    public static bool shootYes = true;
    private bool reloading = false;



    public void TakeDamage()
    {
        
        if(canShoot == fireRate & magazineSize > 0 & !reloading)
        {
            shootYes = true;
        }
        else
        {
            shootYes = false;
        }

        
    }
    public void FixedUpdate()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
        if (mm == null)
        {
            mm = GameObject.Find("/GunCanvas");
            var m = mm.GetComponent<MenuManager>();

            if (m.MGb)
            {
                damage = 0.5f;
                maxMag = 50;
                reloadTime = 8;
                fireRate = 1;
            }
            else if (m.PTb)
            {
                damage = 2;
                maxMag = 5;
                reloadTime = 2;
                fireRate = 2;
            }

            canShoot = fireRate;
            magazineSize = maxMag;
        }

        if(canShoot < fireRate)
        {
            canShoot += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            reloading = true;
            Invoke("Delay", reloadTime);
        }
    }
    public void Delay()
    {
        magazineSize = maxMag;
        reloading = false;
    }
}

// make damage dependent on player
// when it hits it takes 
