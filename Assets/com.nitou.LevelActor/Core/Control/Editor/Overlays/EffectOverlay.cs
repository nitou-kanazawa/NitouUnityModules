#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Overlays;

namespace nitou.LevelActors.Controller.EditorScripts.Overlays {
    
    using Controller.Interfaces.Core;

    /// <summary>
    /// GameObjectが保持する<see cref="IEffect"/>コンポーネントの一覧と、それぞれの<see cref="IEffect"/>のVelocityを表示するオーバーレイ．
    /// このオーバーレイは<see cref="LacEditorTool"/>によって作成される
    /// </summary>
    [Overlay(defaultDisplay = false, displayName = "Lac Effect Velocity Debug")]
    public class EffectOverlay : Overlay {
        
        private readonly List<IEffect> _velocities = new();

        /// <summary>
        /// コンストラクタ．
        /// </summary>
        public EffectOverlay(GameObject obj) {
            displayName = "LAC Effects";
            obj.GetComponents(_velocities);
        }

        public override VisualElement CreatePanelContent() {
            var root = new VisualElement();
            root.style.width = 200;

            foreach (var effect in _velocities) {
                // Add UI elements
                var velocityField = CreateVelocityElement(root, effect.GetType().Name);

                // Update UI
                UpdateVelocityElement(velocityField, effect);
                if (Application.isPlaying) {
                    velocityField.schedule.Execute(() =>
                        UpdateVelocityElement(velocityField, effect)).Every(16);
                }
            }

            return root;
        }

        /// <summary>
        /// UI要素を更新する．
        /// </summary>
        /// <param name="velocityField">Element to update</param>
        /// <param name="effect">Value used for updating</param>
        private static void UpdateVelocityElement(Vector3Field velocityField, IEffect effect) {
            velocityField.value = effect.Velocity;
        }

        /// <summary>
        /// Velocityを表すUI要素を追加
        /// </summary>
        /// <param name="root">Hierarchy to add to</param>
        /// <param name="effectName">Name of the Effect to display</param>
        /// <returns>Vector3Field to represent the effect</returns>
        private static Vector3Field CreateVelocityElement(VisualElement root, string effectName) {
            // Make UI collapsible with FoldOut
            var foldout = new Foldout();
            foldout.text = effectName;
            foldout.viewDataKey = $"lac-effect-{effectName}";

            // Add field to represent velocity
            var velocityField = new Vector3Field();
            velocityField.SetEnabled(false); // Disable the field to prevent selection and editing

            // Build UI
            foldout.Add(velocityField);
            root.Add(foldout);

            return velocityField;
        }
    }
}
#endif