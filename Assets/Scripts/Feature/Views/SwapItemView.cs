using System;
using UnityEngine;

namespace Feature.Views
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SwapItemView: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer material;
        
        Vector3 startPosition;
        private void Start()
        {
            startPosition = transform.position;
        }
        
        public void SetHighlight(bool isHighlight)
        {
            material.color = isHighlight ? Color.red : Color.blue;
        }
    }
}