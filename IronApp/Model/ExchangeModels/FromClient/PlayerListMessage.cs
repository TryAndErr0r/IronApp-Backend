using System;

namespace IronApp.Model.ExchangeModels
{
    public class PlayerListMessage
    {
        public Guid gameId { get; set; }
        public Credentials credentials { get; set; }
    }
}
