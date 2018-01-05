using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Northwind2;

namespace Northwind2Test
{
    [TestClass()]
    public class TestsContexte1
    {
        [TestMethod()]
        public void GetPaysFournisseursTest()
        {
            var liste = Northwind2App.DataContext.GetPaysFournisseurs();
            int index = liste.Count;
            Assert.AreEqual(16, index);
            Assert.AreEqual("USA", liste[index - 1]);
        }

        //Vérifier que les fournisseurs du Japon sont ceux d’id 6 et 4
        [TestMethod()]
        public void GetFournisseursTest()
        {
            var liste = Northwind2App.DataContext.GetFournisseurs("Japan");

            Assert.AreEqual(4, liste[0].SupplierId);
            Assert.AreEqual(6, liste[1].SupplierId);
        }

        //Vérifier que le Royaume Uni propose 7 produits
        [TestMethod()]
        public void GetNbProduitsTest()

        {

            Assert.AreEqual(7, Northwind2App.DataContext.GetNbProduits("UK"));
        }

        //Vérifier qu’on récupère 8 catégories de produits et que la dernière est nommée « Seafood »
        [TestMethod()]
        public void GetCatProduitsTest()
        {
            var liste = Northwind2App.DataContext.GetCatProduits();
            Assert.AreEqual(8, liste.Count);
            Assert.AreEqual("Seafood", liste[liste.Count - 1].Name);
        }

        //Vérifier qu’il y a 12 produits dans la catégorie Seafood et que le 7ème est le N° 40
        [TestMethod()]
        public void GetListProduitsTest()
        {

            var liste = Northwind2App.DataContext.GetCatProduits();
            for (int i = 0; i < liste.Count; i++)
            {
                if (liste[i].Name == "Seafood")
                {
                    var x = Northwind2App.DataContext.GetListProduits(liste[i].Categoryid);
                    Assert.AreEqual(12, x.Count);
                    Assert.AreEqual(40, x.ElementAt(6).Productid);
                }
            }

        }


        [TestMethod()]
        //Ajouter un nouveau produit dans la catégorie 
        //Cheeses et vérifier qu’il y a désormais 11 produits dans cette catégorie
        public void AjouterModifierProduitTest()
        {
            Product p = new Product();

            var liste = Northwind2App.DataContext.GetCatProduits();
            for (int i = 0; i < liste.Count; i++)
            {
                if (liste[i].Description == "Cheeses")
                {
                    p.Categoryid = liste[i].Categoryid;
                    p.Name = "Nouveau produit";
                    p.Supplierid = 2;
                    p.UnitPrice = 2.5m;
                    p.UnitsInStock = 23;
                    var c = choix.creer;
                    Northwind2App.DataContext.AjouterModifierProduit(p,c);
                    Assert.AreEqual(11, Northwind2App.DataContext.GetListProduits(liste[i].Categoryid).Count);
                }
            }

        }

        //Supprimer le produit créé précédemment et vérifier qu’il y a de nouveau 10 produits dans la catégorie
        [TestMethod()]
        public void SupprimerUnProduitTest()
        {
            //Produit p = new Produit();
            //var cheeses = Contexte.GetCatProduits()[4];
            //Guid idCateCheeses = cheeses.Categoryid;
            var liste = Northwind2App.DataContext.GetCatProduits();
            for (int i = 0; i < liste.Count; i++)
                if (liste[i].Description == "Cheeses")
                {
                    Guid idcate = liste[i].Categoryid;
                    var liste2 = Northwind2App.DataContext.GetListProduits(idcate);
                    for (int y = 0; y < liste.Count; y++)
                    {
                        if (liste2[i].Name == "Nouveau produit")
                        {

                            int id = liste2[i].Productid;
                            Northwind2App.DataContext.SupprimerUnProduit(id);
                            Assert.AreEqual(10, liste2.Count);
                        }
                    }
                }

        }
    }
}

