﻿namespace DominandoEFCore17.Domain.Abstract
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string TenantId { get; set; }
    }
}