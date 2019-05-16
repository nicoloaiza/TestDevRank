using System;
using System.ComponentModel.DataAnnotations;

namespace Test.Models
{
    public abstract class AbstractEntity<TId> : IEntity<TId>
    {
        [Key]
        public TId Id { get; set; }
    }
}
