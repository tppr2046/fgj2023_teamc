
using UnityEngine;
using UnityEngine.UI;

namespace FGJ_2023_TeamC
{
public class BloodBarController : MonoBehaviour
{
        //public float barSlice;
        bool GameStart = false;
 //       private float timer;
 //       float durition;
        public Image Player_1_Blood_Bar;
        public Image Player_2_Blood_Bar;
        public Image Player_1_Magic_Bar;
        public Image Player_2_Magic_Bar;


        public void Player_1_Hit(float hitPercent)
        {
            Player_1_Blood_Bar.fillAmount = hitPercent;
        }

        public void Player_2_Hit(float hitPercent)
        {
            Player_2_Blood_Bar.fillAmount = hitPercent;
        }

        public void Player_1_Magic(float magicPercent)
        {
            Player_1_Magic_Bar.fillAmount = magicPercent;
        }

        public void Player_2_Magic(float magicPercent)
        {
            Player_2_Magic_Bar.fillAmount = magicPercent;
        }


    }

}
