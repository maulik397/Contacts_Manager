using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
 
  public class Person
  {
    [Key]
    public Guid PersonID { get; set; }

    [StringLength(40)] //nvarchar(40)
    //[Required]
    public string? PersonName { get; set; }

    [StringLength(40)] //nvarchar(40)
    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [StringLength(10)] //nvarchar(100)
    public string? Gender { get; set; }

    //uniqueidentifier
    public Guid? CountryID { get; set; }

    [StringLength(200)] //nvarchar(200)
    public string? Address { get; set; }

    //bit
    public bool ReceiveNewsLetters { get; set; }

    [Column("TaxIdentificationNumber", TypeName = "varchar(8)")]
    public string? TIN { get; set; }

    //adds country id to person coulm so it tells this person belongs to which country  
    [ForeignKey("CountryID")]
    public virtual Country? Country { get; set; }
    //virtual enables lazy loading means dont load class until it is needed 


  }
}
