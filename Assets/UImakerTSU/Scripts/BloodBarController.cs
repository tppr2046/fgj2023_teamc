
using UnityEngine;
using UnityEngine.UI;

namespace FGJ_2023_TeamC
{
public class BloodBarController : MonoBehaviour
{
        //public float barSlice;
        bool GameStart = false;
        private float timer;
        float durition;
        public Image Player_1_Blood_Bar;
        public Image Player_2_Blood_Bar;
        public Image Player_1_Magic_Bar;
        public Image Player_2_Magic_Bar;
        public void Start()
        {
            
            timer = 0;
            durition = 1;
        }
        private void Update()
        {
            
            if (GameStart) {
                timer += Time.deltaTime;
                if (timer > 0)
                {
                    //Player_1_Blood_Bar.fillAmount = Mathf.Lerp(1,0,timer/durition);
                    //Player_2_Blood_Bar.fillAmount = Mathf.Lerp(1, 0, timer / durition);
                    //Player_1_Magic_Bar.fillAmount = Mathf.Lerp(1, 0, timer / durition);
                    //Player_2_Magic_Bar.fillAmount = Mathf.Lerp(1, 0, timer / durition);

                    if (timer >= durition)
                    {
                        GameStart = false;
                        timer = 0;
                    }
                }
                
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Player_1_Blood_Bar.fillAmount -= 0.1f;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Player_1_Blood_Bar.fillAmount -= 0.1f;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                Player_2_Blood_Bar.fillAmount -= 0.1f;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Player_2_Magic_Bar.fillAmount -= 0.1f;
            }
        }
        

    }

}
