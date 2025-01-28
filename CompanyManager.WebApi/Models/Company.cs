namespace CompanyManager.WebApi.Models
{
    public class Company : ModelObject, Logic.Contracts.ICompany
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Description { get; set; }

        public virtual void Copyproperties(Logic.Contracts.ICompany other)
        {
            base.CopyProperties(other);

            Name = other.Name;
            Address = other.Address;
            Description = other.Description;
        }

        public static Company Create(Logic.Contracts.ICompany company)
        {
            var result = new Company();

            result.Copyproperties(company);
            return result;
        }
    }
}
