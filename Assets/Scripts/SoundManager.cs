using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

public class SoundManager : MonoBehaviour
{
    public PlayMakerFSM fsm;

    //������
    public void HitSound()
    {
        fsm.SendEvent("HITSOUND");
    }

    //�Ĩ�
    public void DashSound()
    {
        fsm.SendEvent("DASHSOUND");
    }

    //�ޯ�
    public void SkillSound()
    {
        fsm.SendEvent("SKILLSOUND");

    }

    //�g��
    public void ShootSound()
    {
        fsm.SendEvent("SHOOTSOUND");
    }

    //���`
    public void DieSound()
    {
        fsm.SendEvent("DIESOUND");
    }

}
