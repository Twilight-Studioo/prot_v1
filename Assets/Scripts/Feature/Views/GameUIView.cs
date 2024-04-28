using UnityEngine;
using UnityEngine.UIElements;

namespace Feature.Views
{
    public class GameUIView: MonoBehaviour
    {
        [SerializeField] private Slider slider;
        
        public void SetHP(ushort hp)
        {
            slider.value = hp;
        }
    }
}