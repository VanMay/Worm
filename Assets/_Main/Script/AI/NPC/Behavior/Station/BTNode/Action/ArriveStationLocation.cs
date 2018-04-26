using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.NPC.TypeStation
{
    [TaskCategory("Main/AI/NPC/Station")]
    public class ArriveStationLocation : Action
    {
        Station station;
        NavMeshAgent agent;

        public override void OnStart()
        {
            station = GetComponent<Station>();
            agent = GetComponent<NavMeshAgent>();

        }

        public override TaskStatus OnUpdate()
        {
            station.ArriveStationPoint();
            NavMeshObstacle obstacle = station.stationLocation.GetComponent<NavMeshObstacle>();
            agent.enabled = false;
            obstacle.enabled = true;
            return TaskStatus.Success;
        }
    }
}
