using System;
namespace Test.Models
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}
