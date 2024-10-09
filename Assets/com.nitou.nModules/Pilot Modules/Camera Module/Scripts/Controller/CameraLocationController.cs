using System.Collections;
using UnityEngine;
using UniRx;

// [参考]
//  qiita: Cinemachineを使わないお手軽なカメラワーク設計 https://qiita.com/flankids/items/0a4f70c9bfb6d20f20eb

namespace nitou.CameraModule
{
    /*

    /// <summary>
    /// 被写体の位置に基づいてカメラの回転、移動、ズームを制御するコンポーネント
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [DisallowMultipleComponent]
    public class CameraLocationController : MonoBehaviour, ICameraController
    {

        [Title("References")]
        [SerializeField, Indent] private Transform _parent;
        [SerializeField, Indent] private Camera _camera;

        [Title("Location Params")]
        [SerializeField, Indent] private float _tiltAngle = 10f;    // cameraTransform.right (X軸)周りの回転角度
        [SerializeField, Indent] private float _headingAngle = 0f;  // parentTransform.up (Y軸)周りの回転角度
        [SerializeField, Indent] private float _distance = 5f;      // 
        [Space]
        [SerializeField, Indent] private Vector2 _offset = Vector2.zero;
        [SerializeField, Indent] private Vector3 _defaultTargetPos = Vector3.zero;

        [Title("Misc")]
        [SerializeField, Indent] bool _autoUpdate = true;

        private ICameraTarget _target;
        private ReactiveProperty<Vector3> _viewCenterRP = new();

        private Coroutine _currentCoroutine;

        public Camera Camera => _camera;

        /// <summary>
        /// 被写体が設定されているかどうか
        /// </summary>
        public bool ExistTarget => _target != null;

        /// <summary>
        /// 回転中心 (被写体の位置)
        /// </summary>
        public Vector3 RotationCenter
        {
            // ※ICameraTargetが設定されている場合は、その座標を使用する
            get => _target != null ? _target.Position : _defaultTargetPos;
        }

        /// <summary>
        /// 画面の中心位置
        /// </summary>
        public IReadOnlyReactiveProperty<Vector3> ViewCenterRP => _viewCenterRP;

        /// <summary>
        /// 傾斜角
        /// </summary>
        public float TiltAngle
        {
            get => _tiltAngle;
            set
            {
                _tiltAngle = value;
                OnValidate();
            }
        }

        /// <summary>
        /// 旋回角
        /// </summary>
        public float HeadingAngle
        {
            get => _headingAngle;
            set
            {
                _headingAngle = value;
                OnValidate();
            }
        }

        /// <summary>
        /// 対象とカメラの距離
        /// </summary>
        public float Distance
        {
            get => _distance;
            set
            {
                _distance = value;
                OnValidate();
            }
        }

        /// <summary>
        /// 画面内オフセット
        /// </summary>
        public Vector2 Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                OnValidate();
            }
        }

        // 定数
        public static readonly bool ENABLED_LIMIT_TILT = false;
        public static readonly bool ENABLED_LIMIT_DISTANCE = true;
        public static readonly float DEFAULT_DISTANCE = 200f;
        public static readonly float MIN_DISTANCE = 100f;     // ※3Dシーンでロボ全体を移す場合、200程度になる模様
        public static readonly float MAX_DISTANCE = 300f;
        public static readonly float MIN_TILT = -90f;
        public static readonly float MAX_TILT = 90f;


        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method

        private void OnValidate()
        {
            if (_camera == null)
            {
                _camera = gameObject.GetComponent<Camera>();
            }
            if (_camera != null)
            {
                _parent = _camera.transform.parent;
            }

            // 傾斜角
            _tiltAngle = ENABLED_LIMIT_TILT ?
                Mathf.Clamp(_tiltAngle, MIN_TILT, MAX_TILT) :   //　(Min ~ X ~ Max)
                AngleUtils.NormalizeAngle(_tiltAngle);  //　(-180 ~ X ~ 180)

            // 旋回角
            _headingAngle = AngleUtils.NormalizeAngle(_headingAngle);

            // 距離
            if (ENABLED_LIMIT_DISTANCE)
                _distance = Mathf.Clamp(_distance, MIN_DISTANCE, MAX_DISTANCE);

            // 位置更新
            UpdateLocation();
        }

        private void OnDestroy()
        {
            _viewCenterRP?.Dispose();
        }

        private void Update()
        {
            if (!_autoUpdate)
                return;

            UpdateLocation();
        }


        /// ----------------------------------------------------------------------------
        // Public Method

        /// <summary>
        /// 被写体を設定する
        /// </summary>
        public void SetTarget(Vector3 targetPos, bool resetOffset = true)
        {
            _defaultTargetPos = targetPos;
            _target = null;     // ※target objはクリア
            if (resetOffset)
            {
                _offset = Vector2.zero;
            }

            UpdateLocation();
        }

        /// <summary>
        /// 被写体を設定する
        /// </summary>
        public void SetTarget(ICameraTarget target, bool resetOffset = true)
        {
            _target = target;
            if (resetOffset)
            {
                _offset = Vector2.zero;
            }

            UpdateLocation();
        }

        /// <summary>
        /// パラメータを一括設定する
        /// </summary>
        public void SetParams(CameraLocationParam param)
        {
            if (param is null)
            {
                Debug.LogWarning("camera param must be not null.");
                return;
            }

            // パラメータ設定
            ApplyParams(param);
            UpdateLocation();
        }

        // 指定時間をかけてパラメータを変化させるメソッド
        public void SetParamsOverTime(CameraLocationParam param, float duration)
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }

            _target = null;
            _currentCoroutine = StartCoroutine(SetParamsCoroutine(param, duration));
        }

        public void SetDefaultParam()
        {
            var param = new CameraLocationParam(this._defaultTargetPos, 0f, 0f, DEFAULT_DISTANCE);
            SetParams(param);
        }

        /// <summary>
        /// パラメータに基づいてカメラ系のTransformを更新する
        /// </summary>
        public void UpdateLocation()
        {
            if (_parent == null || _camera == null)
            {
                if (Application.isPlaying)
                {
                    Debug.LogError("Update Camera Location: Required Transform references or Camera is null.");
                }
                return;
            }

            // [NOTE] 位置決めフロー
            //  1. parentの位置を"回転中心"と一致させる
            //  2. カメラの仰角、旋回角に基づいて、parentの角度を設定する
            //  3. カメラのズームに基づいて、cameraのlocalposition.zを設定する
            //  4. 回転中心からのオフセットに基づいて、cameraのlocalposition.xyを設定する

            // 未使用プロパティをリセット（規定値）
            _parent.localScale = Vector3.one;
            _camera.transform.localRotation = Quaternion.identity;
            _camera.transform.localScale = Vector3.one;

            // 基本パラメータ
            _parent.position = RotationCenter;
            _parent.eulerAngles = new Vector3(_tiltAngle, _headingAngle, 0f); // 回転

            // 追加パラメータ
            //_camera.fieldOfView = _parameter.fieldOfView;   // 視野
            _camera.transform.localPosition = new Vector3(_offset.x, _offset.y, -_distance); // ズーム + XYオフセット

            // 画面中心情報の更新
            _viewCenterRP.Value = Vector3Utils.FindClosestPointOnRay(transform.position, transform.forward, RotationCenter);
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        private void ApplyParams(CameraLocationParam param)
        {
            param.Validate();

            // パラメータ設定
            _tiltAngle = param.tiltAngle;
            _headingAngle = param.headingAngle;
            _distance = param.distance;
            _offset = param.offset;
            _defaultTargetPos = param.targetPos;
            _target = null;
        }


        private IEnumerator SetParamsCoroutine(CameraLocationParam param, float duration)
        {
            // 開始時点のカメラパラメータを取得
            Vector3 startCenter = _defaultTargetPos;
            float startTilt = _tiltAngle;
            float startHeading = _headingAngle;
            float startDistance = _distance;
            Vector2 startOffset = _offset;

            // 目標パラメータを取得
            Vector3 endCenter = param.targetPos;
            float endTilt = param.tiltAngle;
            float endHeading = param.headingAngle;
            float endDistance = param.distance;
            Vector3 endOffset = param.offset;

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                var t = elapsedTime / duration;

                // 時間経過に応じてパラメータを補間
                _defaultTargetPos = Vector3.Lerp(startCenter, endCenter, t);
                _tiltAngle = Mathf.Lerp(startTilt, endTilt, t);
                _headingAngle = Mathf.LerpAngle(startHeading, endHeading, t);
                _distance = Mathf.Lerp(startDistance, endDistance, t);
                _offset = Vector2.Lerp(startOffset, endOffset, t);

                // カメラの位置と回転を更新
                UpdateLocation();

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 最後に目標のパラメータを適用
            ApplyParams(param);
            UpdateLocation();

            _currentCoroutine = null;
        }



        /// ----------------------------------------------------------------------------
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_parent == null || _camera == null)
                return;

            float radius = 0.1f;

            // 回転中心, 画面中心
            var viewCetner = ViewCenterRP.Value;
            Gizmos.DrawWireSphere(RotationCenter, radius);
            Gizmos.DrawWireSphere(viewCetner, radius);

            Gizmos.DrawLine(transform.position, viewCetner);
            Gizmos.DrawLine(RotationCenter, viewCetner);
        }
#endif
    }
}


namespace Project.Utils
{

    public static partial class Vector3Utils
    {
        // 光線と点の最近傍点を取得する
        public static Vector3 FindClosestPointOnRay(Vector3 position, Vector3 direction, Vector3 targetPoint)
        {
            Vector3 fromRayToPoint = targetPoint - position;
            float projectionLength = Vector3.Dot(fromRayToPoint, direction);
            Vector3 closestPoint = position + direction * projectionLength;

            return closestPoint;
        }
    }


    public static partial class AngleUtils
    {
        /// <summary>
        /// 角度を-180から180の範囲に収める
        /// </summary>
        public static float NormalizeAngle(float angle)
        {
            angle = Mathf.Repeat(angle, 360f);  // 0から360の範囲に収める
            if (angle > 180f)
            {
                angle -= 360f;  // -180から0の範囲に収める
            }
            return angle;
        }
    }

    */
}