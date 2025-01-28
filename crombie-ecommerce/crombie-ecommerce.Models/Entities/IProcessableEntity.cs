using Newtonsoft.Json;
using System;

namespace crombie_ecommerce.Models.Entities
{
    public interface IProcessableEntity
    {
        Guid Id { get; }
        Guid UserId { get; }
        string SerializeToJson();
    }
}
