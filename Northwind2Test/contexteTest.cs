using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Guid c = Guid.Parse("EB4E5F53-8711-4118-946E-F89E00615EC6");
            Assert.AreEqual(12, Contexte.GetListProduits(c).Count);
        }

        [TestMethod()]
        public void AjouterModifierProduitTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SupprimerUnProduitTest()
        {
            Assert.Fail();
        }
    }
}

