using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    public class ProgressBar : MonoBehaviour
    {
        [Header("OBJECTS")]
        public Transform loadingBar;
        public Transform textPercent;

        [Header("SETTINGS")]
        public bool isOn;
        public bool restart;
        public bool invert;
        [Range(0, 100)] public float currentPercent;
        [Range(0, 100)] public int speed;

        [Header("SPECIFIED PERCENT")]
        public bool enableSpecified;
        public bool enableLoop;
        [Range(0, 100)] public float specifiedValue;

        private Image fillImage;
        private TextMeshProUGUI percentText;

        void Start()
        {
            fillImage = loadingBar.GetComponent<Image>();
            percentText = textPercent.GetComponent<TextMeshProUGUI>();

            if (enableSpecified == true)
                currentPercent = specifiedValue;

            if(isOn == false)
            {
                loadingBar.GetComponent<Image>().fillAmount = currentPercent / 100;
                percentText.text = ((int)currentPercent).ToString("F0") + "%";
            }
        }

        void Update()
        {
            if (isOn == true)
            {
                // Added logic my Danny Lum - old logic doesn't seem to work with specified value
                if (enableSpecified)
                {
                    if (System.Math.Abs(currentPercent - specifiedValue) > Mathf.Epsilon)
                    {
                        float delta = speed * Time.deltaTime;
                        if (currentPercent < specifiedValue)
                        {
                            if(specifiedValue - currentPercent <= delta)
                            {
                                currentPercent = specifiedValue;
                            }
                            else
                            {
                                currentPercent += speed * Time.deltaTime;
                            }                            
                        }
                        else if(currentPercent > specifiedValue)
                        {
                            if (currentPercent - specifiedValue <= delta)
                            {
                                currentPercent = specifiedValue;
                            }
                            else
                            {
                                currentPercent -= speed * Time.deltaTime;
                            }
                        }
                    }
                
                    fillImage.fillAmount = currentPercent / 100;
                    percentText.text = ((int)currentPercent).ToString("F0") + "%";
                }
                else
                {
                    if (currentPercent <= 100 && enableSpecified == false && invert == false)
                        currentPercent += speed * Time.deltaTime;

                    else if (currentPercent >= 0 && enableSpecified == false && invert == true)
                        currentPercent -= speed * Time.deltaTime;

                    if (currentPercent == 100 || currentPercent >= 100 && restart == true && invert == false && enableSpecified == false)
                        currentPercent = 0;

                    else if (currentPercent == 0 || currentPercent <= 0 && restart == true && invert == true && enableSpecified == false)
                        currentPercent = 100;

                    if (enableSpecified == true)
                    {
                        if (currentPercent <= specifiedValue)
                            currentPercent += speed * Time.deltaTime;

                        if (enableLoop == true && currentPercent == specifiedValue || currentPercent >= specifiedValue)
                            currentPercent = 0;
                    }

                    loadingBar.GetComponent<Image>().fillAmount = currentPercent / 100;
                    textPercent.GetComponent<TextMeshProUGUI>().text = ((int)currentPercent).ToString("F0") + "%";
                }
            }
        }
    }
}