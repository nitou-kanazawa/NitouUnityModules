#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

namespace nitou.LevelActors.Controller.EditorScripts.Overlays {
    
    using Controller.Core;
    using Controller.Interfaces.Components;

    /// <summary>
    /// Editor tools for registering TCC-related Overlays.
    /// Registers <see cref="ControlPriorityOverlay"/>, <see cref="VelocityOverlay"/>,
    /// and <see cref="EffectOverlay"/>.
    /// </summary>
    [EditorTool("Control Priority", typeof(IBrain))]
    public class LacEditorTool : EditorTool {

        private EffectOverlay _effectOverlay;
        private ControlPriorityOverlay _priorityOverlay;
        private VelocityOverlay _velocityOverlay;

        public override bool IsAvailable() => false; // Do not display the editor tool icon

        private void Initialize() {
            var obj = ((MonoBehaviour)target).gameObject;

            if (obj.TryGetComponent<ActorSettings>(out var settings)) {
                _effectOverlay = new EffectOverlay((settings.EffectParent != null) ? settings.EffectParent.gameObject : obj);
                _priorityOverlay = new ControlPriorityOverlay((settings.ControlParent!= null) ? settings.ControlParent.gameObject : obj);
                _velocityOverlay = new VelocityOverlay(obj.GetComponent<IBrain>());
            }
        }

        private void OnEnable() {
            Initialize();

            SceneView.AddOverlayToActiveView(_effectOverlay);
            SceneView.AddOverlayToActiveView(_priorityOverlay);
            SceneView.AddOverlayToActiveView(_velocityOverlay);

        }

        private void OnDisable() {
            SceneView.RemoveOverlayFromActiveView(_effectOverlay);
            SceneView.RemoveOverlayFromActiveView(_priorityOverlay);
            SceneView.RemoveOverlayFromActiveView(_velocityOverlay);
        }

    }
}
#endif