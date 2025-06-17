using TMPro;
using UnityEngine;

namespace HelloWorld.Utils
{
    public class LocalizedText : MonoBehaviour
    {
        public string key;
        private TextMeshProUGUI textComponent;

        private void Start()
        {
            textComponent = GetComponent<TextMeshProUGUI>();
            UpdateText();
        }

        public void UpdateText()
        {
            if (LocalizationController.Instance != null)
            {
                textComponent.text = LocalizationController.Instance.GetText(key);
            }
        }

        private void OnEnable()
        {
            LocalizationController.OnChangeLanguage += LocalizationController_OnChangeLanguage;
            if (textComponent != null)
                UpdateText();
        }

        private void OnDisable()
        {
            LocalizationController.OnChangeLanguage -= LocalizationController_OnChangeLanguage;
        }

        private void LocalizationController_OnChangeLanguage(string obj)
        {
            Debug.Log("Update language", this);
            UpdateText();
        }
    }
}