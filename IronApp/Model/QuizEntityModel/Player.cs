
using IronApp.Model.ExchangeModels.ToClient;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronApp.Model.QuizEntityModel
{
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsConnected { get; set; }
        public string Name { get; set; }
        public string RConnection { get; internal set; }

        public PlayerDto ToDto()
        {
            return new PlayerDto()
            {
                Id = Id,
                Name = Name,
                IsConnected = IsConnected,
            };
        }
    }
}