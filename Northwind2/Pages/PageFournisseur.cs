using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{
    public class PageFournisseur : MenuPage
    {
        public PageFournisseur() : base("Page Fournisseurs", false)
        {
            Menu.AddOption("1", "Liste de pays",
                AfficherListePaysFournisseurs);
            Menu.AddOption("2", "Fournisseurs d'un pays",
                AfficherFournisseursPays);
            Menu.AddOption("3", "Nombre de produits d'un pays",AfficherNBProduits
              );
        }

        private void AfficherNBProduits()
        {
            string pays;
            Console.WriteLine("quel pays ? ");
            pays = Console.ReadLine();
            int nombre = Northwind2App.DataContext.GetNbProduits(pays);
            //Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine( nombre.ToString() + "produits");

        }

        private void AfficherFournisseursPays()
        {
            string pays;
            Console.WriteLine("quel pays ? ");
            pays = Console.ReadLine();
            IList<Supplier> liste2 = Northwind2App.DataContext.GetFournisseurs(pays);
            ConsoleTable.From(liste2, "Fournisseur").Display("Fournisseurs");

        }

        private void AfficherListePaysFournisseurs()
        {
            var liste = Northwind2App.DataContext.GetPaysFournisseurs();
            ConsoleTable.From(liste, "Pays").Display("Pays");

        }
    }
}
