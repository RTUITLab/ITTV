using System;

namespace KinectTvV2.API.Domain.Entities
{
    public class Entity
    {
        public Guid Id { get; protected set; }
        public DateTime Created { get; protected set; } = DateTime.Now;
        public DateTime Updated { get; protected set; }
        public bool Inactive { get; protected set; }
        protected void SetUpdated()
        {
            Updated = DateTime.Now;
        }
        public void SetInactive()
        {
            Inactive = true;
        }
    }
}