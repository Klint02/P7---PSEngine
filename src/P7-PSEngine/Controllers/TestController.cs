/*
using Models;
using MySqlConnector;
using System;
using System.Reflection;
using System.ComponentModel.Design;
using System.Threading.Tasks;

namespace Testing;

public class TestController {

    //Create the task, then get the result and manipulate it and return the data
    public static string test (MySqlDataSource database, string name) {
        Task<User> task = TestRepository.FetchUserByName(database, name);
        User user = task.Result;

        Console.WriteLine($"id: {user.id}, Name: {user.name}");
        if (user.id != -1) {
            return $"Found user id: {user.id}, Name: {user.name}";

        } else {
            return $"No user {name}";
        }
    }

    public static void MigrationTest () {
        Console.WriteLine("test");
        User user = new User (-1, "");

        Type objType = typeof(User); 
  
        // try-catch block for handling Exception 
        try { 
  
            // Getting array of Fields by 
            // using GetField() Method 
            FieldInfo[] info = objType.GetFields(); 
  
            // Display the Result 
            Console.Write("Fields of current type is as Follow: "); 
            for (int i = 0; i < info.Length; i++) 
                Console.WriteLine(" {0}", info[i]); 
        } 
  
        // catch ArgumentNullException here 
        catch (ArgumentNullException e)  
        { 
            Console.Write("name is null."); 
            Console.Write("Exception Thrown: "); 
            Console.Write("{0}", e.GetType(), e.Message); 
        } 
    }
}

*/