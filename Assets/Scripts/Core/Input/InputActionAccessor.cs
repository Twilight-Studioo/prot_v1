#region

using System;
using Core.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

#endregion

namespace Core.Input
{
    /// <summary>
    ///     Application内で共通のInputActionAssetを管理するクラス
    /// </summary>
    public class InputActionAccessor : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActionAsset;

        private void Awake()
        {
            if (inputActionAsset.IsNull())
            {
                throw new NotImplementedException("InputActionAsset is not set.");
            }

            Debug.Log("InputActionManager Start");
            inputActionAsset.Enable();
        }

        private void OnDestroy()
        {
            Debug.Log("InputActionManager Dispose");
            inputActionAsset.Disable();
        }

        public InputActionEvent CreateAction(Guid actionName)
        {
            var inputAction = inputActionAsset.FindAction(actionName);
            if (inputAction == null)
            {
                Debug.LogError($"Action '{actionName}' not found.");
                return null;
            }

            var actionEvent = new InputActionEvent(inputAction);
            inputAction.Enable();

            return actionEvent;
        }
    }
}