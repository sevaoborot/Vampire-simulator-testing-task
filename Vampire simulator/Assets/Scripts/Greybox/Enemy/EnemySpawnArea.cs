using UnityEngine;

public class EnemySpawnArea //: MonoBehaviour
{
    //eneemies should be spawned behind the camera borders
    private ViewportBounds _viewportBounds;
    private float _spawnOffset; //in world units

    private Rect _spawnBoundsRect;

    public EnemySpawnArea(ViewportBounds viewportBounds, float spawnOffset)
    {
        _viewportBounds = viewportBounds;
        _spawnOffset = spawnOffset;
        CountSpawnArea();
    }

    public void EnemySpawnAreaUpdate()
    {
        _viewportBounds.UpdateViewportBoundsPosition();
        _spawnBoundsRect.position = new Vector2
            (_viewportBounds.viewportRect.xMin - _spawnOffset, _viewportBounds.viewportRect.yMin - _spawnOffset);

        GetRandomPositionToSpawn();

#if UNITY_EDITOR
        DrawDebugRects();
#endif
    }

#if UNITY_EDITOR

    private void DrawDebugRects()
    {
        DrawRect(_viewportBounds.viewportRect, Color.green);
        DrawRect(_spawnBoundsRect, Color.red);
    }

    private void DrawRect(Rect rect, Color color)
    {
        Vector3 bl = new Vector3(rect.xMin, rect.yMin, 0f);
        Vector3 br = new Vector3(rect.xMax, rect.yMin, 0f);
        Vector3 tr = new Vector3(rect.xMax, rect.yMax, 0f);
        Vector3 tl = new Vector3(rect.xMin, rect.yMax, 0f);

        Debug.DrawLine(bl, br, color);
        Debug.DrawLine(br, tr, color);
        Debug.DrawLine(tr, tl, color);
        Debug.DrawLine(tl, bl, color);
    }

#endif

    private void CountSpawnArea()
    {
        _spawnBoundsRect = new Rect(
            _viewportBounds.viewportRect.xMin - _spawnOffset,
            _viewportBounds.viewportRect.yMin - _spawnOffset,
            _viewportBounds.viewportRect.width + _spawnOffset + _spawnOffset,
            _viewportBounds.viewportRect.height + _spawnOffset + _spawnOffset
            );
    }

    public Vector2 GetRandomPositionToSpawn()
    {
        int side = Random.Range( 0, 4);
        float x = 0f, y = 0f;

        switch(side)
        {
            case 0:
                x = Random.Range(_spawnBoundsRect.xMin, _spawnBoundsRect.xMax);
                y = Random.Range(_viewportBounds.viewportRect.yMax, _spawnBoundsRect.yMax);
                break;
            case 1:
                x = Random.Range(_viewportBounds.viewportRect.xMax, _spawnBoundsRect.xMax);
                y = Random.Range(_spawnBoundsRect.yMin, _spawnBoundsRect.yMax);
                break;
            case 2:
                x = Random.Range(_spawnBoundsRect.xMin, _spawnBoundsRect.xMax);
                y = Random.Range(_spawnBoundsRect.yMin, _viewportBounds.viewportRect.yMin);
                break;
            case 3:
                x = Random.Range(_spawnBoundsRect.xMin, _viewportBounds.viewportRect.xMin);
                y = Random.Range(_spawnBoundsRect.yMin, _spawnBoundsRect.yMax);
                break;
            default:
                Debug.LogError("Unexpected value in Enemy Spawner! Return a north side for spawning...");
                x = Random.Range(_spawnBoundsRect.xMin, _spawnBoundsRect.xMax);
                y = Random.Range(_viewportBounds.viewportRect.yMax, _spawnBoundsRect.yMax);
                break;
        }

        return new Vector2(x, y);
    }
}
