using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.AI.NPC.TypeStation
{
    [TaskCategory("Main/AI/NPC/Station")]
    [TaskDescription("Return running until this NPC exited the station point")]
    public class PlayStationAnimation : Action
    {
        Station station;
        Animator anim;

        float timer;

        public override void OnStart()
        {
            station = GetComponent<Station>();
            anim = GetComponent<Animator>();
            timer = 0;
        }

        public override TaskStatus OnUpdate()
        {
            if (station.stationAnimParameter != "")
            {
                anim.SetBool(station.stationAnimParameter, true);
            }

            if (Time.time < station.stationStartTime + station.stationTime)
            {
                return TaskStatus.Running;
            }

            if (station.stationAnimParameter != "")
            {
                anim.SetBool(station.stationAnimParameter, false);
            }
            return TaskStatus.Success;
        }
    }
}