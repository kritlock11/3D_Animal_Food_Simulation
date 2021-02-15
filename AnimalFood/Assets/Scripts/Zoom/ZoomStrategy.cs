using UnityEngine;

namespace Assets.Scripts.Zoom
{
    public interface IZoomStrategy
    {
        void ZoomIn(Camera cam, float delta, float min);
        void ZoomOut(Camera cam, float delta, float max);
    }

    public class OrthographicZoomStrategy : IZoomStrategy
    {
        public OrthographicZoomStrategy(Camera cam, float startZoom)
        {
            cam.orthographicSize = startZoom;
        }

        public void ZoomIn(Camera cam, float delta, float min)
        {
            if (cam.orthographicSize <= min) return;
            cam.orthographicSize = Mathf.Max(cam.orthographicSize - delta, min);
        }

        public void ZoomOut(Camera cam, float delta, float max)
        {
            if (cam.orthographicSize >= max) return;
            cam.orthographicSize = Mathf.Min(cam.orthographicSize + delta, max);
        }
    }

    public class PerspectiveZoomStrategy : IZoomStrategy
    {
        private Vector3 _normCamPos;
        private float _zoomLevel;

        public PerspectiveZoomStrategy(Camera cam, Vector3 offset , float startZoom)
        {
            _normCamPos = new Vector3(0f, Mathf.Abs(offset.y), -Mathf.Abs(offset.x)).normalized;
            _zoomLevel = startZoom;
            PositionCamera(cam);
        }

        private void PositionCamera(Camera cam)
        {
            cam.transform.localPosition = _normCamPos * _zoomLevel;
        }

        public void ZoomIn(Camera cam, float delta, float min)
        {
            if (_zoomLevel<=min) return;
            _zoomLevel = Mathf.Max(_zoomLevel - delta, min);
            PositionCamera(cam);
        }


        public void ZoomOut(Camera cam, float delta, float max)
        {
            if (_zoomLevel >= max) return;
            _zoomLevel = Mathf.Min(_zoomLevel + delta, max);
            PositionCamera(cam);
        }
    }
}
