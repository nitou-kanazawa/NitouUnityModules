using UnityEngine;

namespace nitou.Audio {

    [RequireComponent(typeof(AudioSource))]
    public class SequenceLineRenderer : MonoBehaviour, IAudioRender {

        [SerializeField] LineRenderer _lineRenderer;
        [SerializeField] float _waveLength = 20.0f;
        [SerializeField] float _yLength = 10f;

        private AudioSource _source = default;
        private float[] _data = default;
        private int _sampleStep = default;
        private Vector3[] _samplingLinePoints = default;


        /// ----------------------------------------------------------------------------
        // MonoBehaviour Method

        private void Start() {
            var source = GetComponent<AudioSource>();
            var clip = source.clip;
            var data = new float[clip.channels * clip.samples];
            source.clip.GetData(data, 0);

            Prepare(source, data);
        }

        private void FixedUpdate() {
            if (_source == null) return;

            if (_source.isPlaying && _source.timeSamples < _data.Length) {
                var startIndex = _source.timeSamples;
                var endIndex = _source.timeSamples + _sampleStep;
                Inflate(
                    _data, startIndex, endIndex,
                    _samplingLinePoints,
                    _waveLength, -_waveLength / 2f, _yLength
                );
                Render(_samplingLinePoints);
            } else {
                Reset();
            }

        }

        private void Reset() {
            var x = -_waveLength / 2;
            Render(new[]
            {
            new Vector3(-x, 0, 0) + this.transform.position,
            this.transform.position,
            new Vector3(x, 0, 0) + this.transform.position,
        });
        }


        /// ----------------------------------------------------------------------------
        // Public Method

        public void Prepare(AudioSource source, float[] data) {

            _source = source;
            _data = data;

            // 
            var fps = Mathf.Max(60f, 1f / Time.fixedDeltaTime);
            var clip = _source.clip;
            _sampleStep = (int)(clip.frequency / fps);
            _samplingLinePoints = new Vector3[_sampleStep];
        }

        public void Inflate(float[] target, int start, int end,
        Vector3[] result, float xLength, float xOffset, float yLength
        ) {
            var samples = Mathf.Max(end - start, 1f);
            var xStep = xLength / samples;
            var j = 0;

            for (var i = start; i < end; i++, j++) {
                var x = xOffset + xStep * j;
                var y = i < target.Length ? target[i] * yLength : 0f;
                var p = new Vector3(x, y, 0) + this.transform.position;
                result[j] = p;
            }
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        private void Render(Vector3[] points) {
            if (points == null) return;

            _lineRenderer.positionCount = points.Length;
            _lineRenderer.SetPositions(points);
        }

    }

}
