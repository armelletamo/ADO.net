-- Création de la fonction récupérant le nombre de produits fournis
-- par l'ensemble desforunisseurs d'un pays
drop function if exists ufn_GetNbProduits
go
create function ufn_GetNbProduits (@pays nvarchar(50))
returns int
as
begin
	return (select COUNT(*) NbProduits
			from Product p
			inner join Supplier s on (p.SupplierId = s.SupplierId)
			inner join Address a on s.AddressId = a.AddressId
			where a.Country = @pays)
end
go

-- Appel de la fonction
select dbo.ufn_GetNbProduits ('France')
