using UnityEngine;

namespace nitou.Audio{

    public class SpectramLineRenderer : MonoBehaviour, IAudioRender{

        [SerializeField] LineRenderer _lineRenderer;
        [SerializeField] float _waveLength = 20.0f;
        [SerializeField] float _yLength = 10f;

        private AudioSource _source = null;
        private float[] _spectram = null;
        private Vector3[] _points = null;
        private const int FFT_RESOLUTION = 2048;


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
            Render();
        }

        private void Reset() {
            var x = _waveLength / 2;
            Render(new[]
            {
            new Vector3(-x, 0, 0) + transform.position,
            new Vector3(0, 0, 0) + transform.position,
            new Vector3(x, 0, 0) + transform.position,
        });
        }


        /// ----------------------------------------------------------------------------
        // Public Method

        public void Prepare(AudioSource source, float[] data) {
            this._source = source;
            this._spectram = new float[FFT_RESOLUTION];
            this._points = new Vector3[FFT_RESOLUTION];
        }

        private void Render() {
            _source.GetSpectrumData(_spectram, 0, FFTWindow.BlackmanHarris);
            var xStart = -_waveLength / 2;
            var xStep = _waveLength / _spectram.Length;
            for (var i = 0; i < _points.Length; i++) {
                var y = _spectram[i] * _yLength;
                var x = xStart + xStep * i;
                var p = new Vector3(x, y, 0) + transform.position;
                _points[i] = p;
            }

            Render(_points);
        }

        private void Render(Vector3[] points) {
            if (points == null) return;
            _lineRenderer.positionCount = points.Length;
            _lineRenderer.SetPositions(points);
        }

    }
}
