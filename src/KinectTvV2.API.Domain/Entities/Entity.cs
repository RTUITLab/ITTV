using System;

namespace KinectTvV2.API.Domain.Entities
{
    public class Entity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedDate { get; protected set; } = DateTime.Now;
        public DateTime UpdatedDate { get; protected set; }

        public void SetUpdated()
        {
            UpdatedDate = DateTime.Now;
        }
    }
}