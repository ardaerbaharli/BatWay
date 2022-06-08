using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneController : MonoBehaviour
{
    [SerializeField] private List<BallController> balls;
    [SerializeField] public GameObject hoop;
    [SerializeField] private Collider2D collider;

    [SerializeField] private float slideOutTime = 1f;
    [SerializeField] private float slideDownTime = 1f;
    public float slideOutAmount;
    public float slideDownAmount;
    private bool isHolder;
    public bool isAlive;

    private void Start()
    {
        GameManager.instance.onScore += OnScore;
        GameManager.instance.onPause += OnPause;
        GameManager.instance.onResume += OnResume;
        isAlive = true;
    }

    private void OnResume()
    {
        if (isAlive)
            gameObject.SetActive(true);
    }

    private void OnPause()
    {
        if (isAlive)
            gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.instance.onScore -= OnScore;
        GameManager.instance.onPause -= OnPause;
        GameManager.instance.onResume -= OnResume;
    }

    private void OnScore(int score)
    {
        GameManager.instance.areSafeZonesMoving = true;
        if (isHolder)
            bat.transform.SetParent(transform);

        var targetPosition = new Vector3(transform.position.x, transform.position.y + slideDownAmount,
            transform.position.z);
        iTween.MoveTo(gameObject,
            iTween.Hash("position", targetPosition, "time", slideDownTime, "easetype",
                iTween.EaseType.easeInOutSine, "oncomplete", "SlideDownComplete"));
    }


    private void SlideDownComplete()
    {
        GameManager.instance.areSafeZonesMoving = false;
        if (isHolder)
        {
            bat.GetComponent<BatController>().holder = gameObject;
            transform.DetachChildren();
        }
    }

    public void SetBalls(List<BallController> b)
    {
        balls = b;
        balls.ForEach(x => x.transform.SetParent(transform));
    }

    private Collider2D bat;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bat"))
        {
            bat = col;
            isHolder = true;
            collider.isTrigger = false;
            StartCoroutine(WaitForBatToStop());
        }
    }

    private IEnumerator WaitForBatToStop()
    {
        yield return new WaitUntil(() => bat.GetComponent<BatController>().isMoving == false);

        GameManager.instance.Score();

        hoop.SetActive(false);
        balls.ForEach(x => x.SlideOut());
    }
}