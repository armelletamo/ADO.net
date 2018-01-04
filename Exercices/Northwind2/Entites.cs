using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{
    public class Entites
    {
        //public string Country;
        public int SupplierId { get; set; }
        public string CompanyName { get; set; }
    }
    public class categorie
    {

        public Guid Categoryid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Produit
    {
        [Display(ShortName="None")]
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
}
