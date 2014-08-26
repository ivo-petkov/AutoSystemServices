using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;
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

        public bool EditPerformer(Performer value, string newAuthCode)
        {
            var performer = dbContext.Performers.Find(value.PerformerId);
            if (value.AuthCode != null)
            {
                if (performer.Username != value.Username || performer.AuthCode != value.AuthCode)
                {
                    return false;
                }
            }

            performer.Name = (value.Name != null) ? value.Name : performer.Name;
            performer.AuthCode = (value.AuthCode != null) ? newAuthCode : performer.AuthCode;
            performer.Address = (value.Address != null) ? value.Address : performer.Address;
            performer.Telephone = (value.Telephone != null) ? value.Telephone : performer.Telephone;

            dbContext.SaveChanges();
            return true;
        }
    }      
}
