using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Features.Bots.Impl
{
    public class AerostatController : Bot
    {
        [SerializeField] 
        private Transform _bodyRotationAroundSelf;

        [SerializeField] 
        private float _rotationsSpeed = 25f;

        private NativeArray<float3> _nativeRotation;
        private JobHandle _rotationJobHandle;

        private void Awake()
        {
            _nativeRotation = new NativeArray<float3>(1, Allocator.Persistent);
        }

        private void OnDestroy()
        {
            _nativeRotation.Dispose();
        }

        private void Update()
        {
            MoveForward();

            var rotationJob = new RotationJob(_rotationsSpeed, Time.deltaTime, _nativeRotation);
            _rotationJobHandle = rotationJob.Schedule();
        }

        private void LateUpdate()
        {
            _rotationJobHandle.Complete();
            
            _bodyRotationAroundSelf.Rotate(_nativeRotation[0], Space.Self);
        }
    }
    
    [BurstCompile]
    public struct RotationJob : IJob
    {
        private float _rotationSpeed;
        private float _deltaTime;
        private NativeArray<float3> _result;

        public RotationJob(float rotationSpeed, float deltaTime, NativeArray<float3> result)
        {
            _deltaTime = deltaTime;
            _rotationSpeed = rotationSpeed;
            _result = result;
        }

        public void Execute()
        {
            _result[0] = Vector3.back * _rotationSpeed * _deltaTime;
        }
    }
}