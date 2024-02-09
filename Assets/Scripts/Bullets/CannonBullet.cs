using DG.Tweening;
using UnityEngine;
using Utils;

namespace Bullets
{
    public class CannonBullet : BulletBase
    {
        public bool isIceCannon;
        public override void Init(Transform target, IDamageable damageable)
        {
            base.Init(target, damageable);
            // transform.DOJump(target.position + Vector3.up * 0.5f, 0.3f, 1, bullet.speed).SetEase(Ease.Linear)
            // .OnComplete(() => OnReached(target));
            movingObj.transform.DOMove(target.position /* + Vector3.up * 0.5f */, bullet.speed).SetSpeedBased().SetEase(Ease.OutSine)
             .OnComplete(() => OnReached(target, damageable));

            //TODOtower: burada eğer ice cannon ise freeze yap enemy toweri
        }
    }
}