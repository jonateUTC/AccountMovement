using System.ComponentModel.DataAnnotations.Schema;

namespace AccountMovement.Entities
{
    public class Movement
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovementID { get; set; }
        public DateTime MovementDate { get; set; }
        public String MovementType { get; set; }

        public Decimal MovementValue { get; set; }

        public Decimal MovementBalance  { get; set; }

        public Decimal MovementValueIni { get; set; }

        public int AccountID { get; set; }
        public Account? Account { get; set; }

    }
}
