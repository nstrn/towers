using UnityEngine;

namespace Data_and_Scriptable.GunSo
{
    [CreateAssetMenu(menuName = "Gun")]
    public class GunSo : ScriptableObject
    {
        public BulletSo.BulletSo myBullet;
        public float frequency;
        public GameObject muzzleEffect;

    }
}