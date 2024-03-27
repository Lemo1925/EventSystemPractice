using UnityEngine;
using UnityEngine.UI;
using EventSystem;

namespace LoadRecycleObject
{
    public enum ButtonType { LOAD, RECYCLE };

    public class PanelButton : MonoBehaviour, IEventCell
    {
        public string Index { get => gameObject.name; set => gameObject.name = value; }
        public Button Button { get => gameObject.GetComponent<Button>(); }
        public Dropdown dropdown;
        public ButtonType Type;
    }
}

    

