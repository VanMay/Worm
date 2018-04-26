using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.Public
{
    [TaskCategory("Main/Public")]
    [TaskDescription("Compare distance to gameObject")]
    public class DistanceComparisonGameObject : Conditional
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

        public SharedGameObject targetGameObject;
        public Operation operation;
        public SharedFloat distance;

        public override TaskStatus OnUpdate()
        {
            float currentDistance = (targetGameObject.Value.transform.position - transform.position).magnitude;
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