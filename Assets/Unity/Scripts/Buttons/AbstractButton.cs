using UnityEngine;
using UnityEngine.UI;

namespace Unity.Scripts.Buttons {
    
    // Bouton qui fait une action au clic ou à un input spécifique.
    [RequireComponent(typeof(Button))]
    public abstract class AbstractButton : MonoBehaviour {
        [SerializeField] private PlayerActionType _actionType;
        private Button _button;
        
        void Awake() => _button = GetComponent<Button>();
        void OnEnable() => _button.onClick.AddListener(DoAction);
        void OnDisable() => _button.onClick.RemoveListener(DoAction);
        
        void Update() {
            if (UnityInput.Instance.GetAction(_actionType)) DoAction();
        }
        
        protected abstract void DoAction();
    }
}
