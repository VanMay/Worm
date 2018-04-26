using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.NPC.TypeStation
{
    [TaskCategory("Main/AI/NPC/Station")]
    public class LeaveStationPoint : Action
    {
        Station station;
        NavMeshAgent agent;
        NavMeshObstacle obstacle;

        public override void OnStart()
        {
            station = GetComponent<Station>();
            agent = GetComponent<NavMeshAgent>();
            obstacle = station.stationLocation.GetComponent<NavMeshObstacle>();
            station.LeaveStationPoint();
        }

        public override TaskStatus OnUpdate()
        {
            if (obstacle.enabled)
            {
                obstacle.enabled = false;
                return TaskStatus.Running;
            }
            agent.enabled = true;
            return TaskStatus.Success;
        }
    }
}