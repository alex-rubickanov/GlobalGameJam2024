using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIDamageManager : MonoBehaviour
{
    [SerializeField] float health = 10f;
    [SerializeField] float reviveTime = 5f;
    [SerializeField] float jumpForce = 10f;
    public bool isStunned;
    float startingHealth;

    public AIAnimation anim => GetComponent<AIAnimation>();
    public Rigidbody rb => GetComponent<Rigidbody>();
    public Hide hide => GetComponent<Hide>();

    [Header("Events")]
    public UnityEvent OnStunned;
    public UnityEvent OnRevive;

    private void Awake()
    {
        startingHealth = health;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ReceiveDamage(3);
        }
    }

    public void ReceiveDamage(float damageValue)
    {
        if (!hide.isHiding)
        {
            if (health - damageValue <= 0)
            {
                Stun();

            }
            else
            {
                health -= damageValue;
                anim.SetAnim(anim.AI_HIT);
            }
        }
    }

    public void Stun()
    {
        OnStunned.Invoke();
        isStunned = true;
        anim.SetAnim(anim.AI_STUN);
        StartCoroutine(ReviveDelay());
    }
    IEnumerator ReviveDelay()
    {
        yield return new WaitForSeconds(reviveTime);
        OnRevive.Invoke();
        anim.SetAnim(anim.AI_REVIVE);
        health = startingHealth;
        isStunned = false;
    }
}
