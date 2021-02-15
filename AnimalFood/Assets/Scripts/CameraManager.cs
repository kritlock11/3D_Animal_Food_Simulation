using Assets.Scripts.Zoom;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraManager : MonoBehaviour
    {
        public Vector2 Offset = new Vector2(0f, 14f);
        public float LookAt = 2f;

        public float InOutSpeed = 10f;
        public float LateralSpeed = 30f;
        public float RotateSpeed = 135f;

        public Vector2 MinBounds, MaxBounds;


        public float ZoodSpeed = 16f;
        public float Min = 1;
        public float Max = 7f;
        public float StartingZoom = 7f;

        private IZoomStrategy zoomStrategy;
        private Vector3 FrameMove;
        private float FrameRotate;
        private float FrameZoom;

        private Camera cam;
        private Transform _dummy;


        private void Awake()
        {
            cam = GetComponentInChildren<Camera>();
            cam.transform.localPosition = new Vector3(0f, Mathf.Abs(Offset.y), -Mathf.Abs(Offset.x));
            cam.transform.LookAt(transform.position + Vector3.up * LookAt);
        }


        private void OnEnable()
        {
            InputManager.OnMove += OnMoveCallBack;
            InputManager.OnRotate += OnRotateCallBack;
            InputManager.OnZoom += OnZoomCallBack;
        }
        private void OnDisable()
        {
            InputManager.OnMove -= OnMoveCallBack;
            InputManager.OnRotate -= OnRotateCallBack;
            InputManager.OnZoom -= OnZoomCallBack;
        }


        public void CreateDummy(StartGameData n)
        {
            var XZ = n.N - 1;
            _dummy = new GameObject("0.0").transform;
            _dummy.position = new Vector3(XZ, 0, -XZ);
            _dummy.rotation = Quaternion.Euler(0, 0, 0);

            SetValues(n);
        }

        private void SetValues(StartGameData n)
        {
            transform.position = _dummy.position;
            var nN = n.N;

            StartingZoom = Max = nN;

            zoomStrategy = cam.orthographic ?
                (IZoomStrategy)new OrthographicZoomStrategy(cam, StartingZoom)
                : new PerspectiveZoomStrategy(cam, Offset, StartingZoom);

            MinBounds = new Vector2(-1, -(nN * 2 - 1));
            MaxBounds = new Vector2(nN * 2 - 1, 1);
        }

        private void OnMoveCallBack(Vector3 v) => FrameMove += v;
        private void OnRotateCallBack(float f) => FrameRotate += f;
        private void OnZoomCallBack(float f) => FrameZoom += f;

        private void LateUpdate()
        {

            var dt = Time.deltaTime;
            if (FrameMove != Vector3.zero)
            {
                var speedFrameMove = new Vector3(FrameMove.x * LateralSpeed, FrameMove.y, FrameMove.z * InOutSpeed);

                transform.position += transform.TransformDirection(speedFrameMove) * dt;
                LockPosInBounds();
                FrameMove = Vector3.zero;
            }

            if (FrameRotate != 0f)
            {
                transform.Rotate(Vector3.up, FrameRotate * RotateSpeed * dt);
                FrameRotate = 0f;
            }

            if (FrameZoom < 0f)
            {
                zoomStrategy.ZoomIn(cam, Mathf.Abs(FrameZoom) * ZoodSpeed * dt, Min);
                FrameZoom = 0;
            }
            else if (FrameZoom > 0f)
            {
                zoomStrategy.ZoomOut(cam, FrameZoom * ZoodSpeed * dt, Max);
                FrameZoom = 0;
            }
        }

        private void LockPosInBounds()
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, MinBounds.x, MaxBounds.x),
                transform.position.y,
                Mathf.Clamp(transform.position.z, MinBounds.y, MaxBounds.y));
        }
    }
}
