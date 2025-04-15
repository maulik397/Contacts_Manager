using System.ComponentModel.DataAnnotations;

namespace Entities
{

  public class Country
  {

    //it sets country id as primary id   
    [Key]
    public Guid CountryID { get; set; }

    public string? CountryName { get; set; }

    //defines 1 to M  relation 1 country have multiple persons 
    public virtual ICollection<Person>? Persons { get; set; }
  }
}
