using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{
	public enum Operations { Ajout, Modification, Suppression }

	public static class Contexte1
	{
		// Chaîne de connexion
		private static readonly string _connectString = ConfigurationManager.ConnectionStrings["Northwind2Connection"].ConnectionString;

		// Requête select avec DataReader
		// Récupère la liste des pays des fournisseurs
		public static IList<string> GetPaysFournisseurs()
		{
			var list = new List<string>();
			var cmd = new SqlCommand();
			cmd.CommandText = @"select distinct A.Country
				from Address A
				inner join Supplier S on S.AddressId = A.AddressId
				order by 1";

			using (var conn = new SqlConnection(_connectString))
			{
				cmd.Connection = conn;
				conn.Open();

				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						list.Add((string)reader["Country"]);
					}
				}
			}

			return list;
		}

		// Requête select avec DataReader et paramètre
		// Récupère la liste de tous les fournisseurs d'un pays donné
		public static IList<Supplier> GetFournisseurs(string pays)
		{
			var list = new List<Supplier>();
			var cmd = new SqlCommand();
			cmd.CommandText = @"select S.SupplierId, S.CompanyName
							from Supplier S
							inner join Address A on S.AddressId = A.AddressId
							where A.Country = @pays
							order by 1";

			cmd.Parameters.Add(new SqlParameter
			{
				SqlDbType = SqlDbType.NVarChar,
				ParameterName = "@pays",
				Value = pays
			});

			using (var conn = new SqlConnection(_connectString))
			{
				cmd.Connection = conn;
				conn.Open();

				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var item = new Supplier();
						item.SupplierId = (int)reader["SupplierId"];
						item.CompanyName = (string)reader["CompanyName"];
						list.Add(item);
					}
				}
			}

			return list;
		}

		// Requête select scalaire avec paramètre
		// Récupère le nombre de produits fournis par l’ensemble des fournisseurs d'un pays
		public static int GetNbProduits(string pays)
		{
			var cmd = new SqlCommand();
			cmd.CommandText = @"select COUNT(*) NbProduits
				from Product p
				inner join Supplier s on (p.SupplierId = s.SupplierId)
				inner join Address a on s.AddressId = a.AddressId
				where a.Country = @pays";

			cmd.Parameters.Add(new SqlParameter
			{
				SqlDbType = SqlDbType.NVarChar,
				ParameterName = "@pays",
				Value = pays
			});

			using (var cnx = new SqlConnection(_connectString))
			{
				cmd.Connection = cnx;
				cnx.Open();
				return (int)cmd.ExecuteScalar();
			}
		}

		// Requête select de masse d'un maître - détail
		// Récupère les clients et leur commandes et stocke les données sous forme arborescente
		public static IList<Customer> GetClientsCommandes()
		{
			var list = new List<Customer>();
			var cmd = new SqlCommand();
			cmd.CommandText = @"select C.CustomerId, C.CompanyName,
				O.OrderId, OrderDate, ShippedDate, Freight,
				count(D.ProductId) ItemsCount,
				SUM(D.Quantity * D.UnitPrice) Total
				from Customer C
				left outer join Orders O on C.CustomerId = O.CustomerId
				inner join OrderDetail D on O.OrderId = D.OrderId
				group by C.CustomerId, C.CompanyName, O.OrderId, OrderDate, ShippedDate, Freight
				order by C.CustomerId, O.OrderId";

			using (var conn = new SqlConnection(_connectString))
			{
				cmd.Connection = conn;
				conn.Open();

				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						string idClient = (string)reader["CustomerId"];

						// Si l'id du client courant est différent de celui du dernier
						// client de la liste, on crée un nouvel objet Client
						Customer cli = null;
						if (list.Count == 0 || list[list.Count - 1].CustomerId != idClient)
						{
							cli = new Customer();
							cli.CustomerId = idClient;
							cli.CompanyName = (string)reader["CompanyName"];
							cli.Orders = new List<Order>();
							list.Add(cli);
						}
						else
							cli = list[list.Count - 1];

						// Création de la commande
						Order com = new Order();
						com.OrderId = (int)reader["OrderId"];
						com.OrderDate = (DateTime)reader["OrderDate"];
						if (reader["ShippedDate"] != DBNull.Value)
							com.ShippedDate = (DateTime)reader["ShippedDate"];
						com.ItemsCount = (int)reader["ItemsCount"];
						com.Total = (decimal)reader["Total"];
						com.Freight = (decimal)reader["Freight"];

						cli.Orders.Add(com);
					}
				}
			}

			return list;
		}

		// Liste des catégories
		public static IList<Category> GetCatégories()
		{
			var list = new List<Category>();
			var cmd = new SqlCommand();
			cmd.CommandText = @"select CategoryId, Name, Description from Category";

			using (var conn = new SqlConnection(_connectString))
			{
				cmd.Connection = conn;
				conn.Open();

				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var item = new Category();
						item.CategoryId = (Guid)reader["CategoryId"];
						item.Name = (string)reader["Name"];
						item.Description = (string)reader["Description"];
						list.Add(item);
					}
				}
			}

			return list;
		}

		// Liste des produits d'une catégorie donnée
		public static IList<Product> GetProduits(Guid idCategorie)
		{
			var listProduits = new List<Product>();

			var cmd = new SqlCommand();
			cmd.CommandText = @"select ProductId, Name, SupplierId, UnitPrice, UnitsInStock
									from Product where CategoryId = @categorie order by 1";

			cmd.Parameters.Add(new SqlParameter
			{
				SqlDbType = SqlDbType.UniqueIdentifier,
				ParameterName = "@categorie",
				Value = idCategorie
			});

			using (var cnx = new SqlConnection(_connectString))
			{
				cmd.Connection = cnx;
				cnx.Open();

				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var produit = new Product();
						produit.ProductId = (int)reader["ProductId"];
						produit.Name = (string)reader["Name"];
						produit.SupplierId = (int)reader["SupplierId"];
						produit.UnitPrice = (decimal)reader["UnitPrice"];
						produit.UnitsInStock = (short)reader["UnitsInStock"];
						listProduits.Add(produit);
					}
				}
			}

			return listProduits;
		}

		// Requête insert / update - ajout / modif d'un produit
		public static void AjouterModifierProduit(Product produit, Operations op)
		{
			var cmd = new SqlCommand();

			if (op == Operations.Ajout)
			{
				cmd.CommandText = @"insert Product (Name, CategoryId, SupplierId, UnitPrice, UnitsInStock)
									values (@Nom, @Cate, @Fourni, @PU, @Stock)";
			}
			else if (op == Operations.Modification)
			{
				cmd.CommandText = @"update Product set Name = @Nom, CategoryId = @Cate,
								SupplierId = @Fourni, UnitPrice = @PU, UnitsInStock = @Stock
								where ProductId = @Id";
				cmd.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@Id", Value = produit.ProductId });
			}

			cmd.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "@Nom", Value = produit.Name });
			cmd.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.UniqueIdentifier, ParameterName = "@Cate", Value = produit.CategoryId });
			cmd.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@Fourni", Value = produit.SupplierId });
			cmd.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Decimal, ParameterName = "@PU", Value = produit.UnitPrice });
			cmd.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@Stock", Value = produit.UnitsInStock });

			using (var cnx = new SqlConnection(_connectString))
			{
				cmd.Connection = cnx;
				cnx.Open();
				cmd.ExecuteNonQuery();
			}
		}

		// Requête delete - suppression d'un produit
		// Si le produit est référencé par une commande, la requête lève une
		// SqlException avec le N°547, qu'on intercepte dans le code appelant
		public static void SupprimerProduit(int id)
		{
			var cmd = new SqlCommand();
			cmd.CommandText = @"delete from Product where ProductId = @id";
			cmd.Parameters.Add(new SqlParameter
			{
				SqlDbType = SqlDbType.Int,
				ParameterName = "@id",
				Value = id
			});

			using (var conn = new SqlConnection(_connectString))
			{
				cmd.Connection = conn;
				conn.Open();
				cmd.ExecuteNonQuery();
			}
		}
	}
}
