using System;

namespace IronApp.Model.ExchangeModels
{
    public class NewGameMessage
    {
        public Guid GameId { get; set; }
        public string UserName { get; set; }
        public string BearerToken { get; internal set; }
        public int PlayerId { get; internal set; }
    }

    public class GameCreated{
        
        public Guid GameId { get; set; }        
    }
}
