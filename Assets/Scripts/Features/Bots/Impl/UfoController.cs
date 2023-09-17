using Features.Plane.Components;
using Features.Shared;
using UnityEngine;

namespace Features.Bots.Impl
{
    public class UfoController : AerostatController
    {
        [SerializeField] 
        private FireController _fireController;

        protected override void Update()
        {
            base.Update();
            
            if (IsAlive)
            {
                _fireController.Fire();
            }
        }
    }
}