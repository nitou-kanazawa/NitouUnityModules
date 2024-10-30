#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

// [参考]
//  qiita: UnityEditor.AnimatedValues https://qiita.com/kyusyukeigo/items/baac7a5075d6210fd3b3
//  コガネブログ: EditorGUIUtility.IconContentで使用できる1000種類以上のアイコンがまとめられているGitHubリポジトリ https://baba-s.hatenablog.com/entry/2019/10/30/100000
//  LIGHT11: SerializedObjectの勘所をまとめてみる https://light11.hatenadiary.com/entry/2018/03/15/225709

namespace nitou.AnimationModule.Editor {
    using nitou.EditorShared;
    using UnityEditor.SceneManagement;

    [CustomEditor(typeof(AnimationPreview))]
    public class AnimationPreviewEditor : UnityEditor.Editor {

        private AnimationPreview _instance;

        // Property
        private SerializedProperty TargetObjectProperty;
        private SerializedProperty TargetAnimatorProperty;
        private SerializedProperty AnimationClipProperty;
        private SerializedProperty CollidersProperty;

        private readonly static AnimBool _generalState = new(false);
        private readonly static AnimBool _animationState = new(false);
        private readonly static AnimBool _particleState = new(false);
        private readonly static AnimBool _colliderState = new(false);


        /// ----------------------------------------------------------------------------
        // Editor Method

        private void OnEnable() {
            _instance = target as AnimationPreview;

            // Properity
            TargetObjectProperty = serializedObject.FindProperty("_target");
            TargetAnimatorProperty = serializedObject.FindProperty("_animator");
            AnimationClipProperty = serializedObject.FindProperty("_animationClip");
            CollidersProperty = serializedObject.FindProperty("_colliders");

            EditorApplication.update += OnUpdate;
        }

        private void OnDisable() {
            if (AnimationPreview.autoStopOnDisable) {
                _instance.StopButton();
            }
            EditorApplication.update -= OnUpdate;
        }

        private void OnUpdate() => base.Repaint();

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorUtil.GUI.MonoBehaviourField(_instance);
            EditorUtil.GUI.ScriptableObjectField(this);     // ※しばらくはEditorコードも触りそうなので追加
            GUILayout.Space(pixels: 5);

            // カラー設定
            var simulationColor = _instance.IsSimulating ? Colors.GreenYellow : Colors.White;
            GUI.backgroundColor = simulationColor;

            // 
            DrawGeneralInspector();
            DrawAnimationInspector();
            DrawParticleInspector();
            DrawColliderInspector();

            // Timeline
            if (!IsPrefabInProjectView(_instance.gameObject)) {
                GUILayout.Space(pixels: 5);
                DrawTimelineInspector();
            } else {
                EditorGUILayout.HelpBox(Style.ProjectViewMsg, MessageType.Info);
            }

            serializedObject.ApplyModifiedProperties();
        }


        /// ----------------------------------------------------------------------------
        // Private Method (Inspector)

        private void DrawGeneralInspector() {
            _generalState.target = EditorUtil.GUI.FoldoutHeader("General Settings", _generalState.target);
            using (var group = new EditorGUILayout.FadeGroupScope(_generalState.faded))
            using (new EditorUtil.IndentScope()) {
                if (group.visible) {

                    // Lockボタン
                    using (new EditorGUILayout.HorizontalScope()) {
                        var theHeight = 2f;
                        Rect buttonRect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false, 38, Style.LockButtonOptions));
                        buttonRect.position += Vector2.up * theHeight;
                        buttonRect.size = new Vector2(38, 38);
                        var helpBoxRect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false, 38));
                        helpBoxRect.position += Vector2.up * theHeight;
                        var move = 10f;
                        // helpBoxRect.width += buttonRect.width + move;
                        helpBoxRect.position += new Vector2(-move, 0f); ;

                        // Lock状態
                        if (AnimationPreview.autoStopOnDisable) {
                            if (GUI.Button(buttonRect, Style.lockButtonContent, Style.lockButtonStyle)) {
                                AnimationPreview.autoStopOnDisable = false;
                            }
                            EditorGUI.HelpBox(helpBoxRect, Style.LockMsg, MessageType.Info);
                        }
                        // Unlock状態
                        else {
                            if (GUI.Button(buttonRect, Style.unlockButtonContent, Style.lockButtonStyle)) {
                                AnimationPreview.autoStopOnDisable = true;
                            }
                            EditorGUI.HelpBox(helpBoxRect, Style.UnlockMsg, MessageType.Info);
                        }
                    }

                    // Property
                    _instance.PlaybackSpeed = EditorGUILayout.FloatField("Playback Speed", _instance.PlaybackSpeed);
                }
            }
        }

        private void DrawAnimationInspector() {
            _animationState.target = EditorUtil.GUI.FoldoutHeader("Animation Settings", _animationState.target);
            using (var group = new EditorGUILayout.FadeGroupScope(_animationState.faded))
            using (new EditorUtil.IndentScope()) {
                if (group.visible) {

                    // Object reference
                    using (new EditorGUI.DisabledGroupScope(_instance.IsSimulating)) {
                        EditorGUILayout.PropertyField(TargetObjectProperty);
                        EditorUtil.GUI.ReadOnlyPropertyField(TargetAnimatorProperty);
                        EditorGUILayout.PropertyField(AnimationClipProperty);
                    }
                    EditorUtil.GUI.HorizontalLine();

                    // Parameters
                    //_instance.AnimationSpeed = EditorGUILayout.FloatField(Style.AnimationSpeedContent, _instance.AnimationSpeed);
                    _instance.AnimationDelay = EditorGUILayout.FloatField(Style.AnimationDelayContent, _instance.AnimationDelay);
                }
            }
        }

        private void DrawParticleInspector() {
            _particleState.target = EditorUtil.GUI.FoldoutHeader("Particle Settings", _particleState.target);
            using (var group = new EditorGUILayout.FadeGroupScope(_particleState.faded))
            using (new EditorUtil.IndentScope()) {
                if (group.visible) {

                }
            }
        }

        private void DrawColliderInspector() {
            _colliderState.target = EditorUtil.GUI.FoldoutHeader("Collider Settings", _colliderState.target);
            using (var group = new EditorGUILayout.FadeGroupScope(_colliderState.faded))
            using (new EditorUtil.IndentScope()) {
                if (group.visible) {

                    // Object reference
                    using (new EditorGUI.DisabledGroupScope(_instance.IsSimulating)) {
                        EditorGUILayout.PropertyField(CollidersProperty);
                    }
                }
            }
        }

        private void DrawTimelineInspector() {

            using (new EditorGUILayout.HorizontalScope()) {
                if (_instance.IsSimulating) {
                    var second = Mathf.FloorToInt(_instance.PlaybackValue);
                    var frame = ((_instance.PlaybackValue % 1) * 60);
                    string timeString = (_instance.Clip != null)
                        ? $"{second:f0}:{frame:00}ms (Animation {(_instance.AnimationPlaybackPercent * 100f):f0} %)"
                        : $"{second:f0}:{frame:00}ms";

                    GUILayout.Label($"Playback {timeString}", Style.TitleStyle);
                } else {
                    GUILayout.Label($"", Style.TitleStyle);
                }

            }


            using (new EditorGUILayout.HorizontalScope()) {

                // Stopボタン
                var stopButtonColor = _instance.IsSimulating ? Color.red : Color.white;
                using (new EditorUtil.GUIBackgroundColorScope(stopButtonColor)) {
                    if (GUILayout.Button(Style.stopButtonContent, Style.timelineButtonStyle, Style.ButtonOptions)) {
                        _instance.StopButton();
                    }
                }

                // Playボタン
                var playButtonContent = _instance.IsPaused ? Style.playButtonContent : Style.pauseButtonContent;
                if (GUILayout.Button(playButtonContent, Style.timelineButtonStyle, Style.ButtonOptions)) {
                    if (_instance.IsPaused) {
                        _instance.PlayButton();
                    } else {
                        _instance.PauseButton();
                    }
                }

                // Slider
                {
                    var rectHeight = 20f;
                    var floatFieldWidth = 30f;
                    var spacing = 2f;
                    var heightOffset = 2f;

                    Rect rect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false, rectHeight));
                    var baseWidth = rect.width;


                    // Start value
                    rect.width = floatFieldWidth;
                    rect.position += Vector2.up * heightOffset + Vector2.right * spacing;
                    EditorGUI.BeginChangeCheck();
                    _instance.PlaybackStart = EditorGUI.FloatField(rect, _instance.PlaybackStart);
                    if (EditorGUI.EndChangeCheck()) {
                        UpdatePrefab();
                    }

                    // Slider
                    rect.width = baseWidth - ((floatFieldWidth + spacing) * 2f);
                    rect.position += Vector2.right * (floatFieldWidth + spacing * 2f);
                    var value = EditorGUI.Slider(rect, _instance.PlaybackValue, _instance.PlaybackStart, _instance.PlaybackEnd);
                    if (value != _instance.PlaybackValue) {
                        _instance.PlaybackValue = value;
                        if (!_instance.IsSimulating) {
                            _instance.PlayButton();
                        }
                        // Target.PauseButton(); // If you want to pause when you scrub the playback instead of letting it play, uncomment this line.
                        _instance.IsScrubbing = true;
                    } else {
                        if (_instance.IsScrubbing && !_instance.IsPaused) {
                            _instance.PlayButton();
                            _instance.IsScrubbing = false;
                        }
                    }

                    // End value
                    rect.position = new Vector2(rect.max.x + spacing, rect.position.y);
                    rect.width = floatFieldWidth - spacing;
                    EditorGUI.BeginChangeCheck();
                    _instance.PlaybackEnd = EditorGUI.FloatField(rect, _instance.PlaybackEnd);
                    if (EditorGUI.EndChangeCheck()) {
                        UpdatePrefab();
                    }

                }
            }
        }

        /// ----------------------------------------------------------------------------
        // Private Method

        private static void UpdatePrefab() {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null) {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }

        private static bool IsPrefabInProjectView(GameObject gObject) {
            return gObject.scene.IsValid() == false;
        }


        /// ----------------------------------------------------------------------------
        // Static Method

        public static class Style {

            public static readonly GUIContent playButtonContent;
            public static readonly GUIContent pauseButtonContent;
            public static readonly GUIContent stopButtonContent;
            public static readonly GUIContent lockButtonContent;
            public static readonly GUIContent unlockButtonContent;
            //
            public static readonly GUIContent AnimationContent;
            public static readonly GUIContent AnimatedContent;
            public static readonly GUIContent AnimationSpeedContent;
            public static readonly GUIContent AnimationDelayContent;

            // style
            public static readonly GUIStyle timelineButtonStyle;
            public static readonly GUIStyle lockButtonStyle;
            public static readonly GUIStyle TitleStyle;

            // options
            public static readonly GUILayoutOption[] ButtonOptions;
            public static readonly GUILayoutOption[] LockButtonOptions;

            // message text
            public static readonly string WARNING_ANIMATED_IS_NOT_SCENE_OBJECT = $" is not a SCENE gameObject. Add to {nameof(AnimationPreview)} a gameObject from a SCENE with an Animator/Animation component (and not the project window).";
            public static readonly string WARNING_PS_IS_NOT_SCENE_OBJECT = $" is not a SCENE ParticleSystem. Add to {nameof(AnimationPreview)} a ParticleSystem from a SCENE (not the project window).";
            public static readonly string LockMsg = "Stops previewing on previewer deselected";
            public static readonly string UnlockMsg = "Keeps previewing on previewer deselected";
            public static readonly string ProjectViewMsg = "Playback not available from the Project view.";


            static Style() {

                // content
                playButtonContent = EditorGUIUtility.IconContent("PlayButton", "Play");
                playButtonContent.tooltip = "PLAY";
                pauseButtonContent = EditorGUIUtility.IconContent("PauseButton", "Pause");
                pauseButtonContent.tooltip = "PAUSE";
                stopButtonContent = EditorGUIUtility.IconContent("ol_minus", "Preview");
                stopButtonContent.tooltip = "STOP";

                lockButtonContent = EditorGUIUtility.IconContent("IN LockButton@2x", "Lock");
                lockButtonContent.tooltip = $"The animation will play even when this gameObject is deselected\n\nTIP: This is useful if you want to edit objects while previewing your animations.";
                unlockButtonContent = EditorGUIUtility.IconContent("IN LockButton on@2x", "Unlock");
                unlockButtonContent.tooltip = $"When you deselect this gameObject, STOP is called automatically and the preview stops";

                AnimatedContent = new GUIContent("Animated", "A SCENE gameObject.\nIt must have an Animator/Animation component.");
                AnimationContent = new GUIContent("Animation", "What animation should we preview?\nThe animations are read from an Animation/Animator component.");
                AnimationDelayContent = new GUIContent("Delay", "Delays the animation of the Animated object.\nDefault = 0");
                AnimationSpeedContent = new GUIContent("Speed", "The Animated object's animation speed.\nDefault = 1");


                // style
                timelineButtonStyle = new GUIStyle("Button");
                lockButtonStyle = new GUIStyle("Button");
                TitleStyle = new GUIStyle("BoldLabel") { alignment = TextAnchor.MiddleCenter };


                // options
                ButtonOptions = new GUILayoutOption[] {
                    GUILayout.MinWidth(25),
                    GUILayout.MaxWidth(40),
                    GUILayout.Height(25)
                };
                LockButtonOptions = new GUILayoutOption[] {
                    GUILayout.MinWidth(40),
                    GUILayout.MaxWidth(60)
                };

            }
        }
    }
}
#endif