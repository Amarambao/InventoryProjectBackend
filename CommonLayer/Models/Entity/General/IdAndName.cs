using CommonLayer.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace CommonLayer.Models.Entity.General
{
    public abstract class IdAndName
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string NormalizedName { get; set; }

        [SetsRequiredMembers]
        protected IdAndName(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            NormalizedName = name.CustomNormalize();
        }
    }
}
