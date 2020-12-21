using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public SphereCollider col;
    public float explosionRadius;
    private float or;
    public bool hit;
    public float explosionTime, bombDamage;
    public ParticleSystem particle;

    public void Start()
    {
        col = gameObject.GetComponent<SphereCollider>();
        or = col.radius;
    }

    private void OnCollisionEnter(Collision collision)
    {
        col.radius = explosionRadius;
        particle.Play(true);
        Invoke("Delay1", explosionTime - 0.01f);
        Invoke("Delay", explosionTime);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.CompareTag("Enemy"))
        {
            coll.gameObject.GetComponent<Health>().health -= bombDamage;
        }
    }
    public void Delay()
    {
        Destroy(gameObject);
    }
    public void Delay1()
    {
        particle.Play(true);
    }
}
