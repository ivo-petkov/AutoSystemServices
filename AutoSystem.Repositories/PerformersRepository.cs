using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace AutoSystem.Repositories
{
    public class PerformersRepository : EfRepository<Performer>
    {
        private AutoSystemContext dbContext;

        public PerformersRepository(AutoSystemContext context)
            : base(context)
        {
            this.dbContext = context;
        }

        public Performer CheckLogin(string username, string authCode)
        {
            var performer = dbContext.Performers.FirstOrDefault(u => u.Username == username && u.AuthCode == authCode);
            return performer;
        }

        public Performer GetByUsername(string username)
        {
            return dbContext.Performers.FirstOrDefault(u => u.Username == username);
        }

        public Performer GetBySessionKey(string sessionKey)
        {
            return dbContext.Performers.FirstOrDefault(u => u.SessionKey == sessionKey);
        }

        public void Logout(Performer performer)
        {
            dbContext.Performers.Attach(performer);
            performer.SessionKey = null;
            dbContext.SaveChanges();
        }

        public void SetSessionKey(Performer performer, string sessionKey)
        {
            dbContext.Performers.Attach(performer);
            performer.SessionKey = sessionKey;
            dbContext.SaveChanges();
        }

        public bool EditPerformer(Performer value)
        {
            var performer = dbContext.Performers.Find(value.PerformerId);
            if (value == null)
            {
                return false;
            }

            performer.Name = value.Name;
            performer.Username = value.Username;
            performer.AuthCode = value.AuthCode;
            performer.Address = value.Address;
            performer.Telephone = value.Telephone;
            performer.Mol = value.Mol;
            performer.Bulstat = value.Bulstat;
            performer.Email = value.Email;

            dbContext.SaveChanges();
            return true;
        }

        public bool EditClients(ICollection<Client> editedClients, string performerUsername)
        {
            var performer =   this.GetByUsername(performerUsername);
            if (performer == null)
            {
                return false;
            }

            int count = performer.Clients.Count;
            var performerClients = performer.Clients.ToList();
            for (int i = 0; i < count; i++)
            {
                if (!editedClients.Contains(performerClients[i]))
                {
                    performer.Clients.Remove(performerClients[i]);
                }
            }

            foreach (var client in editedClients)
            {
                if (!performer.Clients.Contains(client))
                {
                    performer.Clients.Add(client);  
                }
            }  
            dbContext.SaveChanges();
            return true;        
        }

        public bool CheckForRepairs(int performerId)
        {
            var performer = this.Get(performerId);

            if (performer.Repairs == null || performer.Repairs.Count == 0)
            {
                return false;
            }
            return true;
        }
    }      
}
