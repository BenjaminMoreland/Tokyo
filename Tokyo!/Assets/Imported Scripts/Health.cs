using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int CurrentHealth = 10;
    public int MaxHealth = 10;
    public bool DestroyAtZero = true;
    public float InvincTime = 0.3f;
    float InvTimer = 0;
    float DeathTime = 0.2f;
    bool Dying = false;
    public int points = 1;

    public void ChangeHealth(int amount)
    {
        //check if the amount is damaged. If so, check if invincible currently
        if (amount >= 0 || InvTimer <= 0)
        {
            CurrentHealth += amount;
            if (amount < 0)
                InvTimer = InvincTime;
        }
        //if not add amount

        //if reduced to 0 and we need to destroy, do that.
        if (CurrentHealth <= 0 && DestroyAtZero)
        {
            if(!Dying)
                StartCoroutine(TimeDeath());
        }
        // if over max health fix that
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;

        IEnumerator TimeDeath()
        {
            Dying = true;
            GameManager.score += points;
            yield return new WaitForSeconds(DeathTime);
            Death Grim = GetComponent<Death>();
            if (Grim != null)
            {
                Grim.OnDeath.Invoke();
            }
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(InvincTime > 0)
        {
            InvTimer -= Time.deltaTime;
        }
    }
}
