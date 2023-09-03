using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static float doubleClickDuration = 500;

    public bool inputControl = false;
    public Action<Vector2> p1MoveAction, p2MoveAction = null;
    public Action<Vector2> p1FlashAction, p2FlashAction = null;
    public Action p1MeleeAtk, p2MeleeAtk = null;
    public Action p1ProjectionAtk, p2ProjectionAtk = null;
    public Action p1NovaAtk, p2NovaAtk = null;

    private Vector2 p1Move = new Vector2();
    private Vector2 p2Move = new Vector2();

    private Vector2 p1Flash = new Vector2();
    private Vector2 p2Flash = new Vector2();
    private Stopwatch p1FlashDoubleClickTimer = new Stopwatch();
    private Stopwatch p2FlashDoubleClickTimer = new Stopwatch();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!inputControl)
        {
            return;
        }

        // Move

        p1Move.x = Input.GetAxis("Horizontal1");
        p1Move.y = Input.GetAxis("Vertical1");
        if (p1MoveAction != null)
        {
            p1MoveAction(p1Move);
        }
        p2Move.x = Input.GetAxis("Horizontal2");
        p2Move.y = Input.GetAxis("Vertical2");
        if (p2MoveAction != null)
        {
            p2MoveAction(p2Move);
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
