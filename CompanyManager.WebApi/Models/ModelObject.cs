namespace CompanyManager.WebApi.Models
{
    public abstract class ModelObject : Logic.Contracts.IIdentifiable
    {
        public int Id { get; set; }

        public virtual void CopyProperties(Logic.Contracts.IIdentifiable other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            Id = other.Id;
        }
    }
}
