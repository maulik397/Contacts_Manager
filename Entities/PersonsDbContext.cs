using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
  public class PersonsDbContext : DbContext
  {

        // here using context class  is used to setup database connection ( db is added as service in program, cs where we inject connection string  from appsettings) 
        // it track changes ,store 1st cache result and return same result on same request  
        // it perfoms CRUD operation like this context.Add<>() 
    public PersonsDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }  //binds Country model with Countries table in ssms 
    public DbSet<Person> Persons { get; set; }  // binds Person with Person table in ssms 


        // this method defines how your c# models are map to database tables using Fluent API 
        //onmodelcreating method belongs to dbcontext  which is now oveerriden 
        //modelbuilder defines coulmns tables and setup realtionship  1 to many ,may to one 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder); // Ensures default EF Core behaviors are applied (like standard conventions). Prevents accidental overrides of important settings from the base class.

      modelBuilder.Entity<Country>().ToTable("Countries");
      modelBuilder.Entity<Person>().ToTable("Persons");

      //Seed to Countries
      string countriesJson = System.IO.File.ReadAllText("countries.json");
      List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

      foreach (Country country in countries)
        modelBuilder.Entity<Country>().HasData(country);


      //Seed to Persons
      string personsJson = System.IO.File.ReadAllText("persons.json");
      List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

      foreach (Person person in persons)
        modelBuilder.Entity<Person>().HasData(person);


            //Fluent API is way to configure (set things up ) using method chaining  like .hasone  .hasmany 
            /*
                Method	Purpose	Type
               .HasOne()	Configures a 1-side of a 1-to-many or 1-to-1 relationship	Relationship
               .HasMany()	Configures a many-side of a relationship	Relationship
               .HasForeignKey()	Defines the foreign key property	Relationship
               .HasData()	Adds seed (initial) data	Seeding
               .HasColumnType()	Specifies SQL data type	Column Mapping 
               .HasDefaultValue()	Sets a default column value	Column Mapping
               .HasMaxLength()	Limits string column size	Column Mapping
               .HasIndex()	Adds an index on a column	Indexing
               .HasCheckConstraint()	Adds SQL check constraint	Constraint
               .HasKey()	Sets a primary key (useful for composite keys)	Key Definition
               .HasName()	Used with constraints/indexes to name them	Naming
               .Property() lets u acces particualr column for changes
         .WithOne()	Specifies the inverse navigation property for a one-to-one relationship.	.WithOne(p => p.Passport)
         .WithMany()	Specifies the inverse navigation property for a one-to-many relationship.	.WithMany(c => c.Persons)
         .HasForeignKey()	Defines which property is the foreign key.	.HasForeignKey(p => p.CountryId)
                      */



         modelBuilder.Entity<Person>().Property(temp => temp.TIN)
        .HasColumnName("TaxIdentificationNumber")
        .HasColumnType("varchar(8)")
        .HasDefaultValue("ABC12345");

      //modelBuilder.Entity<Person>()
      //  .HasIndex(temp => temp.TIN).IsUnique();

      modelBuilder.Entity<Person>()
        .HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8");

      //Table Relations
      modelBuilder.Entity<Person>(entity =>
      {
        entity.HasOne<Country>(c => c.Country)
        .WithMany(p => p.Persons)
        .HasForeignKey(p => p.CountryID);
      });
    }
        
    // execute query without params  
    public List<Person> sp_GetAllPersons()
    {
      return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
    }
        // .ToList executes query and stores result in List<Person>  It executes the query immediately and converts the result into a List in memory
        

   //execute query with params  
    public int sp_InsertPerson(Person person)
    {
      SqlParameter[] parameters = new SqlParameter[] 
      { 
        new SqlParameter("@PersonID", person.PersonID),
        new SqlParameter("@PersonName", person.PersonName),
        new SqlParameter("@Email", person.Email),
        new SqlParameter("@DateOfBirth", person.DateOfBirth),
        new SqlParameter("@Gender", person.Gender),
        new SqlParameter("@CountryID", person.CountryID),
        new SqlParameter("@Address", person.Address),
        new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)
      };

 return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetters", parameters);

    }

//Migrations  it  is used to sync c# models with database 
// to create migration create a model class then run in nuget pm cli that Add-Migration <Migration name > then types Update-database
//to check which migration are applied which not then run command Get-Migration  so this folder is automatically created  
/*
 
add dbcontext as service in program.cs  
then create dbcontext 
 
 
 */


  }
}
