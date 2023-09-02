using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerId
    {
        player1 = 0,
        player2
    }

    private static int hpMax = 100;
    private static int mpMax = 100;
    private static int mpRecover = 1;
    private static float moveSpeed = 5;
    private static float flashSpeed = 30;
    private static float flashCD = 5000;
    private static int meleeMpLose = 5;
    private static float meleeDamageTime = 600;
    private static int meleeDamageMaxStack = 20;

    private static int meleeDamageHp = 10;
    private static int bulletDamageHp = 5;

    public InputManager input = null;

    public PlayerId id = PlayerId.player1;

    public int hp = 0;
    public int mp = 0;

    public Player inMeleePlayer = null;

    public bool closeInput = false;

    public float recoverDuration = 0.1f;

    public float flashDuration = 0.5f;

    private Rigidbody2D rigidbody = null;
    private ObjectPool<Bullet> bulletPool;
    private List<Bullet> updateBulletList;

    private bool dontLoseMp = false;

    private float recoverTimer = 0.0f;

    private bool isFlash = false;
    private bool isNeedFlashNextStepCheck = false;

    private Vector2 beforeFlashSite;

    private float flashTimer = 0.0f;

    private Vector2 flashDir;

    private Stopwatch flashCountDownTimer = new Stopwatch();

    private Stopwatch[] meleeDamageCountDownTimer = new Stopwatch[meleeDamageMaxStack];



    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        hp = hpMax;
        mp = mpMax;

        for(int i = 0;i < meleeDamageMaxStack; i++)
        {
            meleeDamageCountDownTimer[i] = new Stopwatch();
        }

        bulletPool = ObjectPool<Bullet>.instance;

        updateBulletList = new List<Bullet>();

        if (id == PlayerId.player1)
        {
            input.p1MoveAction = Move;
            input.p1FlashAction = Flash;
            input.p1MeleeAtk = MeleeAtk;
            input.p1ProjectionAtk = ProjecteAtk;
        }
        else
        {
            input.p2MoveAction = Move;
            input.p2FlashAction = Flash;
            input.p2MeleeAtk = MeleeAtk;
            input.p2ProjectionAtk = ProjecteAtk;
        }

        closeInput = true;
        flashTimer = 0;
        isNeedFlashNextStepCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (closeInput)
        {
            return;
        }

        // mp recover
        if(recoverTimer > recoverDuration)
        {
            mp = mp + mpRecover > mpMax ? mpMax : mp + mpRecover;
            recoverTimer = 0;
        }
        else
        {
            recoverTimer += Time.deltaTime;
        }

        // melee atk
        for (int i = 0; i < meleeDamageMaxStack; i++)
        {
            if (meleeDamageCountDownTimer[i].IsRunning && 
                meleeDamageCountDownTimer[i].ElapsedMilliseconds > meleeDamageTime)
            {
                if (inMeleePlayer != null)
                {
                    inMeleePlayer.damage(meleeDamageHp);
                }
                meleeDamageCountDownTimer[i].Stop();
            }
        }

        if (isFlash)
        {
            flashTimer += Time.deltaTime;
            rigidbody.MovePosition(rigidbody.position + flashDir * flashSpeed * Time.deltaTime);
            if(flashTimer >= flashDuration)
            {
                isFlash = false;
                isNeedFlashNextStepCheck = true;
                flashCountDownTimer.Restart();
            }
        }
    }

    private void Move(Vector2 _dir)
    {
        if (closeInput || isFlash)
        {
            return;
        }

        rigidbody.MovePosition(rigidbody.position + _dir * moveSpeed * Time.deltaTime);
    }

    private void Flash(Vector2 _dir)
    {
        if (closeInput || isFlash)
        {
            return;
        }

        beforeFlashSite = transform.localPosition;
        flashDir = _dir;
        flashTimer = 0;
        isFlash = true;
    }

    private void MeleeAtk()
    {
        if (closeInput || isFlash)
        {
            return;
        }

        if (!dontLoseMp)
        {
            if(mp < meleeMpLose)
            {
                return;
            }
            mp -= meleeMpLose;
        }

        isNeedFlashNextStepCheck = false;
        for (int i = 0; i < meleeDamageMaxStack; i++)
        {
            if (!meleeDamageCountDownTimer[i].IsRunning)
            {
                meleeDamageCountDownTimer[i].Restart();
                break;
            }
        }
    }

    private void ProjecteAtk()
    {
        if (closeInput || isFlash)
        {
            return;
        }

        if (!dontLoseMp)
        {
            if (mp < meleeMpLose)
            {
                return;
            }
            mp -= meleeMpLose;
        }

        if (isNeedFlashNextStepCheck)
        {
            transform.localPosition = beforeFlashSite;
        }

        isNeedFlashNextStepCheck = false;
        Bullet b = bulletPool.Spawn(this.transform.position + this.transform.right, Quaternion.identity);
        b.Reset();
        b.flyDir = this.transform.right;
    }
    
    public void damage(int _atk)
    {
        hp = hp - _atk < 0 ? 0 : hp - _atk;
    }
}
