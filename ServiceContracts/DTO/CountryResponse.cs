using System;
using Entities;

namespace ServiceContracts.DTO
{
  /// <summary>
  /// DTO class that is used as return type for most of CountriesService methods
  /// </summary>
  public class CountryResponse
  {
    public Guid CountryID { get; set; }
    public string? CountryName { get; set; }

    //It compares the current object to another object of CountryResponse type and returns true, if both values are same; otherwise returns false
    public override bool Equals(object? obj)
    {
      if (obj == null)
      {
        return false;
      }

      if (obj.GetType() != typeof(CountryResponse))
      {
        return false;
      }
      CountryResponse country_to_compare = (CountryResponse)obj;

      return CountryID == country_to_compare.CountryID && CountryName == country_to_compare.CountryName;
    }

    //returns an unique key for the current object
    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }

  public static class CountryExtensions
  {
        /* Static members can be accessed without creating an instance of the class. 
           use to create helper method so that it ccan be accessble so that don't need to have  object state 
         
          static class, method,properties bellong to type of itself not to specific instaces  
          You don’t need to create an object to access a static member
         
         */
        //Converts from Country object to CountryResponse object
    public static CountryResponse ToCountryResponse(this Country country)
    {
      return new CountryResponse() {  CountryID = country.CountryID, CountryName = country.CountryName };
    }
  }
}
