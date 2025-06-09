using UnityEngine;

namespace Unity.Scripts.Buttons {
    public class ExitButtonBehaviour : MonoBehaviour {
        public void OnClick() {
            Application.Quit();
        }
    }
}
