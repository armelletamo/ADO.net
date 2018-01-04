using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2.Pages
{
	public class PageFournisseurs : MenuPage
	{
		private IList<string> _pays;
		private IList<Supplier> _fournisseurs;

		public PageFournisseurs() : base ("Fournisseurs")
		{
			Menu.AddOption("1", "Liste des pays", AfficherPays);
			Menu.AddOption("2", "Fournisseurs d'un pays", AfficherFournisseursPays);
			Menu.AddOption("3", "Nombre de produits d'un pays", AfficherNbProduitsPays);
		}

		private void AfficherPays()
		{
			_pays = Contexte1.GetPaysFournisseurs();
			ConsoleTable.From(_pays, "Pays").Display("pays");
		}

		private void AfficherFournisseursPays()
		{
			string pays = Input.Read<string>("De quel pays souhaitez-vous afficher la liste des fournisseurs ? ");
			_fournisseurs = Contexte1.GetFournisseurs(pays);
			ConsoleTable.From(_fournisseurs).Display("fournisseurs");
		}

		private void AfficherNbProduitsPays()
		{
			string pays = Input.Read<string>("pour quel pays souhaitez-vous afficher le nombre de produits ? ");
			int nb = Contexte1.GetNbProduits(pays);
			Output.WriteLine(ConsoleColor.Cyan, nb.ToString() + " produits");
		}
	}
}
