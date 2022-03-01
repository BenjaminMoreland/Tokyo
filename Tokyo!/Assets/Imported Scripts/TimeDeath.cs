using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDeath : MonoBehaviour
{
    //time until object is destroyed in seconds
    public float LifeTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathTimer());
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(LifeTime);
        Death Grim = GetComponent<Death>();
        if (Grim != null)
        {
            Grim.OnDeath.Invoke();
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}