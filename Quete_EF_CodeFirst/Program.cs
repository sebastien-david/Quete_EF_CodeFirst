using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace Quete_EF_CodeFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SavingContext())
            {
                Person richard = new Person { Name = "Richard" };
                context.Persons.Add(richard);

                SavingAccount account1 = new SavingAccount
                {
                    Person = richard,
                    SavingRateMonth = 0.05,
                    InitialFound = 2000000
                };

                SavingAccount account2 = new SavingAccount
                {
                    Person = richard,
                    SavingRateMonth = 0.15 / 12,
                    InitialFound = 250000
                };

                SavingAccount account3 = new SavingAccount
                {
                    Person = richard,
                    SavingRateMonth = 0.02 / 12,
                    InitialFound = 10000000
                };

                context.SavingAccounts.Add(account1);
                context.SavingAccounts.Add(account2);
                context.SavingAccounts.Add(account3);

                context.SaveChanges();
                string richardString = "Epargnes de Mr.Richard durant les trois dernières années\n\n";
                var richarSavingAccounts = context.SavingAccounts.Where(a => a.PersonId == richard.ID).ToList();
                foreach (var account in richarSavingAccounts)
                {
                    double interest = SavingCalculator.SavingCalculThreeYears(account);
                    richardString += $"compte : {account.ID}\n" +
                        $"Taux d'épargne par mois : {account.SavingRateMonth} %\n" +
                        $"Fonds déposés initialement : {account.InitialFound} €\n" +
                        $"Intérêts aquis durant les trois dernières années : {interest} €\n\n";
                }

                MessageBox.Show(richardString,"Comptes de Mr. Richard",
                    MessageBoxButtons.OK);
            }
        }
    }
    public class SavingAccount
    {
        public int ID { get; set; }
        public double SavingRateMonth { get; set; }
        public double InitialFound { get; set; }
        public int PersonId { get; set; }

        public virtual Person Person { get; set; }

    }

    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SavingAccount> SavingAccounts { get; set; }

    }

    class SavingCalculator
    {
        public static double SavingCalculThreeYears(SavingAccount savingaccount)
        {
            return (savingaccount.SavingRateMonth * savingaccount.InitialFound) * 36;
        }

    }

    public class SavingContext : DbContext
    {
        public SavingContext() : base("SavingContextDB") { }
        public virtual DbSet<SavingAccount> SavingAccounts { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
    }
}
