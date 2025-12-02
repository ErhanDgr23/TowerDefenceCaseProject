using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public GunSO CurrentGun;

    [SerializeField] CustomAnimator MuzzleFlash;
    [SerializeField] GameObject BulletPre;
    [SerializeField] EnemySensor Sensor;
    [SerializeField] Transform GunPivot, BulletSpawnPoint;

    CameraFollowFixedRotation _cameraFollow;
    SpriteRenderer _GunSprite;
    CustomAnimator _animator;
    Transform _enemyTarget;
    float _zmn;

    private void Start()
    {
        _GunSprite = GunPivot.GetChild(0).GetComponent<SpriteRenderer>();
        _animator = GetComponent<CustomAnimator>();
        GunPivot.GetChild(0).gameObject.SetActive(false);
        _cameraFollow = CameraFollowFixedRotation.instance;

        Sensor.EnemyReach += CalculateClosestEnemyTarget;
    }

    void CalculateClosestEnemyTarget(List<GameObject> enemyList)
    {
        print("Calculating");

        if (enemyList == null || enemyList.Count == 0)
        {
            _enemyTarget = null;
            return;
        }

        float closestDist = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemyList)
        {
            float currentDist = Vector2.Distance(enemy.transform.position, transform.position);

            if (currentDist < closestDist)
            {
                closestDist = currentDist;
                closestEnemy = enemy.transform;
            }
        }

        _enemyTarget = closestEnemy;
    }

    private void LateUpdate()
    {
        if (_enemyTarget == null)
            GunPivot.eulerAngles = new Vector3(GunPivot.localEulerAngles.x, _animator.flipX == false ? 0f : 180f, 0f);
    }

    public void LookEnemy()
    {
        if (_enemyTarget == null || CurrentGun == null) return;

        GunPivot.GetChild(0).gameObject.SetActive(_enemyTarget);
        EnemyManager enemyManger = _enemyTarget.GetComponent<EnemyManager>();

        if (enemyManger.IsDead)
        {
            _enemyTarget = null;
            Sensor.FireEvent();
            return;
        }

        if (_zmn <= CurrentGun.FireCooldown)
            _zmn += Time.deltaTime;

        Vector3 dir = _enemyTarget.position - GunPivot.parent.position;
        Vector3 localDir = GunPivot.parent.InverseTransformDirection(dir);
        float angle = Mathf.Atan2(localDir.y, localDir.x) * Mathf.Rad2Deg;
        GunPivot.localEulerAngles = new Vector3(0f, 0f, angle);
        _cameraFollow.OffsetZChange(dir.normalized.z * 2.5f);

        if (angle >= -90f && angle <= 90f)
            _GunSprite.transform.localScale = new Vector2(1f, 1f);
        else if (angle > 90f || angle < -90f)
            _GunSprite.transform.localScale = new Vector2(1f, -1f);

        if (_zmn > CurrentGun.FireCooldown)
        {
            ShootGun();
            _zmn = 0f;
        }
    }

    void ShootGun()
    {
        if (_enemyTarget == null) return;

        MuzzleFlash.StopAndPlay("Shoot");
        GameObject bullet = Instantiate(BulletPre, BulletSpawnPoint.transform.position, BulletSpawnPoint.transform.rotation);
        CustomAnimator bulletAnim = bullet.GetComponent<CustomAnimator>();
        Transform target = _enemyTarget;
        bulletAnim.StopAndPlay("Fire");
        bullet.transform.DOMove(target.position, 0.25f)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                if (target != null)
                    DamageEnemy(target.GetComponent<EnemyManager>(), bullet);
            });
    }

    void DamageEnemy(EnemyManager Enemy, GameObject Bulletobj)
    {
        if(Enemy.TryGetComponent<IEnemy>(out IEnemy EnemyDmgFunc))
        {
            EnemyDmgFunc.Damage((int)CurrentGun.Damage);
        }

        Destroy(Bulletobj);
    }
}
