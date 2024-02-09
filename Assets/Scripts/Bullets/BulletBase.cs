using System;
using Data_and_Scriptable.BulletSo;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.iOS;
using UnityEngine;
using Utils;
using Utils.PoolSystem;

namespace Bullets
{
    [Serializable]
    public class Projectile
    {
        public GameObject spawnEffect, movingObj, explodeEffect;
    }
    public class BulletBase : MonoBehaviour
    {
        [SerializeField] public BulletSo bullet;
        [SerializeField] private Transform skin;
        [SerializeField] private Projectile mySkin;
        public GameObject movingObj, tempSpawnEffect, tempExplodeEffect;
        public virtual void Init(Transform target, IDamageable damageable)
        {
            tempSpawnEffect = mySkin.spawnEffect.Spawn(transform.position, quaternion.identity);
            movingObj = mySkin.movingObj.Spawn(transform.position, quaternion.identity, transform);
            movingObj.transform.LookAt(target);
            // transform.LookAt(target);

            // for (int i = 0; i < skin.childCount; i++)
            // {
            //     skin.GetChild(i).gameObject.SetActive(i == (int)bullet.bulletType);
            // }
        }

        public void OnReached(Transform other, IDamageable damageable)
        {
            tempExplodeEffect = mySkin.explodeEffect.Spawn(other.position, quaternion.identity);

            // if (other.GetChild(0).TryGetComponent(out IDamageable damageable))
            // {
            damageable.Damage(bullet.damage);
            // }

            movingObj.transform.parent = null;
            movingObj.Despawn();
            OnHit(other);
        }

        private void OnHit(Transform other)
        {
            tempExplodeEffect.Despawn(1);
            tempSpawnEffect.Despawn();
            movingObj.transform.parent = null;
            // mySkin.explodeEffect.Spawn(other.position, quaternion.identity);
            gameObject.Despawn();
            // bullet.effect.Spawn(transform.position, Quaternion.identity).Despawn(bullet.despawnTime);
        }
    }
}