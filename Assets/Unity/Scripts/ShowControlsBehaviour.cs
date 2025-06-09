using UnityEngine;

namespace Unity.Scripts {
    public class ShowControlsBehaviour : MonoBehaviour {
        public void OnClick() {
            GameControllerBehaviour.Instance.SwitchShowControls();
        }
    }
}
