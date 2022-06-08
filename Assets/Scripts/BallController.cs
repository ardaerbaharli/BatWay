using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour
{
    public enum RotationDirection
    {
        Clockwise,
        CounterClockwise
    }

    [SerializeField] public RotationDirection rotationDirection;
    [SerializeField] public float speed;
    [SerializeField] private float selfRotationSpeed;

    [SerializeField] public float distance;
    [SerializeField] private float slideOutTime = 1f;
    [SerializeField] private Collider2D col;

    public float slideOutAmount;
    private Transform _pivot;
    private bool _pivotSet;
    [SerializeField] public float minDistance, maxDistance;
    [SerializeField] public float minSpeed, maxSpeed;

    [SerializeField] private AudioSource ballHitSound;

    private void Start()
    {
        GameManager.instance.onScore += OnScore;
    }

    private void OnScore(int score)
    {
        minSpeed += score;
        maxSpeed += score;
    }

    private void Update()
    {
        if (!_pivotSet) return;
        var pivPos = _pivot.position;
        var axis = rotationDirection == RotationDirection.Clockwise ? Vector3.forward : Vector3.back;
        transform.RotateAround(pivPos, axis, speed * Time.deltaTime);

        // rotate the ball around z axis constantly
        transform.Rotate(axis, selfRotationSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bat"))
        {
            print("DED");
            ballHitSound.Play();
            GameManager.instance.GameOver();
            col.gameObject.SetActive(false);
        }
    }

    public void SetPivot(Transform t)
    {
        _pivot = t;
        _pivotSet = true;
    }


    public void SlideOut()
    {
        slideOutTime = Random.Range(0.3f, 1f);
        _pivotSet = false;
        col.isTrigger = false;

        var targetPosition = new Vector3(transform.position.x + slideOutAmount, transform.position.y, 0);
        iTween.MoveTo(gameObject,
            iTween.Hash("position", targetPosition, "time", slideOutTime, "easetype",
                iTween.EaseType.easeInOutSine, "oncomplete", "SelfDestroy"));
    }

    private void SelfDestroy()
    {
        gameObject.SetActive(false);
    }

    public void SetPosition(int distanceMultipler)
    {
        var pivAnchPos = _pivot.position;
        transform.position = new Vector2(pivAnchPos.x + distance * distanceMultipler, pivAnchPos.y);
    }
}