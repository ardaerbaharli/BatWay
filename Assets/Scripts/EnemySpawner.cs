using System.Collections.Generic;
using UnityEngine;
using static BallController;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SpriteRenderer hoopPrefab;
    [SerializeField] private BallController _ballControllerPrefab;
    [SerializeField] private float spawnY;
    [SerializeField] private float _bound;
    [SerializeField] private int numberOfEnemiesAtStart = 5;

    private List<GameObject> activeEnemies;
    private int enemyIndex = -1;
    private float distanceBetweenEnemies;
    private float lastSpawnedEnemyY;

    private void Awake()
    {
        var width = Helpers.Camera.orthographicSize / 2;

        var hoopSpriteBounds = hoopPrefab.bounds;
        var ballControllerSpriteBounds = _ballControllerPrefab.GetComponent<SpriteRenderer>().bounds;


        _bound = width - hoopSpriteBounds.size.x / 2 - _ballControllerPrefab.maxDistance -
                 ballControllerSpriteBounds.size.x / 2;
        var hoopHeight = hoopSpriteBounds.size.x;
        distanceBetweenEnemies = hoopHeight * 3 + _ballControllerPrefab.maxDistance;

        spawnY = -5 + distanceBetweenEnemies;
        lastSpawnedEnemyY = spawnY;
    }

    private void Start()
    {
        GameManager.instance.onGameStarted += SpawnStartEnemies;
        GameManager.instance.onScore += SpawnEnemy;
    }

    private void SpawnStartEnemies()
    {
        SpawnEnemyByPiece(numberOfEnemiesAtStart);
    }

    private void SpawnEnemy(int score)
    {
        SpawnEnemyByPiece(1);
    }

    private void SpawnEnemyByPiece(int n)
    {
        for (var i = 0; i < n; i++)
        {
            enemyIndex++;
            var safeZone = ObjectPool.instance.GetPooledObject("SafeZone");

            var isEvenEnemyIndex = enemyIndex % 2 == 0;
            var pos = isEvenEnemyIndex ? GetRandomPosition("Left") : GetRandomPosition("Right");
            safeZone.transform.position = pos;

            var hoop = ObjectPool.instance.GetPooledObject("Hoop");
            hoop.transform.SetParent(safeZone.transform);
            hoop.transform.localPosition = Vector3.zero;

            safeZone.gameObject.SetActive(true);
            hoop.gameObject.SetActive(true);

            var ballCount = GetBallCount();
            var balls = new List<BallController>();

            float speed = 0, distance = 0;
            RotationDirection rotDirection = RotationDirection.Clockwise;
            for (var j = 0; j < ballCount; j++)
            {
                var isEvenBallIndex = j % 2 == 0;
                var ball = ObjectPool.instance.GetPooledObject("Ball");
                var bc = ball.gameObject.GetComponent<BallController>();
                bc.SetPivot(safeZone.transform);

                // change the speed every 2 balls
                if (j % 2 == 0)
                {
                    speed = GetRandomSpeed(bc);
                    distance = GetRandomDistance(bc);
                    rotDirection = GetRandomRotationDirection();
                }

                bc.slideOutAmount = distanceBetweenEnemies * 2 * (isEvenBallIndex ? -1 : 1);
                bc.speed = speed;
                bc.distance = distance;
                bc.rotationDirection = rotDirection;
                bc.SetPosition(isEvenBallIndex ? -1 : 1);


                // speed = Random.Range(minSpeed, maxSpeed);


                // bc.SetProperties();

                balls.Add(bc);
                ball.gameObject.SetActive(true);
            }

            var szf = safeZone.gameObject.GetComponent<SafeZoneController>();
            szf.SetBalls(balls);
            szf.hoop = hoop.gameObject;
            szf.slideOutAmount = distanceBetweenEnemies * 2 * (isEvenEnemyIndex ? -1 : 1);
            szf.slideDownAmount = -distanceBetweenEnemies;
            GameManager.instance.SpawnEnemy(safeZone.transform);
        }
    }

    private RotationDirection GetRandomRotationDirection()
    {
        return (RotationDirection) Random.Range(0, 2);
    }

    private float GetRandomDistance(BallController bc)
    {
        return Random.Range(bc.minDistance, bc.maxDistance);
    }

    private float GetRandomSpeed(BallController bc)
    {
        return Random.Range(bc.minSpeed, bc.maxSpeed);
    }

    private void OnDestroy()
    {
        GameManager.instance.onGameStarted -= SpawnStartEnemies;
        GameManager.instance.onScore -= SpawnEnemy;
    }

    private int GetBallCount()
    {
        return Random.Range(1, 3) * 2;
    }


    private Vector3 GetRandomPosition(string leanTo)
    {
        spawnY = lastSpawnedEnemyY;
        if (numberOfEnemiesAtStart > enemyIndex) spawnY += distanceBetweenEnemies;
        lastSpawnedEnemyY = spawnY;
        return leanTo == "Left"
            ? new Vector3(Random.Range(-_bound, 0), spawnY, 0)
            : new Vector3(Random.Range(0, _bound), spawnY, 0);
    }
}