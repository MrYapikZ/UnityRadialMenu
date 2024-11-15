using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace FloxyDev.RadialMenu
{
    public class RadialMenuManager : MonoBehaviour
    {
        public static RadialMenuManager Instance;
        [SerializeField, Range(2, 10)] private int numberOfRadialPart;
        [SerializeField, Range(0, 100)] private float radialPartsGap;
        [SerializeField] private GameObject radialPartPrefab;
        [SerializeField] private Transform radialPartsCanvas;
        [SerializeField] private Transform handTransform;
        
        public UnityEvent<int> onPartSelected;

        private readonly List<GameObject> _spawnedRadialParts = new List<GameObject>();
        private int _currentSelectedRadialPart;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpawnRadialPart();
            }

            if (Input.GetMouseButton(0))
            {
                GetSelectedRadialPart();
            }

            if (Input.GetMouseButtonUp(0))
            {
                HideRadialParts();
            }
        }

        public void HideRadialParts()
        {
            onPartSelected.Invoke(_currentSelectedRadialPart);
            radialPartsCanvas.gameObject.SetActive(false);
        }

        public void GetSelectedRadialPart()
        {
            Vector3 centerToHand = handTransform.position - radialPartsCanvas.position;
            Vector3 centerToHandProjected = Vector3.ProjectOnPlane(centerToHand, radialPartsCanvas.forward);

            float angle = Vector3.SignedAngle(radialPartsCanvas.up, centerToHandProjected, -radialPartsCanvas.forward);

            if (angle < 0) angle += 360;

            _currentSelectedRadialPart = (int)angle * numberOfRadialPart / 360;

            for (int i = 0; i < _spawnedRadialParts.Count; i++)
            {
                if (i == _currentSelectedRadialPart)
                {
                    _spawnedRadialParts[i].GetComponent<Image>().color = Color.green;
                    _spawnedRadialParts[i].transform.localScale = 1.1f * Vector3.one;
                }
                else
                {
                    _spawnedRadialParts[i].GetComponent<Image>().color = Color.white;
                    _spawnedRadialParts[i].transform.localScale = Vector3.one;
                }
            }
        }

        public void SpawnRadialPart()
        {
            radialPartsCanvas.gameObject.SetActive(true);
            radialPartsCanvas.position = handTransform.position;
            radialPartsCanvas.rotation = handTransform.rotation;
            
            foreach (var radialPart in _spawnedRadialParts)
            {
                Destroy(radialPart);
            }

            _spawnedRadialParts.Clear();

            for (int i = 0; i < numberOfRadialPart; i++)
            {
                float angle = -i * 360 / (float)numberOfRadialPart - radialPartsGap / 2;
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