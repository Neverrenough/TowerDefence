using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    public class EnemyWave : MonoBehaviour
    {
        public static Action<float> onWavePrepare;
        [Serializable]
        private class Squad
        {
            public EnemyAsset asset;
            public int count;
        }
        [Serializable]
        private class PathGroup
        {
            public Squad[] squads;
        }
        [SerializeField] private PathGroup[] groups;

        [SerializeField] private float prepareTime = 10f;
        public float GetRemainingTime() { return prepareTime - Time.time; }
        private void Awake()
        {
            enabled = false;
        }
        private event Action OnWaveReady;
        public IEnumerable<(EnemyAsset asset, int count, int pathIndex)> EnumerateSquads()
        {
            for(int i = 0; i < groups.Length; i++)
            {
                foreach(var squad in groups[i].squads)
                {
                    yield return (squad.asset, squad.count, i);
                }
            }
        }

        internal void Prepare(Action spawnEnemies)
        {
            onWavePrepare?.Invoke(prepareTime);
            prepareTime += Time.time;
            enabled = true;
            OnWaveReady += spawnEnemies;
        }
        private void Update()
        {
            if(Time.time >= prepareTime)
            {
                enabled = false;
                OnWaveReady?.Invoke();
            }
        }
        [SerializeField] private EnemyWave next;
        internal EnemyWave PrepareNext(Action spawnEnemies)
        {
            TDPlayer.Instance.ChangeMana(10);
            OnWaveReady -= spawnEnemies;
            if(next) next.Prepare(spawnEnemies);
            return next;
        }
    }
}