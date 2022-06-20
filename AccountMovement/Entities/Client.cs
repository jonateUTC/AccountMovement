using System.ComponentModel.DataAnnotations.Schema;

namespace AccountMovement.Entities
{
    public class Client : Persons
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientID { get; set; }

        public int ClientPassword { get; set; }

        public Boolean ClientState { get; set; }
        
    }
}
