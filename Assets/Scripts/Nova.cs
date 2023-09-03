using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nova : MonoBehaviour
{
    private static float delayDamage = 2.0f;

    private float timer = 0.0f;
    private List<Player> damagePlayer = new List<Player>();

    public SoundManager sound = null;

    public int damage;

    public void Reset()
    {
        damagePlayer.Clear();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        timer += deltaTime;
        if (timer > delayDamage)
        {
            for (int i = 0; i < damagePlayer.Count; i++)
            {
                sound.HitSound();
                damagePlayer[i].damage(damage);
            }
            ObjectPool<Nova>.instance.Recycle(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision?.gameObject.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<Player>();
            for (int i = 0;i < damagePlayer.Count; i++)
            {
                if(damagePlayer[i] == player)
                {
                    return;
                }
            }
            damagePlayer.Add(player);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision?.gameObject.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<Player>();
            for (int i = 0; i < damagePlayer.Count; i++)
            {
                if (damagePlayer[i] == player)
                {
                    damagePlayer.Remove(player);
                }
            }
        }
    }
}
