using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.NPC.TypeStation
{
    [TaskCategory("Main/AI/NPC/Station")]
    public class ToBeStation : Conditional
    {
        Station station;

        public override void OnStart()
        {
            station = GetComponent<Station>();
        }

        public override TaskStatus OnUpdate()
        {
            if (station.stationLocation == null)
            {
                return TaskStatus.Failure;
            }
            else
            {
                return TaskStatus.Success;
            }
        }
    }
}
