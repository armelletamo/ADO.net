using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{
    internal class Client_Commande : Page

    {
        private IList<Client> _clients;
        public Client_Commande() : base("Clients et commandes")
        {

        }

        public override void Display()
        {
            // Affichage de la liste des clients
            _clients = Contexte.GetClientsCommandes();
            ConsoleTable.From(_clients).Display("clients");

            // Affichage de la liste des commandes du client sélectionné
            string id = Input.Read<string>("De quel client souhaitez-vous afficher la liste des commandes ? ");
            var commandes = _clients.Where(c => c.customerid == id).Select(c => c.commandes).FirstOrDefault();
            ConsoleTable.From(commandes).Display("commandes");
        }
        

    }
}
