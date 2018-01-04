using Northwind2;
using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercices

//    //Dans la fonction Main de la classe Program, utiliser le singleton Northwind2app.Instance pour 
//•	Définir le titre de l’appli
//•	Ajouter la page d’accueil, puis naviguer vers elle
//•	Lancer l’appli

{
    class Program
    {
        static void Main(string[] args)
        {
            Northwind2App app = Northwind2App.Instance;
            app.Title = "Northwind2";

            // Ajout des pages
            Page Accueil = new Pageaccueil();
            app.AddPage(Accueil);

            Page Fournisseur = new PageFournisseur();
            app.AddPage(Fournisseur);

            Page Produits = new PageProduits();
            app.AddPage(Produits);

            // Affichage de la page d'accueil
            app.NavigateTo(Accueil);
           
            app.NavigateTo(Fournisseur);

            app.NavigateTo(Produits);

            app.Run();


        }
    }
}
