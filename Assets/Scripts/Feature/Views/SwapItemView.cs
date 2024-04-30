using System;
using UnityEngine;

namespace Feature.Views
{
    [RequireComponent(typeof(Material))]
    public class SwapItemView: MonoBehaviour
    {
        Material material;
        
        Vector3 startPosition;
        private void Start()
        {
            material = GetComponent<Material>();
            startPosition = transform.position;
        }
        
        public void SetHighlight(bool isHighlight)
        {
            material.color = isHighlight ? Color.red : Color.white;
        }
    }
}