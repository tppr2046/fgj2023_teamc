using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private static float flyDuration = 2.0f;
    private static float moveSpeed = 10;

    private float timer = 0.0f;

    public SoundManager sound = null;

    public int damage;

    public Vector2 flyDir;

    private Rigidbody2D rigidbody = null;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Reset()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        timer += deltaTime;
        if(timer > flyDuration)
        {
            ObjectPool<Bullet>.instance.Recycle(this);
        }

        rigidbody.MovePosition(rigidbody.position + flyDir * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sound.HitSound();
            collision.gameObject.GetComponent<Player>().damage(damage);
        }
        ObjectPool<Bullet>.instance.Recycle(this);
    }
}
