using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Northwind2;

namespace Northwind2Test
{
    [TestClass()]
    public class contexteTest
    {
        [TestMethod()]
        public void GetPaysFournisseursTest()
        {
            Assert.AreEqual(16, Contexte.GetPaysFournisseurs().Count);
            Assert.AreEqual("USA", Contexte.GetPaysFournisseurs()[15]);
        }

        //Vérifier que les fournisseurs du Japon sont ceux d’id 6 et 4
        [TestMethod()]
        public void GetFournisseursTest()
        {
            string p = "Japan";
            Assert.AreEqual(6, Contexte.GetFournisseurs(p).ElementAt(1).SupplierId);
            Assert.AreEqual(4, Contexte.GetFournisseurs(p).ElementAt(0).SupplierId);
        }

        //Vérifier que le Royaume Uni propose 7 produits
        [TestMethod()]
        public void GetNbProduitsTest()

        {
            string p = "UK";
            Assert.AreEqual(7, Contexte.GetNbProduits(p));
        }

        //Vérifier qu’on récupère 8 catégories de produits et que la dernière est nommée « Seafood »
        [TestMethod()]
        public void GetCatProduitsTest()
        {
            Assert.AreEqual(8, Contexte.GetCatProduits().Count);
            Assert.AreEqual("Seafood", Contexte.GetCatProduits().ElementAt(7).Name);
        }

        //Vérifier qu’il y a 12 produits dans la catégorie Seafood et que le 7ème est le N° 40
        [TestMethod()]
        public void GetListProduitsTest()
        {
            
            var liste = Contexte.GetCatProduits();
            for (int i = 0; i < liste.Count; i++)
            {
                if (liste[i].Name == "Seafood")
                {
                    Assert.AreEqual(12, Contexte.GetListProduits(liste[i].Categoryid).Count);
                    Assert.AreEqual(40, Contexte.GetListProduits(liste[i].Categoryid).ElementAt(6).Productid);
                }
            }

        }


        [TestMethod()]
        //Ajouter un nouveau produit dans la catégorie 
        //Cheeses et vérifier qu’il y a désormais 11 produits dans cette catégorie
        public void AjouterModifierProduitTest()
        {
            Produit p = new Produit();

            var liste = Contexte.GetCatProduits();
            for (int i = 0; i < liste.Count; i++)
            {
                if (liste[i].Description == "Cheeses")
                {
                    p.Categoryid = liste[i].Categoryid;
                    p.Name = "Nouveau produit";
                    p.Supplierid = 2;
                    p.UnitPrice = 2.5m;
                    p.UnitsInStock = 23;
                    Contexte.AjouterModifierProduit(p, Contexte.choix.creer);
                    Assert.AreEqual(11, Contexte.GetListProduits(liste[i].Categoryid).Count);
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
            var liste = Contexte.GetCatProduits();
            for (int i = 0; i < liste.Count; i++)
                if (liste[i].Description == "Cheeses")
                {
                    Guid idcate = liste[i].Categoryid;
                    var liste2 = Contexte.GetListProduits(idcate);
                    for (int y = 0; y < liste.Count; y++)
                    {
                        if (liste2[i].Name == "Nouveau produit")
                        {

                            int id = liste2[i].Productid;
                            Contexte.SupprimerUnProduit(id);
                            Assert.AreEqual(10, Contexte.GetListProduits(idcate).Count);
                        }
                    }
                }
            
        }
    }
}

