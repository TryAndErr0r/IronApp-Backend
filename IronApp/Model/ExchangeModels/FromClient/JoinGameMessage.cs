using System;

namespace IronApp.Model.ExchangeModels
{
    public class JoinGameMessage    
    {
        public Guid gameId { get; set; }    
        public Credentials credentials { get; set; }  
    }
}
