using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HelloWorld.Utils
{
    [AddComponentMenu("UI/Horizontal Selector", 100)]
    public class HorizontalSelector : Selectable, IPointerClickHandler, ISubmitHandler, ICancelHandler, IMoveHandler
    {
        [SerializeField] private TextMeshProUGUI label; // ou TMP_Text se estiver usando TextMeshPro
        [SerializeField] private bool loopable = true;
        [SerializeField] private List<string> options = new List<string>();

        private int currentIndex = 0;
        private float inputDelay = 0.3f;
        private float lastInputTime = 0f;
        private bool isFocused = false;

        [System.Serializable]
        public class IntStringEvent : UnityEngine.Events.UnityEvent<int> { }

        [SerializeField] public IntStringEvent onValueChanged = new IntStringEvent();

        public int GetValue() => currentIndex;
        public string GetCurrentOption() => options.Count > 0 ? options[currentIndex] : string.Empty;

        public void RefreshShownValue()
        {
            UpdateLabel();
        }

        /// <summary>
        /// Remove todas as opções do selector e limpa o label.
        /// </summary>
        public void ClearOptions()
        {
            options.Clear();
            currentIndex = 0;
            UpdateLabel();
        }

        /// <summary>
        /// Define o valor do selector para o índice informado.
        /// </summary>
        public void SetValue(int index, bool callback = true)
        {
            if (options == null || options.Count == 0) return;
            if (index < 0 || index >= options.Count) return;

            currentIndex = index;
            UpdateLabel();
            if (callback)
                onValueChanged?.Invoke(currentIndex);
        }


        public override void OnMove(AxisEventData eventData)
        {
            if (!isFocused || Time.unscaledTime - lastInputTime < inputDelay) return;

            switch (eventData.moveDir)
            {
                case MoveDirection.Right:
                    NextOption();
                    lastInputTime = Time.unscaledTime;
                    eventData.Use(); // impede propagação
                    break;

                case MoveDirection.Up:
                    Navigate(eventData, FindSelectableOnUp());
                    break;

                case MoveDirection.Left:
                    PreviousOption();
                    lastInputTime = Time.unscaledTime;
                    eventData.Use();
                    break;

                case MoveDirection.Down:
                    Navigate(eventData, FindSelectableOnDown());
                    break;
            }
        }

        void Navigate(AxisEventData eventData, Selectable sel)
        {
            if (sel != null && sel.IsActive())
                eventData.selectedObject = sel.gameObject;
        }

        public void SetOptions(List<string> newOptions, int defaultIndex = 0)
        {
            options = newOptions;
            currentIndex = Mathf.Clamp(defaultIndex, 0, options.Count - 1);
            UpdateLabel();
        }

        public void NextOption()
        {
            if (options.Count == 0) return;

            if (loopable)
            {
                currentIndex = (currentIndex + 1) % options.Count;
            }
            else
            {
                if (currentIndex < options.Count - 1)
                    currentIndex++;
            }

            UpdateLabel();
            onValueChanged?.Invoke(currentIndex);
        }

        public void PreviousOption()
        {
            if (options.Count == 0) return;

            if (loopable)
            {
                currentIndex = (currentIndex - 1 + options.Count) % options.Count;
            }
            else
            {
                if (currentIndex > 0)
                    currentIndex--;
            }

            UpdateLabel();
            onValueChanged?.Invoke(currentIndex);
        }

        public void SetValueWithoutNotify(int value)
        {
            currentIndex = Mathf.Clamp(value, 0, options.Count - 1);
            UpdateLabel();
        }

        protected override void Start()
        {
            base.Start();
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            if (label != null)
                label.text = options.Count > 0 ? options[currentIndex] : string.Empty;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            isFocused = true;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            isFocused = false;
        }

        public void OnCancel(BaseEventData eventData)
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.selectedObject == gameObject)
            {
                loopable = true;
                NextOption();
                eventData.Use();
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (eventData.selectedObject == gameObject)
            {
                loopable = true;
                NextOption();
                eventData.Use();
            }
        }
    }
}