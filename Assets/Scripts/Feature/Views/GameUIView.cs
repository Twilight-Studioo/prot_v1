#region

using Core.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Feature.Views
{
    [RequireComponent(typeof(Camera))]
    public class GameUIView : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Slider healthSlider;

        public IReadOnlyReactiveProperty<Vector2> CameraPosition;
        public void DoStart()
        {
            if (mainCamera.IsNull() || healthSlider.IsNull())
            {
                throw new NoAttachedException($"{mainCamera} {healthSlider}");
            }
        }

        public void SetHealth(int hp)
        {
            healthSlider.value = hp;
        }

        public void SetHealthRange(int n, int x)
        {
            healthSlider.minValue = n;
            healthSlider.maxValue = x;
            healthSlider.value = x;
        }

        private void FixedUpdate()
        {
            if (!CameraPosition.IsNull())
            {
                mainCamera.transform.position = Vector2.Lerp(mainCamera.transform.position.ToVector2(), CameraPosition.Value, Time.deltaTime).ToVector3(SysEx.Unity.ZIndex.Camera);
            }
        }
    }
}