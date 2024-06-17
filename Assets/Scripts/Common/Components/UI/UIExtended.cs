using UnityEngine;

namespace Assets.Scripts.Common.Components.UI
{
    public class UIExtended : MonoBehaviour
    {
        public void SetNotActive(bool value)
        {
            this.gameObject.SetActive(!value);
        }
    }
}
