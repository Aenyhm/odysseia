using UnityEngine;

namespace Unity.Scripts.Buttons {
    public class ExitButtonBehaviour : AbstractButton {
        protected override void DoAction() {
            Application.Quit();
        }
    }
}
