using UnityEngine;

public class EnemySpawnArea //: MonoBehaviour
{
    //eneemies should be spawned behind the camera borders
    private Camera _camera;
    private float _spawnOffset; //in world units

    private Rect _viewportBounds;
    private Rect _spawnBounds;

    public EnemySpawnArea(Camera camera, float spawnOffset)
    {
        _camera = camera;
        _spawnOffset = spawnOffset;
        CountSpawnArea();
    }

    public void EnemySpawnAreaUpdate()
    {
        _spawnBounds.position = new Vector2
            (_camera.transform.position.x - _spawnBounds.width/2, _camera.transform.position.y - _spawnBounds.height/2);
        _viewportBounds.position = new Vector2
            (_camera.transform.position.x - _viewportBounds.width/2, _camera.transform.position.y - _viewportBounds.height/2);
        GetRandomPositionToSpawn();

#if UNITY_EDITOR
        DrawDebugRects();
#endif
    }

#if UNITY_EDITOR

    private void DrawDebugRects()
    {
        DrawRect(_viewportBounds, Color.green);
        DrawRect(_spawnBounds, Color.red);
    }

    private void DrawRect(Rect rect, Color color)
    {
        Vector3 bl = new Vector3(rect.xMin, rect.yMin, _camera.nearClipPlane);
        Vector3 br = new Vector3(rect.xMax, rect.yMin, _camera.nearClipPlane);
        Vector3 tr = new Vector3(rect.xMax, rect.yMax, _camera.nearClipPlane);
        Vector3 tl = new Vector3(rect.xMin, rect.yMax, _camera.nearClipPlane);

        Debug.DrawLine(bl, br, color);
        Debug.DrawLine(br, tr, color);
        Debug.DrawLine(tr, tl, color);
        Debug.DrawLine(tl, bl, color);
    }

#endif

    private void CountSpawnArea()
    {
        Vector3 viewportBottomLeftWorldCoordinate = _camera.ViewportToWorldPoint(new Vector3(0,0, _camera.nearClipPlane));
        Vector3 viewportTopRightWorldCoordinate = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));

        _viewportBounds = new Rect(
            viewportBottomLeftWorldCoordinate.x,
            viewportBottomLeftWorldCoordinate.y,
            viewportTopRightWorldCoordinate.x - viewportBottomLeftWorldCoordinate.x,
            viewportTopRightWorldCoordinate.y - viewportBottomLeftWorldCoordinate.y
            );

        _spawnBounds = new Rect(
            _viewportBounds.xMin - _spawnOffset,
            _viewportBounds.yMin - _spawnOffset,
            _viewportBounds.width + _spawnOffset + _spawnOffset,
            _viewportBounds.height + _spawnOffset + _spawnOffset    
            );
    }

    public Vector2 GetRandomPositionToSpawn()
    {
        int side = Random.Range( 0, 4);
        float x = 0f, y = 0f;

        switch(side)
        {
            case 0:
                x = Random.Range(_spawnBounds.xMin, _spawnBounds.xMax);
                y = Random.Range(_viewportBounds.yMax, _spawnBounds.yMax);
                break;
            case 1:
                x = Random.Range(_viewportBounds.xMax, _spawnBounds.xMax);
                y = Random.Range(_spawnBounds.yMin, _spawnBounds.yMax);
                break;
            case 2:
                x = Random.Range(_spawnBounds.xMin, _spawnBounds.xMax);
                y = Random.Range(_spawnBounds.yMin, _viewportBounds.yMin);
                break;
            case 3:
                x = Random.Range(_spawnBounds.xMin, _viewportBounds.xMin);
                y = Random.Range(_spawnBounds.yMin, _spawnBounds.yMax);
                break;
            default:
                Debug.LogError("Unexpected value in Enemy Spawner! Return a north side for spawning...");
                x = Random.Range(_spawnBounds.xMin, _spawnBounds.xMax);
                y = Random.Range(_viewportBounds.yMax, _spawnBounds.yMax);
                break;
        }

        return new Vector2(x, y);
    }
}
