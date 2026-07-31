using UnityEngine;

public class ViewportBounds
{
    private Camera _camera;

    public Rect viewportRect { get; private set; }

    public ViewportBounds(Camera camera)
    {
        _camera = camera;
        CountViewportBoundsRect();
    }

    public void UpdateViewportBoundsPosition()
    {
        viewportRect = new Rect(
            _camera.transform.position.x - viewportRect.width / 2,
            _camera.transform.position.y - viewportRect.height / 2,
            viewportRect.width,
            viewportRect.height
        );
    }

    private void CountViewportBoundsRect()
    {
        Vector3 viewportBottomLeftWorldCoordinate = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
        Vector3 viewportTopRightWorldCoordinate = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));

        viewportRect = new Rect(
            viewportBottomLeftWorldCoordinate.x,
            viewportBottomLeftWorldCoordinate.y,
            viewportTopRightWorldCoordinate.x - viewportBottomLeftWorldCoordinate.x,
            viewportTopRightWorldCoordinate.y - viewportBottomLeftWorldCoordinate.y
            );
    }
}
