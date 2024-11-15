using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FloxyDev.RadialMenu
{
    public class RadialMenuManager : MonoBehaviour
    {
        [SerializeField, Range(2, 10)] private int numberOfRadialPart;
        [SerializeField, Range(0, 100)] private float radialPartsGap;
        [SerializeField] private GameObject radialPartPrefab;
        [SerializeField] private Transform radialPartsCanvas;
        
        private readonly List<GameObject> _spawnedRadialParts = new List<GameObject>();

        private void Update()
        {
            SpawnRadialPart();
        }
        
        private void SpawnRadialPart()
        {
            foreach (var radialPart in _spawnedRadialParts)
            {
                Destroy(radialPart);
            }
            
            _spawnedRadialParts.Clear();
            
            for (int i = 0; i < numberOfRadialPart; i++)
            {
                float angle = i * 360 / (float)numberOfRadialPart - radialPartsGap / 2;
                Vector3 radialPartEulerAngles = new Vector3(0, 0, angle);

                GameObject spawnedRadialPart = Instantiate(radialPartPrefab, radialPartsCanvas);
                spawnedRadialPart.transform.position = radialPartsCanvas.position;
                spawnedRadialPart.transform.localEulerAngles = radialPartEulerAngles;

                spawnedRadialPart.GetComponent<Image>().fillAmount =
                    (1 / (float)numberOfRadialPart) - (radialPartsGap / 360);
                
                _spawnedRadialParts.Add(spawnedRadialPart);
            }
        }
    }
}