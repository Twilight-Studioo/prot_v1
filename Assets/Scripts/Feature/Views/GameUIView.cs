#region

using UnityEngine;
using UnityEngine.UIElements;

#endregion

namespace Feature.Views
{
    [RequireComponent(typeof(Camera))]
    public class GameUIView : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Slider slider;

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