using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{
    public class Pageaccueil:MenuPage
    {
        public Pageaccueil(): base("Accueil", false)
        {
            Menu.AddOption("0", "Quitter l'application",
                () => Environment.Exit(0));
            Menu.AddOption("1", "Fournisseur",
                 () => Northwind2App.Instance.NavigateTo(typeof(PageFournisseur)));
            //Menu.AddOption("2", "Client-Commande",
            //     () => Northwind2App.Instance.NavigateTo(typeof(Client_Commande)));
            Menu.AddOption("3", "Produits",
                 () => Northwind2App.Instance.NavigateTo(typeof(PageProduits)));

        }
    }
}
