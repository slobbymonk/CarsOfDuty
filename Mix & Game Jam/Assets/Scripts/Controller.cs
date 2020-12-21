using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    private Rigidbody rb;
    public float turnSpeed, speed, jumpF;

    //Shooting
    private Vector3 guntip;
    public Transform guntipT;
    public float dist;
    public float counterMovement = 0.175f;
    public LayerMask layer;
    public ParticleSystem muzzle;
    public bool floating = false;
    public Transform[] wheels;

    //Bazooka
    public float rocketSpeed = 10f;
    public GameObject rocket;
    public GameObject mm;

    public AudioSource shot;

    public GameObject go;
    public GameObject win;

    public ParticleSystem part;

    //Enemy Counter
    public GameObject[] enemies;
    private float ene;
    public string enAm;
    public Text enemieCount;
    private GameObject e;

    //Magazine
    private string bullets;
    private Text magazineSizeText;
    public GameObject getMagText;

    //Won
    [SerializeField] public bool won = false;

    private void Start()
    {
        win = GameObject.Find("GunCanvas/Win");
        rb = GetComponent<Rigidbody>();
        gameObject.GetComponent<Collider>().enabled = false;
        go = GameObject.Find("GunCanvas/GO");
        Invoke("Delay", 2);
        Invoke("l", 0.2f);
        mm = GameObject.Find("/GunCanvas");

        e = GameObject.Find("EText");
        enemieCount = e.GetComponent<Text>();

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            ene += 1;
        }

        var playerDamage = gameObject.GetComponent<Health>();

        getMagText = GameObject.Find("magText");
        magazineSizeText = getMagText.GetComponent<Text>();

        bullets = playerDamage.magazineSize.ToString();
        magazineSizeText.text = bullets;
    }

    public void Victory()
    {
        win.SetActive(true);
    }

    private void Update()
    {
        if (won)
        {
            Victory();
        }
        if(ene <= 0)
        {
            won = true;
        }

        //Enemy Counter
        e = GameObject.Find("EText");
        enemieCount = e.GetComponent<Text>();

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        ene = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            ene += 1;
        }

        //.

        enAm = ene.ToString();
        enemieCount.text = enAm;

        //BulletCounter
        var playerDamage = gameObject.GetComponent<Health>();

        getMagText = GameObject.Find("magText");
        magazineSizeText = getMagText.GetComponent<Text>();

        bullets = playerDamage.magazineSize.ToString();
        magazineSizeText.text = bullets;


        guntip = guntipT.transform.position;
        Debug.DrawRay(guntip, guntipT.forward, Color.yellow);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!floating)
            {
                Debug.Log("hey");
                rb.constraints = RigidbodyConstraints.FreezeRotationZ;
                rb.constraints = RigidbodyConstraints.FreezeRotationX;

                for (int i = 0; i < 2; i++)
                {
                    wheels[i].transform.Rotate(0, 0, 41.91f, Space.Self);
                    wheels[i].GetComponent<WheelCollider>().enabled = true;
                }
                for (int i = 2; i < 4; i++)
                {
                    wheels[i].transform.Rotate(0, 0, -41.91f, Space.Self);
                    wheels[i].GetComponent<WheelCollider>().enabled = true;
                }
                floating = true;
            }
            else if (floating)
            {
                rb.constraints = RigidbodyConstraints.None;

                for (int i = 0; i < 2; i++)
                {
                    wheels[i].transform.Rotate(0, 0, -41.91f, Space.Self);
                    wheels[i].GetComponent<WheelCollider>().enabled = false;
                }
                for (int i = 2; i < 4; i++)
                {
                wheels[i].transform.Rotate(0, 0, 41.91f, Space.Self);
                    wheels[i].GetComponent<WheelCollider>().enabled = false;
                }
                floating = false;
            }

            
        }


        if (Input.GetMouseButton(0))
        {
            playerDamage.magazineSize -= 1;
            if(playerDamage.magazineSize <= 0)
            {
                return;
            }
            Shooting();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Grenadier();
        }
        

        Move();
    }

    public void Grenadier()
    {
        if (Health.shootYes)
        {
            var rocke = Instantiate(rocket, guntip, Quaternion.identity);
            var rock = rocke.GetComponent<Rigidbody>();
            rock.AddForce(transform.forward * rocketSpeed, ForceMode.Impulse);
            rock.AddForce(transform.up * rocketSpeed, ForceMode.Impulse);
        }
    }
    public void OnDestroy()
    {
        go.SetActive(true);
    }
    void Shooting()
    {
        shot.Play();
        muzzle.Play();
        RaycastHit hit;
        Ray ray = new Ray(guntip, guntipT.forward);


        if (Physics.Raycast(ray, out hit, dist))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                if (Health.shootYes)
                {
                    var enemHealth = hit.transform.GetComponent<Health>();
                    var playerDamage = gameObject.GetComponent<Health>();
                    enemHealth.health -= playerDamage.damage;
                    if (enemHealth.health <= 0)
                    {
                        Destroy(hit.transform.gameObject);

                    }
                    Debug.Log("Target Hit!");
                }
                if (Health.shootYes)
                {
                    Instantiate(rocket, guntipT);
                    var rock = rocket.GetComponent<Rigidbody>();
                    rock.AddForce(transform.forward * rocketSpeed, ForceMode.Impulse);
                }


            }

        }


    }

    void Move()
    {

        if (Input.GetKey(KeyCode.LeftControl))
        {
            rb.velocity = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * jumpF, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += transform.forward * Time.deltaTime * -speed / 2;
        }
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Rotate(0, turnSpeed, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            gameObject.transform.Rotate(0, -turnSpeed, 0);
        }

        
    }
    public void Delay()
    {
        gameObject.GetComponent<Collider>().enabled = true;
    }
    public void l()
    {
        go.SetActive(false);
        win.SetActive(false);
    }
    
    
}
