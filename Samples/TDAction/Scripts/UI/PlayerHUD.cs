using System.Collections;
using System.Collections.Generic;
using TDAction.Fighter;
using UnityEngine;
using UnityEngine.UI;

namespace TDAction.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        enum HealthCatchup
        {
            NONE = 0,
            DAMAGE = 1,
            HEAL = 2
        }

        private FighterManager entity;

        public Image frontHealthBar;
        public Image backHealthBar;

        public Color healHealthColor;
        public Color damageHealthColor;
        public float chipSpeed = 1;
        
        float lerpTimer = 0;
        HealthCatchup catchupType;

        public void SetEntity(FighterManager entity)
        {
            this.entity = entity;
        }

        private void Update()
        {
            switch (catchupType)
            {
                case HealthCatchup.DAMAGE:
                    lerpTimer += Time.deltaTime;
                    backHealthBar.fillAmount = Mathf.Lerp(backHealthBar.fillAmount, frontHealthBar.fillAmount, lerpTimer / chipSpeed);
                    if(backHealthBar.fillAmount == frontHealthBar.fillAmount)
                    {
                        catchupType = HealthCatchup.NONE;
                    }
                    break;
                case HealthCatchup.HEAL:
                    lerpTimer += Time.deltaTime;
                    frontHealthBar.fillAmount = Mathf.Lerp(frontHealthBar.fillAmount, backHealthBar.fillAmount, lerpTimer / chipSpeed);
                    if(frontHealthBar.fillAmount == backHealthBar.fillAmount)
                    {
                        catchupType = HealthCatchup.NONE;
                    }
                    break;
            }
        }

        public void Heal(float value, float maxValue)
        {
            lerpTimer = 0;
            backHealthBar.fillAmount = value / maxValue;
            backHealthBar.color = healHealthColor;
            catchupType = HealthCatchup.HEAL;
        }

        public void Damage(float value, float maxValue)
        {
            lerpTimer = 0;
            frontHealthBar.fillAmount = value / maxValue;
            backHealthBar.color = damageHealthColor;
            catchupType = HealthCatchup.DAMAGE;
        }
    }
}