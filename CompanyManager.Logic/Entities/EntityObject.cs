﻿namespace CompanyManager.Logic.Entities
{
    /// <summary>
    /// Represents an abstract base class for entities with an identifier.
    /// </summary>
    public abstract class EntityObject : Common.Contracts.IIdentifiable
    {
        /// <summary>
        /// Gets or sets the identifier of the entity.
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
    }
}
