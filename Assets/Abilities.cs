using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDefence
{
    public class Abilities : MonoSingleton<Abilities>
    {
        [SerializeField] private Button TimeButton;
        [SerializeField] private Button FireButton;
        [SerializeField] private Image TargetingCircles;
        [SerializeField] private FireAbility m_FireAbility;
        public void UseFireAbility() => m_FireAbility.Use();
        [SerializeField] private TimeAbility m_TimeAbility;
        public void UseTimeAbility() => m_TimeAbility.Use();
        [Serializable]
        public class FireAbility
        {
            [SerializeField] protected TDPlayer tdPlayer;
            [SerializeField] public int cost = 10;
            [SerializeField] private int damage = 2;
            [SerializeField] private Color TargetingColor;
            public void Use()
            {
                tdPlayer.ChangeMana(-cost);
                ClickProtection.Instance.Activate((Vector2 v) =>
                {
                    
                    Vector3 position = v;
                    position.z = Camera.main.transform.position.z;
                    position = Camera.main.ScreenToWorldPoint(position);
                    foreach(var collider in Physics2D.OverlapCircleAll(position, 5))
                    {
                        if(collider.transform.parent.TryGetComponent<Enemy>(out var enemy))
                        {
                            enemy.TakeDamage(damage, TDProjectile.DamageType.Magic);
                        }
                    }

                });
                IEnumerator FireAbilityButton()
                {
                    Instance.FireButton.interactable = false;
                    yield return new WaitUntil(() => cost <= tdPlayer.Mana);
                    Instance.FireButton.interactable = true;
                }
                Instance.StartCoroutine(FireAbilityButton());
            }
        }
        [Serializable]
        public class TimeAbility
        {
            [SerializeField] private TDPlayer tdPlayer;
            [SerializeField] public int cost = 20;
            [SerializeField] private float cooldown = 30f;
            [SerializeField] private float duration = 5f;
            public void Use()
            {
                tdPlayer.ChangeMana(-cost);
                if (tdPlayer.Mana < cost)
                {
                    Instance.TimeButton.interactable = false;
                }
                void Slow(Enemy ship)
                {
                    ship.GetComponent<SpaceShip>().HalfMaxLinearVelocity();
                }
                
                EnemyWaveManager.OnEnemySpawn +=Slow;
                IEnumerator Restore()
                {
                    yield return new WaitForSeconds(duration);
                    foreach (var ship in FindObjectsOfType<SpaceShip>())
                    {
                        ship.RestoreMaxLinearVelocity();
                        Debug.Log(ship + "Restored");
                    }
                        
                    EnemyWaveManager.OnEnemySpawn -= Slow;
                }
                Instance.StartCoroutine(Restore());

                IEnumerator TimeAbilityButton()
                {
                    Instance.TimeButton.interactable = false;
                    yield return new WaitForSeconds(cooldown);
                    yield return new WaitUntil(() => cost <= tdPlayer.Mana);
                    Instance.TimeButton.interactable = true;
                }
                Instance.StartCoroutine(TimeAbilityButton());
            }
        }

    }
}
