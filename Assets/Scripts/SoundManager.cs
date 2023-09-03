using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

public class SoundManager : MonoBehaviour
{
    public PlayMakerFSM fsm;

    //擊中音
    void HitSound()
    {
        fsm.SendEvent("HITSOUND");
    }

    //衝刺
    void DashSound()
    {
        fsm.SendEvent("DASHSOUND");
    }

    //技能
    void SkillSound()
    {
        fsm.SendEvent("SKILLSOUND");

    }

    //射擊
    void ShootSound()
    {
        fsm.SendEvent("SHOOTSOUND");
    }

    //死亡
    void DieSound()
    {
        fsm.SendEvent("DIESOUND");
    }

}
