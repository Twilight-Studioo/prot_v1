#region

using UnityEngine;

#endregion

namespace Feature.Views
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SwapItemView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer material;

        public void SetHighlight(bool isHighlight)
        {
            material.color = isHighlight ? Color.red : Color.blue;
        }
    }
}