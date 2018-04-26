using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.NPC.TypeStation
{
    [TaskCategory("Main/AI/NPC/Station")]
    public class GoToStationPoint : Action
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
            agent.isStopped = false;
            agent.SetDestination(station.stationLocation.transform.position);
            return TaskStatus.Success;
        }
    }
}