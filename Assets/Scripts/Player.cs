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

    public static int hpMax = 100;
    public static int mpMax = 100;
    private static int mpRecover = 1;
    private static float moveSpeed = 5;
    private static float flashSpeed = 30;
    private static float obsticleSpeedMulti = 3;
    private static float obsticleMoveCheck = 0.1f;
    private static float flashCD = 5000;
    private static float meleeDamageTime = 600;
    private static int meleeDamageMaxStack = 20;
    private static float novaSpellDuration = 2500;
    private static float dontLoseMpDuration = 10000;

    private static int meleeMpLose = 10;
    private static int bulletMpLose = 5;
    private static int novaMpLose = 30;
    private static int meleeDamageHp = 10;
    private static int bulletDamageHp = 5;
    private static int novaDamageHp = 30;

    public InputManager input = null;

    public Animator animator = null;

    public SoundManager sound = null;

    public PlayerId id = PlayerId.player1;

    public int hp = 0;
    public int mp = 0;

    public Player inMeleePlayer = null;

    public bool closeInput = false;

    public float recoverDuration = 0.1f;

    public float flashDuration = 0.5f;

    private Rigidbody2D rigidbody = null;
    private ObjectPool<Bullet> bulletPool;
    private ObjectPool<Nova> novaPool;
    private List<Bullet> updateBulletList;

    private bool dontLoseMp = false;

    private float recoverTimer = 0.0f;

    private bool inObsticle = false;

    private bool isFlash = false;
    private bool isNeedFlashNextStepCheck = false;

    private Vector2 beforeFlashSite;

    private float flashTimer = 0.0f;

    private Vector2 flashDir;

    private Stopwatch flashCountDownTimer = new Stopwatch();

    private Stopwatch[] meleeDamageCountDownTimer = new Stopwatch[meleeDamageMaxStack];

    private Stopwatch novaCountDownTimer = new Stopwatch();

    private Stopwatch dontLoseMpTimer = new Stopwatch();

    private bool isNova = false;

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
        novaPool = ObjectPool<Nova>.instance;

        updateBulletList = new List<Bullet>();

        if (id == PlayerId.player1)
        {
            input.p1MoveAction = Move;
            input.p1FlashAction = Flash;
            input.p1MeleeAtk = MeleeAtk;
            input.p1ProjectionAtk = ProjecteAtk;
            input.p1NovaAtk = NovaAtk;
            input.p1Debug = DebugKey;
        }
        else
        {
            input.p2MoveAction = Move;
            input.p2FlashAction = Flash;
            input.p2MeleeAtk = MeleeAtk;
            input.p2ProjectionAtk = ProjecteAtk;
            input.p2NovaAtk = NovaAtk;
            input.p2Debug = DebugKey;
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

        if(dontLoseMpTimer.IsRunning && dontLoseMpTimer.ElapsedMilliseconds > dontLoseMpDuration)
        {
            dontLoseMpTimer.Stop();
            dontLoseMp = false;
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

        if (isNova)
        {
            if(novaCountDownTimer.ElapsedMilliseconds > novaSpellDuration)
            {
                novaCountDownTimer.Stop();
                isNova = false;
            }
        }

        // melee atk
        for (int i = 0; i < meleeDamageMaxStack; i++)
        {
            if (meleeDamageCountDownTimer[i].IsRunning && 
                meleeDamageCountDownTimer[i].ElapsedMilliseconds > meleeDamageTime)
            {
                if (inMeleePlayer != null)
                {
                    sound.HitSound();
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
        if (closeInput || isFlash || isNova)
        {
            return;
        }

        var obsticleSpeedUpMul = 1.0f;

        if(inObsticle &&  Mathf.Abs(_dir.x) > obsticleMoveCheck && Mathf.Abs(_dir.y) > obsticleMoveCheck)
        {
            obsticleSpeedUpMul = obsticleSpeedMulti;
        }

        rigidbody.MovePosition(rigidbody.position + _dir * moveSpeed * Time.deltaTime * obsticleSpeedUpMul);
    }

    private void Flash(Vector2 _dir)
    {
        if (closeInput || isFlash || isNova)
        {
            return;
        }

        if (flashCountDownTimer.IsRunning)
        {
            if(flashCountDownTimer.ElapsedMilliseconds > flashCD)
            {
                flashCountDownTimer.Stop();
            }
            else
            {
                return;
            }
        }

        sound.DashSound();
        beforeFlashSite = transform.localPosition;
        flashDir = _dir;
        flashTimer = 0;
        isFlash = true;
    }

    private void MeleeAtk()
    {
        if (closeInput || isFlash || isNova)
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
                animator.SetTrigger("Melee");
                meleeDamageCountDownTimer[i].Restart();
                break;
            }
        }
    }

    private void ProjecteAtk()
    {
        if (closeInput || isFlash || isNova)
        {
            return;
        }

        if (!dontLoseMp)
        {
            if (mp < bulletMpLose)
            {
                return;
            }
            mp -= bulletMpLose;
        }

        if (isNeedFlashNextStepCheck)
        {
            transform.localPosition = beforeFlashSite;
        }

        animator.SetTrigger("Shoot");
        sound.ShootSound();
        isNeedFlashNextStepCheck = false;
        Bullet b = bulletPool.Spawn(this.transform.position + this.transform.right, Quaternion.identity);
        b.Reset();
        b.sound = sound;
        b.damage = bulletDamageHp;
        b.flyDir = this.transform.right;
    }

    private void NovaAtk()
    {
        if (closeInput || isFlash)
        {
            return;
        }

        if (!dontLoseMp)
        {
            if (mp < novaMpLose)
            {
                return;
            }
            mp -= novaMpLose;
        }

        var spellSite = transform.localPosition;

        if (isNeedFlashNextStepCheck)
        {
            spellSite = beforeFlashSite;
        }
        
        animator.SetTrigger("Nova");
        sound.SkillSound();
        isNeedFlashNextStepCheck = false;
        isNova = true;
        novaCountDownTimer.Restart();
        Nova b = novaPool.Spawn(spellSite, Quaternion.identity);
        if(id == PlayerId.player2)
        {
            b.gameObject.GetComponent<Collider2D>().offset = new Vector2(-4, 0);
            b.transform.GetChild(0).localPosition = new Vector3(-1.15f, 3.89f, 0);
        }
        
        b.Reset();
        b.sound = sound;
        b.damage = novaDamageHp;
    }

    private void DebugKey()
    {
        if (closeInput)
        {
            return;
        }

        dontLoseMp = true;
        if (!dontLoseMpTimer.IsRunning)
        {
            dontLoseMpTimer.Restart();
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision?.gameObject.tag == "Obsticle")
        {
            inObsticle = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision?.gameObject.tag == "Obsticle")
        {
            inObsticle = false;
        }
    }

    public void damage(int _atk)
    {
        hp = hp - _atk < 0 ? 0 : hp - _atk;
    }
}
