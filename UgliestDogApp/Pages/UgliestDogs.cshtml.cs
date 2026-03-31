using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

public class UgliestDogsModel : PageModel
{
    public List<SelectListItem> DogList {get; set;}
    public Dog SelectedDog {get; set;}

    //On get call method LoadDogList
    public void OnGet()
    {
        LoadDogList();
    }
    //On Form input Call LoadDogList
    public void OnPost(string selectedDog)
    {
        //If the selected dog found by LoadDogList is not Null call GetDogByID method to set Selected dog to that dog object
        LoadDogList();
        if (!string.IsNullOrEmpty(selectedDog))
        {
            SelectedDog = GetDogById(int.Parse(selectedDog));
        }
    }

// Load DogList method
    private void LoadDogList()
    {
        //Initialization of DogList
        DogList = new List<SelectListItem>();
        //Open New connection to the UgliestDogs.db SQLite Database
        using (var connection = new SqliteConnection("Data Source=UgliestDogs.db"))
        {
            connection.Open();
            var command = connection.CreateCommand();
            //SQL select command selecting ID and Name FROM Dogs Entity in Database
            command.CommandText = "SELECT Id, Name FROM Dogs";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    //Add Selections to DogList
                    DogList.Add(new SelectListItem
                    {
                        Value = reader.GetInt32(0).ToString(),
                        Text = reader.GetString(1)
                    });
                }
            }
        }
    }
    //GetDogById method taking a id Integer 
    private Dog GetDogById(int id)
    {
        //open connection to the UgliestDogs.db Sqlite database
        using (var connection = new SqliteConnection("Data Source=UgliestDogs.db"))
        {
            connection.Open();
            //SQL Select command selecting Id = to the paramaterized Id
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Dogs WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            using (var reader = command.ExecuteReader())
            {
                //If reader finds dog in Database with ID return Dog Object Parsing Attributes from the returned reader string
                if (reader.Read())
                {
                    return new Dog
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Breed = reader.GetString(2),
                        Year = reader.GetInt32(3),
                        ImageFileName = reader.GetString(4)
                    };
                }
            }
        }
        return null;
    }
}

//public class Dog Defines a dog object with Id, Name, Breed, Year, and Image File Name as attributes
public class Dog
{
    public int Id {get; set;}
    public string Name {get; set;}
    public string Breed {get; set;}
    public int Year {get; set;}
    public string ImageFileName {get; set;}
}