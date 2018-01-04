using Northwind2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class Contexte
{

    public static List<client> GetClientsCommandes(int i)
    {
        var clients = new List<client>();

        var cmd1 = new SqlCommand();
        cmd1.CommandText = @"select  od.ProductId,c.customerid, c.CompanyName,
 o.OrderId, o.OrderDate, o.Freight, sum(od.Quantity*(1-od.Discount)*UnitPrice) MontantTotal
from Customer c
inner join Orders o on o.CustomerId=c.CustomerId
inner join OrderDetail od on od.OrderId=o.OrderId
 where od.productid=@productid
group by od.ProductId,c.customerid, c.CompanyName,
 o.OrderId, o.OrderDate, o.Freight";

        var param = new SqlParameter
        {
            SqlDbType = SqlDbType.Int,
            ParameterName = "@productid",
            Value = i
        };
        // Ajout à la collection des paramètres de la commande
        cmd1.Parameters.Add(param);



        using (var cnx1 = new SqlConnection(Settings1.Default.Northwind2Connect))
        {
            cmd1.Connection = cnx1;
            cnx1.Open();

            using (SqlDataReader sdr1 = cmd1.ExecuteReader())
            {
                while (sdr1.Read())
                {
                    client Monobjet = new client();
                    Monobjet.customerid = (string)sdr1["customerid"];
                    Monobjet.companyname = (string)sdr1["CompanyName"];
                    Monobjet.commandes = (int)sdr1["ProductId"];

                    clients.Add(Monobjet);
                }
            }
        }
        return clients;
    }

    public static List<string> GetPaysFournisseurs()
    {
        var listPaysFournisseurs = new List<string>();

        var cmd = new SqlCommand();
        cmd.CommandText = @"SELECT distinct A.Country
                           from Address A
                            inner join Supplier S on S.AddressId=A.AddressId
                            order by 1";

        using (var cnx = new SqlConnection(Settings1.Default.Northwind2Connect))
        {
            cmd.Connection = cnx;
            cnx.Open();

            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read())
                {
                    listPaysFournisseurs.Add((string)sdr["Country"]);
                }
            }
        }

        return listPaysFournisseurs;

    }
    public static List<Entites> GetFournisseurs(string p)
    {
        var Fournisseurs = new List<Entites>();

        var cmd1 = new SqlCommand();
        cmd1.CommandText = @"SELECT S.SupplierID, S.CompanyName, A.Country
                           from Supplier S 
                           inner join Address A on S.AddressId=A.AddressId
                           where A.country=@pays
                           order by 1";

        var param = new SqlParameter
        {
            SqlDbType = SqlDbType.NVarChar,
            ParameterName = "@pays",
            Value = p
        };
        // Ajout à la collection des paramètres de la commande
        cmd1.Parameters.Add(param);



        using (var cnx1 = new SqlConnection(Settings1.Default.Northwind2Connect))
        {
            cmd1.Connection = cnx1;
            cnx1.Open();

            using (SqlDataReader sdr1 = cmd1.ExecuteReader())
            {
                while (sdr1.Read())
                {
                    Entites Monobjet = new Entites();
                    Monobjet.SupplierId = (int)sdr1["SupplierId"];
                    Monobjet.CompanyName = (string)sdr1["CompanyName"];
                    //Monobjet.Country = (string)sdr1["Country"];
                    Fournisseurs.Add(Monobjet);
                }
            }
        }
        return Fournisseurs;
    }

    public static int GetNbProduits(string py)
    {
        var cmd2 = new SqlCommand();
        cmd2.CommandText = @"select  dbo.ufn_GetNbProduits (@country)";
        var param = new SqlParameter
        {
            SqlDbType = SqlDbType.NVarChar,
            ParameterName = "@country",
            Value = py
        };
        // Ajout à la collection des paramètres de la commande
        cmd2.Parameters.Add(param);

        using (var cnx2 = new SqlConnection(Settings1.Default.Northwind2Connect))
        {
            cmd2.Connection = cnx2;
            cnx2.Open();
            return (int)cmd2.ExecuteScalar();
        }
    }
    public static List<categorie> GetCatProduits()
    {
        var listcatproduits = new List<categorie>();

        var cmd3 = new SqlCommand();
        cmd3.CommandText = @"select CategoryId, Name, Description
                            from Category
                            order by 1";

        using (var cnx3 = new SqlConnection(Settings1.Default.Northwind2Connect))
        {
            cmd3.Connection = cnx3;
            cnx3.Open();

            using (SqlDataReader sdr3 = cmd3.ExecuteReader())
            {
                while (sdr3.Read())
                {
                    categorie MaCat = new categorie();
                    MaCat.Categoryid = (Guid)sdr3["CategoryId"];
                    MaCat.Name = (string)sdr3["Name"];
                    MaCat.Description = (string)sdr3["Description"];
                    listcatproduits.Add(MaCat);
                }
            }
        }

        return listcatproduits;
    }

    public static List<Produit> GetListProduits(Guid c)
    {
        var listproduits = new List<Produit>();

        var cmd = new SqlCommand();
        cmd.CommandText = @"SELECT p.Productid, p.Name, p.UnitPrice, p.UnitsInStock, p.supplierid
                           from product p
                           inner join category c on p.categoryid=c.categoryid
                           where c.categoryid=@categoryid
                           order by 1";

        var param = new SqlParameter
        {
            SqlDbType = SqlDbType.UniqueIdentifier,
            ParameterName = "@categoryid",
            Value = c
        };
        // Ajout à la collection des paramètres de la commande
        cmd.Parameters.Add(param);



        using (var cnx = new SqlConnection(Settings1.Default.Northwind2Connect))
        {
            cmd.Connection = cnx;
            cnx.Open();

            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read())
                {
                    Produit MonProduit = new Produit();
                    MonProduit.Productid = (int)sdr["Productid"];
                    MonProduit.Name = (string)sdr["Name"];
                    MonProduit.UnitPrice = (decimal)sdr["UnitPrice"];
                    MonProduit.UnitsInStock = (Int16)sdr["UnitsInStock"];
                    MonProduit.Supplierid = (int)sdr["supplierid"];
                    listproduits.Add(MonProduit);
                }
            }
        }
        return listproduits;
    }

    public enum choix { creer, modifier }


    public static void AjouterModifierProduit(Produit p, choix c)
    {
        if (c == choix.creer)
        {
            var cmd = new SqlCommand();

            cmd.CommandText = @"insert product (categoryid, Name, supplierID, unitprice, unitsinstock) 
values (@categoryid, @Name, @supplierID, @unitprice, @unitsinstock)

";
            var param1 = new SqlParameter
            {
                SqlDbType = SqlDbType.UniqueIdentifier,
                ParameterName = "@categoryid",
                Value = p.Categoryid
            };
            cmd.Parameters.Add(param1);
            var param2 = new SqlParameter
            {
                SqlDbType = SqlDbType.NVarChar,
                ParameterName = "@Name",
                Value = p.Name
            };
            cmd.Parameters.Add(param2);

            var param3 = new SqlParameter
            {
                SqlDbType = SqlDbType.Int,
                ParameterName = "@supplierID",
                Value = p.Supplierid
            };
            cmd.Parameters.Add(param3);

            var param4 = new SqlParameter
            {
                SqlDbType = SqlDbType.Money,
                ParameterName = "@unitprice",
                Value = p.UnitPrice
            };
            cmd.Parameters.Add(param4);

            var param5 = new SqlParameter
            {
                SqlDbType = SqlDbType.SmallInt,
                ParameterName = "@unitsinstock",
                Value = p.UnitsInStock
            };
            cmd.Parameters.Add(param5);


            using (var cnx = new SqlConnection(Settings1.Default.Northwind2Connect))
            {
                cmd.Connection = cnx;
                cnx.Open();

                cmd.ExecuteNonQuery();
            }

        }

        else if (c == choix.modifier)
        {
            var cmd = new SqlCommand();

            cmd.CommandText = @"update product set categoryid=@categoryid,
                                Name=@Name, supplierID=@supplierID, 
                                unitprice=@unitprice, unitsinstock=@unitsinstock 
                                  where productid=@id1
                                ";
            cmd.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@id1", Value = p.Productid });

            var param1 = new SqlParameter
            {
                SqlDbType = SqlDbType.UniqueIdentifier,
                ParameterName = "@categoryid",
                Value = p.Categoryid
            };
            cmd.Parameters.Add(param1);
            var param2 = new SqlParameter
            {
                SqlDbType = SqlDbType.NVarChar,
                ParameterName = "@Name",
                Value = p.Name
            };
            cmd.Parameters.Add(param2);

            var param3 = new SqlParameter
            {
                SqlDbType = SqlDbType.Int,
                ParameterName = "@supplierID",
                Value = p.Supplierid
            };
            cmd.Parameters.Add(param3);

            var param4 = new SqlParameter
            {
                SqlDbType = SqlDbType.Money,
                ParameterName = "@unitprice",
                Value = p.UnitPrice
            };
            cmd.Parameters.Add(param4);

            var param5 = new SqlParameter
            {
                SqlDbType = SqlDbType.SmallInt,
                ParameterName = "@unitsinstock",
                Value = p.UnitsInStock
            };
            cmd.Parameters.Add(param5);


            using (var cnx = new SqlConnection(Settings1.Default.Northwind2Connect))
            {
                cmd.Connection = cnx;
                cnx.Open();

                cmd.ExecuteNonQuery();
            }

        }
    }
    public static void SupprimerUnProduit(int id)
    {
        var cmd = new SqlCommand();
        cmd.CommandText = @"delete from Product where ProductId = @id";
        cmd.Parameters.Add(new SqlParameter
        {
            SqlDbType = SqlDbType.Int,
            ParameterName = "@id",
            Value = id
        });

        using (var conn = new SqlConnection(Settings1.Default.Northwind2Connect))
        {
            cmd.Connection = conn;
            conn.Open();
            cmd.ExecuteNonQuery();
        }

    }
}









