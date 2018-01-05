using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{
    class Contexte3 : DbContext, IDataContext

    {
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Address> Address { get; set; }

        public Contexte3():base("name=Exercices.Settings1.Northwind2Connect")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        public void AjouterModifierProduit(Product p, choix c)
        {
            throw new NotImplementedException();
        }

        public int EnregistrerModifsProduits()
        {
            throw new NotImplementedException();
        }

        public IList<Category> GetCatProduits()
        {
            throw new NotImplementedException();
        }

        public IList<customer> GetClientsCommandes()
        {
            throw new NotImplementedException();
        }

        public void GetCommandesFromDataReader(List<customer> clients, SqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        public IList<Supplier> GetFournisseurs(string p)
        {
            var Fournisseur = Supplier.Where(s=>s.Address.Country==p).ToList();
            return Fournisseur;

        }

        public IList<Product> GetListProduits(Guid c)
        {
            throw new NotImplementedException();
        }

        public int GetNbProduits(string py)
        {
            var param = new SqlParameter
            {
                SqlDbType = SqlDbType.NVarChar,
                ParameterName = "@country",
                Value = py
            };
            int NbProduit=Database.SqlQuery<int>("select  dbo.ufn_GetNbProduits(@country)", param).Single();
            
            return NbProduit;
        }

        public IList<string> GetPaysFournisseurs()
        {
            
            var PaysFournisseur = Supplier.Select(a => a.Address.Country).Distinct().ToList();
            return PaysFournisseur;
        }

        public void SupprimerUnProduit(int id)
        {
            throw new NotImplementedException();
        }
    }
}
