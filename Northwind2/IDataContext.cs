using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{
    public enum choix { creer, modifier }
    public interface IDataContext
    {
        
        IList<string> GetPaysFournisseurs();
        IList<Supplier> GetFournisseurs(string p);
       int GetNbProduits(string py);
        IList<Category> GetCatProduits();
        IList<Product> GetListProduits(Guid c);
        void AjouterModifierProduit(Product p, choix c);
        void SupprimerUnProduit(int id);
        IList<customer> GetClientsCommandes();
        int EnregistrerModifsProduits();
        void GetCommandesFromDataReader(List<customer> clients, SqlDataReader reader);
    }
}
