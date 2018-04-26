using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.NPC.TypeStation
{
    [TaskCategory("Main/AI/NPC/Station")]
    public class StationPointArrived : Conditional
    {
        public float distance = 0.5f;
        Station station;

        public override void OnStart()
        {
            station = GetComponent<Station>();
        }

        public override TaskStatus OnUpdate()
        {
            if ((station.stationLocation.transform.position - transform.position).magnitude < distance)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
    }
}
