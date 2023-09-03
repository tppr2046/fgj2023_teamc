using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static float doubleClickDuration = 500;
    private static float debugKeyInputDuration = 10000;
    private static KeyCode[] debugKeyP1Combo = new KeyCode[] {
        KeyCode.W, KeyCode.S,
        KeyCode.W, KeyCode.S,
        KeyCode.A, KeyCode.D,
        KeyCode.A, KeyCode.D,
        KeyCode.T, KeyCode.Y,
        KeyCode.T, KeyCode.Y };
    private static KeyCode[] debugKeyP2Combo = new KeyCode[] {
        KeyCode.UpArrow, KeyCode.DownArrow,
        KeyCode.UpArrow, KeyCode.DownArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.Keypad1, KeyCode.Keypad2,
        KeyCode.Keypad1, KeyCode.Keypad2 };

    public bool inputControl = false;
    public Action<Vector2> p1MoveAction, p2MoveAction = null;
    public Action p1MoveStopAction, p2MoveStopAction = null;
    public Action<Vector2> p1FlashAction, p2FlashAction = null;
    public Action p1MeleeAtk, p2MeleeAtk = null;
    public Action p1ProjectionAtk, p2ProjectionAtk = null;
    public Action p1NovaAtk, p2NovaAtk = null;
    public Action p1Debug, p2Debug = null;

    private Vector2 p1Move = new Vector2();
    private Vector2 p2Move = new Vector2();

    private Vector2 p1Flash = new Vector2();
    private Vector2 p2Flash = new Vector2();
    private Stopwatch p1FlashDoubleClickTimer = new Stopwatch();
    private Stopwatch p2FlashDoubleClickTimer = new Stopwatch();

    private Stopwatch p1DebugKeyTimer = new Stopwatch();
    private Stopwatch p2DebugKeyTimer = new Stopwatch();

    private int p1DebugKeyIndex = 0;
    private int p2DebugKeyIndex = 0;

    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (!inputControl)
        {
            return;
        }

        // Debug
        if (Input.GetKeyDown(debugKeyP1Combo[p1DebugKeyIndex]))
        {
            if(p1DebugKeyTimer.IsRunning)
            {
                if(p1DebugKeyTimer.ElapsedMilliseconds < debugKeyInputDuration)
                {
                    p1DebugKeyIndex++;
                    if(p1DebugKeyIndex == debugKeyP1Combo.Length)
                    {
                        p1Debug();
                        p1DebugKeyIndex = 0;
                        p1DebugKeyTimer.Stop();
                    }
                }
                else
                {
                    p1DebugKeyIndex = 0;
                    p1DebugKeyTimer.Stop();
                }
            }
            else
            {
                p1DebugKeyTimer.Restart();
                p1DebugKeyIndex++;
            }
        }
        else
        {
            if (p1DebugKeyTimer.ElapsedMilliseconds > debugKeyInputDuration)
            {
                p1DebugKeyIndex = 0;
                p1DebugKeyTimer.Stop();
            }
        }

        if (Input.GetKeyDown(debugKeyP2Combo[p2DebugKeyIndex]))
        {
            if (p2DebugKeyTimer.IsRunning)
            {
                if (p2DebugKeyTimer.ElapsedMilliseconds < debugKeyInputDuration)
                {
                    p2DebugKeyIndex++;
                    if (p2DebugKeyIndex == debugKeyP2Combo.Length)
                    {
                        p2Debug();
                        p2DebugKeyIndex = 0;
                        p2DebugKeyTimer.Stop();
                    }
                }
                else
                {
                    p2DebugKeyIndex = 0;
                    p2DebugKeyTimer.Stop();
                }
            }
            else
            {
                p2DebugKeyTimer.Restart();
                p2DebugKeyIndex++;
            }
        }
        else
        {
            if (p2DebugKeyTimer.ElapsedMilliseconds > debugKeyInputDuration)
            {
                p2DebugKeyIndex = 0;
                p2DebugKeyTimer.Stop();
            }
        }

        // Move

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            p1Move.x = Input.GetAxis("Horizontal1");
            p1Move.y = Input.GetAxis("Vertical1");
            if (p1MoveAction != null)
            {
                p1MoveAction(p1Move);
            }
        }
        else
        {
            if (p1MoveAction != null)
            {
                p1MoveStopAction();
            }
        }

        if (Input.GetKey(KeyCode.UpArrow) || 
            Input.GetKey(KeyCode.DownArrow) || 
            Input.GetKey(KeyCode.LeftArrow) || 
            Input.GetKey(KeyCode.RightArrow))
        {
            p2Move.x = Input.GetAxis("Horizontal2");
            p2Move.y = Input.GetAxis("Vertical2");
            if (p2MoveAction != null)
            {
                p2MoveAction(p2Move);
            }
        }
        else
        {
            if (p2MoveAction != null)
            {
                p2MoveStopAction();
            }
        }

        // Flash

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (p1Flash.y == 1 && p1FlashDoubleClickTimer.ElapsedMilliseconds < doubleClickDuration)
            {
                p1FlashAction(p1Flash);
            }
            else
            {
                p1Flash.x = 0;
                p1Flash.y = 1;
                p1FlashDoubleClickTimer.Restart();
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (p1Flash.y == -1 && p1FlashDoubleClickTimer.ElapsedMilliseconds < doubleClickDuration)
            {
                p1FlashAction(p1Flash);
            }
            else
            {
                p1Flash.x = 0;
                p1Flash.y = -1;
                p1FlashDoubleClickTimer.Restart();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (p1Flash.x == -1 && p1FlashDoubleClickTimer.ElapsedMilliseconds < doubleClickDuration)
            {
                p1FlashAction(p1Flash);
            }
            else
            {
                p1Flash.x = -1;
                p1Flash.y = 0;
                p1FlashDoubleClickTimer.Restart();
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (p1Flash.x == 1 && p1FlashDoubleClickTimer.ElapsedMilliseconds < doubleClickDuration)
            {
                p1FlashAction(p1Flash);
            }
            else
            {
                p1Flash.x = 1;
                p1Flash.y = 0;
                p1FlashDoubleClickTimer.Restart();
            }
        }
        else
        {
            if (p1FlashDoubleClickTimer.ElapsedMilliseconds > doubleClickDuration)
            {
                p1FlashDoubleClickTimer.Stop();
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (p2Flash.y == 1 && p2FlashDoubleClickTimer.ElapsedMilliseconds < doubleClickDuration)
            {
                p2FlashAction(p2Flash);
            }
            else
            {
                p2Flash.x = 0;
                p2Flash.y = 1;
                p2FlashDoubleClickTimer.Restart();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (p2Flash.y == -1 && p2FlashDoubleClickTimer.ElapsedMilliseconds < doubleClickDuration)
            {
                p2FlashAction(p2Flash);
            }
            else
            {
                p2Flash.x = 0;
                p2Flash.y = -1;
                p2FlashDoubleClickTimer.Restart();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (p2Flash.x == -1 && p2FlashDoubleClickTimer.ElapsedMilliseconds < doubleClickDuration)
            {
                p2FlashAction(p2Flash);
            }
            else
            {
                p2Flash.x = -1;
                p2Flash.y = 0;
                p2FlashDoubleClickTimer.Restart();
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (p2Flash.x == 1 && p2FlashDoubleClickTimer.ElapsedMilliseconds < doubleClickDuration)
            {
                p2FlashAction(p2Flash);
            }
            else
            {
                p2Flash.x = 1;
                p2Flash.y = 0;
                p2FlashDoubleClickTimer.Restart();
            }
        }
        else
        {
            if (p2FlashDoubleClickTimer.ElapsedMilliseconds > doubleClickDuration)
            {
                p2FlashDoubleClickTimer.Stop();
            }
        }

        // atk 1
        if (Input.GetKeyDown(KeyCode.T))
        {
            p1MeleeAtk();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            p2MeleeAtk();
        }

        // atk 2
        if (Input.GetKeyDown(KeyCode.Y))
        {
            p1ProjectionAtk();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            p2ProjectionAtk();
        }

        // atk 3
        if (Input.GetKeyDown(KeyCode.U))
        {
            p1NovaAtk();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            p2NovaAtk();
        }
    }
}
