using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;

namespace AutoSystem.Repositories
{
    public class RepairsRepository: EfRepository<Repair>
    {
        private AutoSystemContext dbContext;

        public RepairsRepository(AutoSystemContext context)
            : base(context)
        {
            this.dbContext = context;
        }

        //public Repair GetByPerformerCarAndDate(int performerId, int carId, DateTime date)
        //{
        //    var repair = dbContext.Repairs.  .Include("FirstUser").Include("SecondUser").Include("Messages")
        //        .FirstOrDefault(c =>
        //            (c.FirstUser.Username == firstUsername && c.SecondUser.Username == secondUsername)
        //            ||
        //            (c.FirstUser.Username == secondUsername && c.SecondUser.Username == firstUsername));

        //    if (conversation == null)
        //    {
        //        return null;
        //    }

        //    var newConversation = new Conversation()
        //    {
        //        Id = conversation.Id,
        //        FirstUser = conversation.FirstUser,
        //        SecondUser = conversation.SecondUser,
        //    };

        //    newConversation.Messages = conversation.Messages.Select(m => new Message()
        //    {
        //        Sender = m.Sender,
        //        Date = m.Date,
        //        Content = m.Content
        //    }).ToList();

        //    return newConversation;
        //}

    }
}
