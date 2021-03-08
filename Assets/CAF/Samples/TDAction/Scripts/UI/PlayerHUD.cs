using System.Collections;
using System.Collections.Generic;
using TDAction.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace TDAction.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        private FighterManager entity;
        public Image healthImage;

        public void SetEntity(FighterManager entity)
        {
            this.entity = entity;
        }

        public void SetHealthValue(float value, float maxValue)
        {
            healthImage.fillAmount = value / maxValue;
        }
    }
}