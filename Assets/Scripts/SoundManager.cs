using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

public class SoundManager : MonoBehaviour
{
    public PlayMakerFSM fsm;

    //擊中音
    public void HitSound()
    {
        fsm.SendEvent("HITSOUND");
    }

    //衝刺
    public void DashSound()
    {
        fsm.SendEvent("DASHSOUND");
    }

    //技能
    public void SkillSound()
    {
        fsm.SendEvent("SKILLSOUND");

    }

    //射擊
    public void ShootSound()
    {
        fsm.SendEvent("SHOOTSOUND");
    }

    //死亡
    public void DieSound()
    {
        fsm.SendEvent("DIESOUND");
    }

}
