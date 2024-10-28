using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

namespace nitou.LevelActors.Inputs {

    /// <summary>
    /// <see cref="InputSystem"/>を対象とした入力ハンドラ．
    /// </summary>
    public sealed class InputSystemHandler : InputHandler {

        // アクションリスト
        [SerializeField] InputActionAsset inputActionsAsset = null;
        private readonly Dictionary<string, InputAction> _inputActionsDictionary = new();

        [Space]

        // Action Map によるフィルタリング
        [SerializeField] bool _filterByActionMap = false;

        [ShowIf("_filterByActionMap")]
        [BoxGroup, LabelText("Map name")]
        [SerializeField, Indent] string _gameplayActionMap = "Gameplay";

        // Control Scheme によるフィルタリング
        [SerializeField] bool _filterByControlScheme = false;

        [ShowIf("_filterByControlScheme")]
        [BoxGroup, LabelText("Scheme name")]
        [SerializeField, Indent] string _controlSchemeName = "Keyboard Mouse";


        /// ----------------------------------------------------------------------------
        // Lifecycle Events

        private void Awake() {

            if (inputActionsAsset == null) {
                Debug_.Log("No input actions asset found!");
                return;
            }

            inputActionsAsset.Enable();

            // Control Scheme によるフィルタリング
            if (_filterByControlScheme) {
                string bindingGroup = inputActionsAsset.controlSchemes.First(x => x.name == _controlSchemeName).bindingGroup;
                inputActionsAsset.bindingMask = InputBinding.MaskByGroup(bindingGroup);
            }

            // Action Map によるフィルタリング
            if (_filterByActionMap) {
                var rawInputActions = inputActionsAsset.FindActionMap(_gameplayActionMap).actions;

                rawInputActions.ForEach(action => {
                    _inputActionsDictionary.Add(action.name, action);
                });


            } else {
                for (int i = 0; i < inputActionsAsset.actionMaps.Count; i++) {
                    var actionMap = inputActionsAsset.actionMaps[i];

                    actionMap.actions.ForEach(action => {
                        _inputActionsDictionary.Add(action.name, action);
                    });
                }
            }

        }


        /// ----------------------------------------------------------------------------
        // Overrided Method 

        /// <summary>
        /// Bool型アクションの取得
        /// </summary>
        public override bool GetBool(string actionName) {
            if (!_inputActionsDictionary.TryGetValue(actionName, out var inputAction)) {
                return false;
            }

            return inputAction.ReadValue<float>() >= InputSystem.settings.defaultButtonPressPoint;
        }

        /// <summary>
        /// Float型アクションの取得
        /// </summary>
        public override float GetFloat(string actionName) {
            if (!_inputActionsDictionary.TryGetValue(actionName, out var inputAction)) {
                return 0f;
            }

            return inputAction.ReadValue<float>();
        }

        /// <summary>
        /// Vector2型アクションの取得
        /// </summary>
        public override Vector2 GetVector2(string actionName) {
            if (!_inputActionsDictionary.TryGetValue(actionName, out var inputAction)) {
                return Vector2.zero;
            }

            return inputAction.ReadValue<Vector2>();
        }
    }

}