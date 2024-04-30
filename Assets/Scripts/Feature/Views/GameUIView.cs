using UnityEngine;
using UnityEngine.UIElements;

namespace Feature.Views
{
    [RequireComponent(typeof(Camera))]
    public class GameUIView: MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Camera mainCamera;
        public void SetHP(ushort hp)
        {
            slider.value = hp;
        }
        
        public void SetCameraPosition(Vector3 position)
        {
            mainCamera.transform.position = position;
        }
    }
}