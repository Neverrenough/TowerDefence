using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class NextLevelGUI : MonoBehaviour
    {
        [SerializeField] private Text bonusAmount;
        private EnemyWaveManager manager;
        private float timeToNextWave;
        private void Start()
        {
            manager = FindObjectOfType<EnemyWaveManager>();
            EnemyWave.onWavePrepare += (float time) =>
            {
                 timeToNextWave = time;
            };
        }
        public void CallWave()
        {
            manager.ForceNextWave();
        }
        private void Update()
        {
            var bonus = (int)timeToNextWave;
            if (bonus < 0) bonus = 0;
            bonusAmount.text = bonus.ToString();
            timeToNextWave -= Time.deltaTime;
        }
    }
}
