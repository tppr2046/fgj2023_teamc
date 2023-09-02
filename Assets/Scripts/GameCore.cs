using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameCore : MonoBehaviour
{
    public enum GameState : int 
    {
        Idle = 0,
        BeforePlay,
        Game,
        Finish,
        Max
    }

    public GameState gameState = GameState.Idle;

    public float[] gameStateTime = new float[(int)GameState.Max];

    public InputManager input;

    public Player[] players;

    private ObjectPool<Bullet> bulletPool;

    private float timer = 0.0f;

    [SerializeField]
    private GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        bulletPool = ObjectPool<Bullet>.instance;
        bulletPool.InitPool(bulletPrefab, 50);

        timer = 0.0f;
        gameState = GameState.BeforePlay;
        input.inputControl = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState < GameState.Finish)
        {
            timer += Time.deltaTime;
            if (timer >= gameStateTime[(int)gameState])
            {
                switch (gameState)
                {
                    case GameState.BeforePlay:
                        {
                            timer = 0;
                            gameState = GameState.Game;
                            input.inputControl = true;
                            for(int i = 0;i < players.Length; i++)
                            {
                                players[i].closeInput = false;
                            }
                        }
                        break;
                    case GameState.Game:
                        {
                            timer = 0;
                            gameState = GameState.Finish;
                            input.inputControl = false;
                            for (int i = 0; i < players.Length; i++)
                            {
                                players[i].closeInput = true;
                            }
                        }
                        break;
                    case GameState.Finish:
                        {

                        }
                        break;
                }
            }
            else
            {
                if(gameState == GameState.Game)
                {
                    for (int i = 0; i < players.Length; i++)
                    {
                        if(players[i].hp <= 0)
                        {
                            gameState = GameState.Finish;
                            input.inputControl = false;
                            for (int j = 0; j < players.Length; j++)
                            {
                                players[j].closeInput = true;
                            }
                        }
                    }
                }
            }
        }
    }
}