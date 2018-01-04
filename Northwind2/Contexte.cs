using Northwind2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class Contexte
{

    public static List<Client> GetClientsCommandes()
    {
        var Clients = new List<Client>();

        var cmd1 = new SqlCommand();
        cmd1.CommandText = @"select  od.ProductId,c.customerid, c.CompanyName,
 o.OrderId, o.OrderDate, o.shippeddate, o.Freight, isnull(sum(od.Quantity*(1-od.Discount)*UnitPrice),0) MontantTotal
from Customer c
left outer join Orders o on o.CustomerId=c.CustomerId
left outer join OrderDetail od on od.OrderId=o.OrderId

group by od.ProductId,c.customerid, c.CompanyName,
 o.OrderId, o.OrderDate, o.shippeddate, o.Freight
order by 1,2";

        using (var cnx1 = new SqlConnection(Settings1.Default.Northwind2Connect))
        {
            cmd1.Connection = cnx1;
            cnx1.Open();

            using (SqlDataReader sdr1 = cmd1.ExecuteReader())
            {
                while (sdr1.Read())
                {
                    GetCommandesFromDataReader(Clients, sdr1);

                }
            }
        }
        return Clients;
    }

    private static void GetCommandesFromDataReader(List<Client> clients, SqlDataReader reader)
    {
        string customerid = (string)reader["customerid"];

        // Si l'id de la région courante est différent de celui de la dernière région de 
        // la liste, on crée un nouvel objet Region
        Client r = null;
        if (clients.Count == 0 || clients[clients.Count - 1].customerid != customerid)
        {
            r = new Client();
            r.customerid = (string)reader["customerid"];
            r.companyname = (string)reader["CompanyName"];
            r.commandes = new List<Command>();
            clients.Add(r);
        }
        else r = clients[clients.Count - 1];

        // Création du territoire et association à la région
        Command t = new Command();
        t.orderid = (string)reader["OrderId"];
        t.productid = (int)reader["ProductId"];
        t.orderdate = (DateTime)reader["OrderDate"];
        t.shippeddate = (DateTime)reader["shippeddate"];
        t.freight = (decimal)reader["Freight"];
        r.commandes.Add(t);


    }
    //public static IList<Customer> GetClientsCommandes()
    //{
    //    var list = new List<Customer>();
    //    var cmd = new SqlCommand();
    //    cmd.CommandText = @"select C.CustomerId, C.CompanyName,
				//O.OrderId, OrderDate, ShippedDate, Freight,
				//count(D.ProductId) ItemsCount,
				//SUM(D.Quantity * D.UnitPrice) Total
				//from Customer C
				//left outer join Orders O on C.CustomerId = O.CustomerId
				//inner join OrderDetail D on O.OrderId = D.OrderId
				//group by C.CustomerId, C.CompanyName, O.OrderId, OrderDate, ShippedDate, Freight
				//order by C.CustomerId, O.OrderId";

    //    using (var conn = new SqlConnection(_connectString))
    //    {
    //        cmd.Connection = conn;
    //        conn.Open();

    //        using (SqlDataReader reader = cmd.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                string idClient = (string)reader["CustomerId"];

    //                // Si l'id du client courant est différent de celui du dernier
    //                // client de la liste, on crée un nouvel objet Client
    //                Customer cli = null;
    //                if (list.Count == 0 || list[list.Count - 1].CustomerId != idClient)
    //                {
    //                    cli = new Customer();
    //                    cli.CustomerId = idClient;
    //                    cli.CompanyName = (string)reader["CompanyName"];
    //                    cli.Orders = new List<Order>();
    //                    list.Add(cli);
    //                }
    //                else
    //                    cli = list[list.Count - 1];

    //                // Création de la commande
    //                Order com = new Order();
    //                com.OrderId = (int)reader["OrderId"];
    //                com.OrderDate = (DateTime)reader["OrderDate"];
    //                if (reader["ShippedDate"] != DBNull.Value)
    //                    com.ShippedDate = (DateTime)reader["ShippedDate"];
    //                com.ItemsCount = (int)reader["ItemsCount"];
    //                com.Total = (decimal)reader["Total"];
    //                com.Freight = (decimal)reader["Freight"];

    //                cli.Orders.Add(com);
    //            }
    //        }
    //    }

    //    return list;
    //}

    public static IList<string> GetPaysFournisseurs()
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
    public static IList<Entites> GetFournisseurs(string p)
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
    public static IList<categorie> GetCatProduits()
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

    public static IList<Produit> GetListProduits(Guid c)
    {
        var listproduits = new List<Produit>();

        var cmd = new SqlCommand();
        cmd.CommandText = @"SELECT p.Productid, p.Name, p.UnitPrice, p.UnitsInStock, p.supplierid
                           from product p
                           inner join category c on p.categoryid=c.categoryid
                           where c.categoryid=@categoryid";

        var param = new SqlParameter               // ceci est un initialiseur
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









