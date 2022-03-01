using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int Amount = 4;
    public bool DestroyOnCollide = true; 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //make sure there is a health component
        Health h = collision.gameObject.GetComponent<Health>();
        if (h != null)
        {
            h.ChangeHealth(-Amount);
        }
        if (DestroyOnCollide)
        {
            Death Grim = GetComponent<Death>();
            if (Grim != null)
            {
                Grim.OnDeath.Invoke();
            }
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        //make sure there is a health component
        Health h = collision.gameObject.GetComponent<Health>();
        if (h != null)
        {
            h.ChangeHealth(-Amount);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //make sure there is a health component
        Health h = collision.GetComponent<Health>();
        if (h != null)
        {
            h.ChangeHealth(-Amount);
        }
        if (DestroyOnCollide)
        {
            Death Grim = GetComponent<Death>();
            if (Grim != null)
            {
                Grim.OnDeath.Invoke();
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider2D collision)
    {
        //make sure there is a health component
        Health h = collision.GetComponent<Health>();
        if (h != null)
        {
            h.ChangeHealth(-Amount);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
