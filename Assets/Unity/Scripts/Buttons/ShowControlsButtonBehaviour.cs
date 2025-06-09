using UnityEngine;

namespace Unity.Scripts.Buttons {
    public class ShowControlsBehaviour : MonoBehaviour {
        public void OnClick() {
            GameControllerBehaviour.Instance.SwitchShowControls();
        }
    }
}
