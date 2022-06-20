using System.ComponentModel.DataAnnotations.Schema;

namespace AccountMovement.Entities
{
    public class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountID { get; set; }

        public int AccountNumber { get; set; }

        public string AccountType { get; set; }

        public Decimal AccountBalance { get; set; }

        public Boolean AccountState { get; set; }

        public int ClientID { get; set; }
        public Client? Client { get; set; }

    }
}
