using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameStates;
using Lean.Touch;
using NaughtyAttributes;
using UnityEngine;
using Tower.Floor;
using Utils.PoolSystem;
using Utils;
using Cinemachine;

namespace Tower
{
    public class TowerController : Singleton<TowerController>
    {
        [SerializeField] private GameObject floorPrefab;
        public List<GameObject> floors = new();
        public GameObject tempFloor;
        [ReorderableList]
        public List<Transform> selectedFloors = new();
        public LeanSelectByFinger selections;
        public TowerData data;
        public CinemachineTargetGroup targetGroup;
        public void Start()
        {
            data = (TowerData)DataPersistenceController.Instance.GetData("tower", new TowerData());
            //Debug.Log(data.FloorCount);
            for (int i = 0; i < data.floorCount; i++)
            {
                AddFloor(i, false);
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AddNewFloor();
            }
        }

        public virtual void AddFloor(int whichFloor, bool isNewFloor = true)
        {
            if (isNewFloor)
                data.FloorCount++;
            //Debug.Log(data.Guns[whichFloor]);
            tempFloor = floorPrefab.Spawn(transform.localPosition + 1.6f * floors.Count * Vector3.up, transform.localRotation, transform);
            if (isNewFloor && floors.Count > 0) floors[^1].GetComponent<FloorMine>().addFloorButton.gameObject.SetActive(false);
            floors.Add(tempFloor);
            if (isNewFloor) floors[^1].GetComponent<FloorMine>().addFloorButton.gameObject.SetActive(true);
            var floorBase = tempFloor.GetComponent<FloorBase>();
            floorBase.Init(this, data.Guns[whichFloor]);
            targetGroup.AddMember(tempFloor.transform.GetChild(1), 1 - (whichFloor * 0.05f), whichFloor == 0 ? 4 : 2);
        }

        public void EditModeOpen(bool state)
        {
            floors[^1].GetComponent<FloorMine>().addFloorButton.gameObject.SetActive(state);
        }
        public void AddNewFloor()
        {
            AddFloor(floors.Count, true);
        }

        protected void AddToList()
        {
            if (GameStateManager.Instance.GetCurrentState() != typeof(OnGameState)) return;

            foreach (var s in selections.Selectables.Where(s =>
                         !selectedFloors.Contains(s.transform) && !s.GetComponentInParent<EnemyTower>()))
            {
                selectedFloors.Add(s.transform);
            }
        }

        protected void ResetSelected()
        {
            StartCoroutine(ClearSelected());
        }

        protected IEnumerator ClearSelected()
        {
            yield return new WaitForEndOfFrame(); //WaitForSeconds(0.1f);
            selectedFloors.Clear();
        }
    }
}