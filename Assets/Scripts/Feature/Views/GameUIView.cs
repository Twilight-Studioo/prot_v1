#region

using System;
using Core.Utilities;
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

        private void Start()
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

        public void SetCameraPosition(Vector3 position)
        {
            mainCamera.transform.position = position;
        }
    }
}