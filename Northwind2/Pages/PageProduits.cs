using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{
    class PageProduits : MenuPage
    {
        Produit Produit = new Produit();
        IList<Produit> listeProduit;
        IList<categorie> listeCategorie;

        public PageProduits() : base("Page Produits", false)
        {
            Menu.AddOption("1", "Liste des produits", () =>
                 AfficherCatProduits());
            Menu.AddOption("2", "Créer un nouveau produit",
               CréerProduit);
            Menu.AddOption("3", "Modifier un produit",
               ModifierProduit);
            Menu.AddOption("4", "supprimer un produit",
               SupprimerProduit);
        }

        private void SupprimerProduit()
        {
            // Affiche la liste des catégories puis des produits de la catégorie sélectionnée
            Guid id = AfficherCatProduits();

            int i = Input.Read<int>("Id du produit à supprimer :");
            try
            {
                Contexte.SupprimerUnProduit(i);
            }
            catch (SqlException e)
            {
                GérerErreurSql(e);
            }
           
        }


        private void GérerErreurSql(SqlException ex)
        {
            if (ex.Number == 547)
                Output.WriteLine(ConsoleColor.Red,
                    "Le produit ne peut pas être supprimé car il est référencé par une commande");
            else
                throw ex;
        }


        private void ModifierProduit()
        {
            Guid id = AfficherCatProduits();
            /*if(listeProduit==null)
                listeProduit = Contexte.GetListProduits(id);
                */
            int idp = Input.Read<int>(" Saississez un l'id du produit à modifier: ");
            Produit Produit = listeProduit.Where(p => p.Productid == idp).FirstOrDefault();
            Produit.Categoryid = id;

            Produit.Name = Input.Read<string>("Saissisez le nom du produit: ", Produit.Name);
            Produit.Categoryid = Input.Read<Guid>(" Saississez un l'id de la categorie: ", Produit.Categoryid);
            Produit.Supplierid = Input.Read<int>(" Saississez un l'id du fournisseur: ", Produit.Supplierid);
            Produit.UnitPrice = Input.Read<decimal>("Saissisez le prix unitaire du produit: ", Produit.UnitPrice);
            Produit.UnitsInStock = Input.Read<int>("Saissisez la quantité en stock du produit: ", Produit.UnitsInStock);

            Contexte.choix m = Contexte.choix.modifier;
            Contexte.AjouterModifierProduit(Produit, m);
            Output.WriteLine(ConsoleColor.Green, " Produit modifié avec succès ");
        }

        private void CréerProduit()
        {
            var liste = Contexte.GetCatProduits();
            ConsoleTable.From(liste, "categories").Display("categories");


            Produit.Categoryid = Input.Read<Guid>(" Saississez un l'id de la categorie: ");
            Produit.Name = Input.Read<string>("Saissisez le nom du produit: ");
            Produit.Supplierid = Input.Read<int>(" Saississez un l'id du fournisseur: ");
            Produit.UnitPrice = Input.Read<decimal>("Saissisez le prix unitaire du produit: ");
            Produit.UnitsInStock = Input.Read<int>("Saissisez la quantité en stock du produit: ");

            Contexte.choix c = Contexte.choix.creer;
            Contexte.AjouterModifierProduit(Produit, c);
            Output.WriteLine(ConsoleColor.Green, " Produit créé avec succès ");

        }



        private Guid AfficherCatProduits()
        {
            if (listeCategorie == null)
                listeCategorie = Contexte.GetCatProduits();
            ConsoleTable.From(listeCategorie, "categories").Display("categories");
            Guid id = Input.Read<Guid>(" Saississez un id de categorie: ");
            if(listeProduit==null)
                listeProduit = Contexte.GetListProduits(id);
            ConsoleTable.From(listeProduit, "liste de produits").Display(" produits");
            return id;
        }
    }
}

