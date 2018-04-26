using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.Public
{
    [TaskCategory("Main/Public")]
    [TaskDescription("Compare distance to position")]
    public class DistanceComparisonPosition : Conditional
    {
        public enum Operation
        {
            LessThan,
            LessThanOrEqualTo,
            EqualTo,
            NotEqualTo,
            GreaterThanOrEqualTo,
            GreaterThan
        }

        public SharedVector3 targetPosition;
        public Operation operation;
        public SharedFloat distance;

        public override TaskStatus OnUpdate()
        {
            float currentDistance = (targetPosition.Value - transform.position).magnitude;
            switch (operation)
            {
                case Operation.EqualTo:
                    {
                        if (currentDistance == distance.Value)
                        {
                            return TaskStatus.Success;
                        }
                        break;
                    }
                case Operation.GreaterThan:
                    {
                        if (currentDistance > distance.Value)
                        {
                            return TaskStatus.Success;
                        }
                        break;
                    }
                case Operation.GreaterThanOrEqualTo:
                    {
                        if (currentDistance >= distance.Value)
                        {
                            return TaskStatus.Success;
                        }
                        break;
                    }
                case Operation.LessThan:
                    {
                        if (currentDistance < distance.Value)
                        {
                            return TaskStatus.Success;
                        }
                        break;
                    }
                case Operation.LessThanOrEqualTo:
                    {
                        if (currentDistance <= distance.Value)
                        {
                            return TaskStatus.Success;
                        }
                        break;
                    }
                case Operation.NotEqualTo:
                    {
                        if (currentDistance != distance.Value)
                        {
                            return TaskStatus.Success;
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return TaskStatus.Failure;
        }
    }
}