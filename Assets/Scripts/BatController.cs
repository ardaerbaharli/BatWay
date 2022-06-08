using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] public GameObject holder;

    [SerializeField] private List<Transform> enemyTransforms;
    [SerializeField] private float moveTime;
    [SerializeField] private AudioSource batMovingSound;


    private int _currentEnemyIndex = -1;
    public bool isMoving;

    private void Start()
    {
        _inputHandler.onPointerDown_ += OnPointerDown;
        GameManager.instance.onNewEnemySpawned += OnNewEnemySpawned;
    }


    private void OnNewEnemySpawned(Transform p)
    {
        enemyTransforms.Add(p);
    }


    private void OnPointerDown()
    {
        if (!GameManager.instance.isGameStarted) return;
        if (GameManager.instance.areSafeZonesMoving) return;
        if (isMoving) return;
        holder.transform.DetachChildren();
        var szf = holder.GetComponent<SafeZoneController>();
        if (szf != null)
            szf.isAlive = false;

        holder.SetActive(false);
        MoveToNextPosition();
    }

    private void MoveToNextPosition()
    {
        isMoving = true;
        _currentEnemyIndex++;
        var enemyPos = enemyTransforms[_currentEnemyIndex].position;
        var diff = enemyPos - transform.position;
        diff.Normalize();

        var rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        batMovingSound.Play();
        iTween.MoveTo(gameObject, iTween.Hash(
            "position", enemyPos,
            "time", moveTime,
            "oncomplete", "MovingComplete"));
    }

    private void MovingComplete()
    {
        isMoving = false;
        var enemyPos = enemyTransforms[_currentEnemyIndex + 1].position;
        var diff = enemyPos - transform.position;
        diff.Normalize();

        var rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}