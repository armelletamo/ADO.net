using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{

    public class Supplier
    {
        //public string Country;
        public int SupplierId { get; set; }
        [ForeignKey("Address")]
        [Display(ShortName = "None")]
        public Guid Addressid { get; set; }

        public string CompanyName { get; set; }
        [Display(ShortName = "None")]
        public virtual Address Address { get; set; }

    }
    public class Category
    {

        public Guid Categoryid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Product
    {
        [Display(ShortName = "None")]
        public Guid Categoryid { get; set; }
        public int Productid { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int Supplierid { get; set; }
    }
    //public class NouveauProduits
    //{

    //    public Guid Categoryid { get; set; }
    //    public string Name { get; set; }
    //    public int Supplierid { get; set; }
    //    public decimal UnitPrice { get; set; }
    //    public int UnitsInStock { get; set; }
    //}

    public class customer
    {

        public string customerid { get; set; }
        public string companyname { get; set; }
        [Display(ShortName = "None")]
        public List<orders> commandes { get; set; }
    }

    public class orders
    {

        public int ordersid { get; set; }
        public DateTime orderdate { get; set; }
        public DateTime shippeddate { get; set; }
        public Decimal freight { get; set; }
        public int productid { get; set; }

    }

    //    •	Créer une entité Address contenant l’id et le pays.
    //•	Sur l’entité Supplier, ajouter une propriété virtuelle Address de type Address,
    //        et une propriété AddressId de type Guid.Ceci permet de relier l’adresse au fournisseur.
    //        Faire en sorte qu’elles ne s’affichent pas à l’écran quand on affiche la liste des fournisseurs

    public class Address
    {
 
        public Guid Addressid { get; set; }
        public string Country { get; set; }
       
    }




}
