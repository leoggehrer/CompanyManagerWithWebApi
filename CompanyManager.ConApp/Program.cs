using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CompanyManager.ConApp
{
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(/*string[] args*/)
        {
            string input = string.Empty;
            using Logic.Contracts.IContext context = Logic.DataContext.Factory.CreateContext();

            while (!input.Equals("x", StringComparison.CurrentCultureIgnoreCase))
            {
                int index = 1;
                Console.Clear();
                Console.WriteLine("CompanyManager");
                Console.WriteLine("==========================================");

                Console.WriteLine($"{nameof(PrintCompanyies),-25}....{index++}");
                Console.WriteLine($"{nameof(QueryCompanies),-25}....{index++}");
                Console.WriteLine($"{nameof(AddCompany),-25}....{index++}");
                Console.WriteLine($"{nameof(DeleteCompany),-25}....{index++}");
                Console.WriteLine($"{nameof(PrintCustomers),-25}....{index++}");
                Console.WriteLine($"{nameof(QuerCustomers),-25}....{index++}");
                Console.WriteLine($"{nameof(AddCustomer),-25}....{index++}");
                Console.WriteLine($"{nameof(DeleteCustomer),-25}....{index++}");

                Console.WriteLine();
                Console.WriteLine($"Exit...............x");
                Console.Write("Your choice: ");

                input = Console.ReadLine()!;
                if (Int32.TryParse(input, out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            PrintCompanyies(context);
                            Console.WriteLine();
                            Console.Write("Continue with Enter...");
                            Console.ReadLine();
                            break;
                        case 2:
                            QueryCompanies(context);
                            Console.WriteLine();
                            Console.Write("Continue with Enter...");
                            Console.ReadLine();
                            break;
                        case 3:
                            AddCompany(context);
                            break;
                        case 4:
                            DeleteCompany(context);
                            break;
                        case 5:
                            PrintCustomers(context);
                            Console.WriteLine();
                            Console.Write("Continue with Enter...");
                            Console.ReadLine();
                            break;
                        case 6:
                            QuerCustomers(context);
                            Console.WriteLine();
                            Console.Write("Continue with Enter...");
                            Console.ReadLine();
                            break;
                        case 7:
                            AddCustomer(context);
                            break;
                        case 8:
                            DeleteCustomer(context);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Prints all companies in the context.
        /// </summary>
        /// <param name="context">The database context.</param>
        private static void PrintCompanyies(Logic.Contracts.IContext context)
        {
            Console.WriteLine();
            Console.WriteLine("Companies:");
            Console.WriteLine("----------");

            foreach (var company in context.CompanySet.Include(e => e.Customers))
            {
                Console.WriteLine($"{company}");
                foreach (var customer in company.Customers)
                {
                    Console.WriteLine($"\t{customer}");
                }
            }
        }

        /// <summary>
        /// Queries companies based on a user-provided condition.
        /// </summary>
        /// <param name="context">Thedatabase context.</param>
        private static void QueryCompanies(Logic.Contracts.IContext context)
        {
            Console.WriteLine();
            Console.WriteLine("Query-Companies:");
            Console.WriteLine("----------------");

            Console.Write("Query: ");
            var query = Console.ReadLine()!;

            try
            {
                foreach (var company in context.CompanySet.AsQueryable().Where(query).Include(e => e.Customers))
                {
                    Console.WriteLine($"{company}");
                    foreach (var customer in company.Customers)
                    {
                        Console.WriteLine($"{customer}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new company to the context.
        /// </summary>
        /// <param name="context">Thedatabase context.</param>
        private static void AddCompany(Logic.Contracts.IContext context)
        {
            Console.WriteLine();
            Console.WriteLine("Add company:");
            Console.WriteLine("------------");

            var company = new Logic.Entities.Company();

            Console.Write("Name [256]:          ");
            company.Name = Console.ReadLine()!;
            Console.Write("Adresse [1024]:      ");
            company.Address = Console.ReadLine()!;
            Console.Write("Beschreibung [1024]: ");
            company.Description = Console.ReadLine()!;

            context.CompanySet.Add(company);
            context.SaveChanges();
        }

        /// <summary>
        /// Deletes a company from the context.
        /// </summary>
        /// <param name="context">Thedatabase context.</param>
        private static void DeleteCompany(Logic.Contracts.IContext context)
        {
            Console.WriteLine();
            Console.WriteLine("Delete company:");
            Console.WriteLine("---------------");

            Console.WriteLine();
            Console.Write("Name: ");
            var name = Console.ReadLine()!;
            var entity = context.CompanySet.FirstOrDefault(e => e.Name == name);

            if (entity != null)
            {
                try
                {
                    context.CompanySet.Remove(entity);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.Write("Continue with enter...");
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Prints all employees in the context.
        /// </summary>
        /// <param name="context">Thedatabase context.</param>
        private static void PrintCustomers(Logic.Contracts.IContext context)
        {
            Console.WriteLine();
            Console.WriteLine("Customers:");
            Console.WriteLine("----------");

            foreach (var item in context.CustomerSet)
            {
                Console.WriteLine($"{item}");
            }
        }

        /// <summary>
        /// Queries employees based on a user-provided condition.
        /// </summary>
        /// <param name="context">Thedatabase context.</param>
        private static void QuerCustomers(Logic.Contracts.IContext context)
        {
            Console.WriteLine();
            Console.WriteLine("Query-Customers:");
            Console.WriteLine("----------------");

            Console.Write("Query: ");
            var query = Console.ReadLine()!;

            try
            {
                foreach (var customer in context.CustomerSet.AsQueryable().Where(query).Include(e => e.Company))
                {
                    Console.WriteLine($"{customer} - {customer.Company?.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new employee to the context.
        /// </summary>
        /// <param name="context">Thedatabase context.</param>
        private static void AddCustomer(Logic.Contracts.IContext context)
        {
            Console.WriteLine();
            Console.WriteLine("Add customer:");
            Console.WriteLine("------------");

            var customer = new Logic.Entities.Customer();

            Console.Write("Name [256]:   ");
            customer.Name = Console.ReadLine()!;
            Console.Write("Email [1024]: ");
            customer.Email = Console.ReadLine()!;
            Console.Write("Company name: ");
            var count = 0; 
            var companyName = Console.ReadLine()!;
            var company = context.CompanySet.FirstOrDefault(x => x.Name == companyName);

            while (company == null && count < 3)
            {
                count++;
   
                Console.Write("Company name: ");
                companyName = Console.ReadLine()!;
                company = context.CompanySet.FirstOrDefault(x => x.Name == companyName);
            }
            try
            {
                if (company != null)
                {
                    customer.CompanyId = company.Id;
                    context.CustomerSet.Add(customer);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Write("Continue with enter...");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Deletes an employee from the context.
        /// </summary>
        /// <param name="context">Thedatabase context.</param>
        private static void DeleteCustomer(Logic.Contracts.IContext context)
        {
            throw new NotImplementedException();
        }
    }
}
