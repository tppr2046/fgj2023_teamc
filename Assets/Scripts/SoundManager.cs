using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

public class SoundManager : MonoBehaviour
{
    public PlayMakerFSM fsm;

    //������
    void HitSound()
    {
        fsm.SendEvent("HITSOUND");
    }

    //�Ĩ�
    void DashSound()
    {
        fsm.SendEvent("DASHSOUND");
    }

    //�ޯ�
    void SkillSound()
    {
        fsm.SendEvent("SKILLSOUND");

    }

    //�g��
    void ShootSound()
    {
        fsm.SendEvent("SHOOTSOUND");
    }

    //���`
    void DieSound()
    {
        fsm.SendEvent("DIESOUND");
    }

}
