using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFModelRelationsApp
{
    public class Company
    {
        public int Id { set; get; }
        public string? Title { set; get; } = null!;
        public List<Employe> Employes { get; set; } = new List<Employe>();
    }
    public class Employe
    {
        public int Id { set; get; }
        public string? Name { set; get; } = null!;
        public DateTime BirthDate { set; get; }
        //public string CompanyName { set; get; } // свойство - внешний ключ
        //[ForeignKey("CompanyKeyInfo")]
        public int? CompanyId { set; get; } // свойство - внешний ключ
        public Company? Company { set; get; } // навигационное свойство
    }

    public class AppContext : DbContext
    {
        public DbSet<Employe> Employes { get; set; }
        public DbSet<Company> Companies { get; set; }
        public AppContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CompaniesDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // настройка внешнего ключа для любого свойства
            //modelBuilder.Entity<Employe>()
            //            .HasOne(e => e.Company)
            //            .WithMany(c => c.Employes)
            //            .HasForeignKey(e => e.CompanyName)
            //            .HasPrincipalKey(c => c.Title);

            // установка каскадного удаления - обнуления
            //modelBuilder.Entity<Employe>()
            //            .Property(e => e.CompanyId)
            //            


            modelBuilder.Entity<Employe>()
                        .HasOne(e => e.Company)
                        .WithMany(c => c.Employes)
                        .OnDelete(DeleteBehavior.SetNull);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            using(AppContext context = new())
            {
                /*
                Company[] companies = new Company[]
                {
                    new(){ Title = "Yandex"},
                    new(){ Title = "Mail Group"},
                    new(){ Title = "Ozon"},
                };
                context.Companies.AddRange(companies);
                context.SaveChanges();

                Employe[] employes = new Employe[]
                {
                    new Employe(){ Name = "Joe",
                                   BirthDate = new DateTime(1997, 6, 21),
                                   CompanyId = companies[0].Id},
                    new Employe(){ Name = "Bob",
                                   BirthDate = new DateTime(2001, 2, 1),
                                   CompanyId = companies[1].Id},
                    new Employe(){ Name = "Leo",
                                   BirthDate = new DateTime(1989, 8, 9),
                                   CompanyId = companies[2].Id},
                    new Employe(){ Name = "Tim",
                                   BirthDate = new DateTime(2002, 4, 18),
                                   CompanyId = companies[0].Id},
                    new Employe(){ Name = "Sam",
                                   BirthDate = new DateTime(1992, 11, 12),
                                   CompanyId = companies[1].Id},
                    new Employe(){ Name = "Jim",
                                   BirthDate = new DateTime(1981, 7, 5),
                                   CompanyId = companies[2].Id},
                };
                context.Employes.AddRange(employes);
                */

                Employe[] employes2 = new Employe[]
                {
                    new Employe(){ Name = "Joe",
                                   BirthDate = new DateTime(1997, 6, 21) },
                    new Employe(){ Name = "Bob",
                                   BirthDate = new DateTime(2001, 2, 1) },
                    new Employe(){ Name = "Leo",
                                   BirthDate = new DateTime(1989, 8, 9) },
                    new Employe(){ Name = "Tim",
                                   BirthDate = new DateTime(2002, 4, 18) },
                    new Employe(){ Name = "Sam",
                                   BirthDate = new DateTime(1992, 11, 12) },
                    new Employe(){ Name = "Jim",
                                   BirthDate = new DateTime(1981, 7, 5) },
                };
                context.Employes.AddRange(employes2);
                //context.SaveChanges();

                Company[] companies2 = new Company[]
                {
                    new(){ Title = "Yandex", Employes = { employes2[0], employes2[3] } },
                    new(){ Title = "Mail Group", Employes = { employes2[1], employes2[4] }},
                    new(){ Title = "Ozon", Employes = { employes2[2], employes2[5] }},
                };
                context.Companies.AddRange(companies2);
                context.SaveChanges();

                
                foreach(Company company in context.Companies)
                {
                    Console.WriteLine(company.Title);
                    foreach(Employe employe in company.Employes)
                        Console.WriteLine($"\t{employe.Name} [{employe.BirthDate.ToString()}]");
                    Console.WriteLine();
                }
                Console.WriteLine("----------------------");
                //var employes = context.Employes.Include(e => e.Company).ToList();
                foreach (Employe employe in context.Employes)
                {
                    Console.WriteLine($"{employe.Name} {employe.Company?.Title}");
                }

                var comp = context.Companies.FirstOrDefault();
                if (comp is not null)
                    context.Companies.Remove(comp);
                context.SaveChanges();

                Console.WriteLine("------- After Remove -------");
                foreach (Employe employe in context.Employes)
                {
                    Console.WriteLine($"{employe.Name} {employe.Company?.Title}");
                }
            }
        }
    }
}