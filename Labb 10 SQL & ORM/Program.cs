using Labb_10_SQL___ORM.Data;
using Labb_10_SQL___ORM.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualBasic;
using System.Data;
using System.Linq;
using System.Text;

namespace Labb_10_SQL___ORM
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//While-loop to keep menu going
			while (true)
			{
				//Prints menu
				ShowMenu();
				
				//Try user input
				int.TryParse(Console.ReadLine(), out int choice);
				switch (choice)
				{
					//Prints all customers
					case 1:
						ShowCustomers();
						break;
					//Prints a specific customer based on company name
					case 2:
						ChooseCustomer();
						break;
					//Creates a new customer
					case 3:
						CreateCustomer();
						break;
					//Exits the program
					case 4:
						Environment.Exit(0);
						break;
					//Default for faulty input from user
					default:
						Console.Clear();
						Console.WriteLine("Thats not a valid menu choice!\n");
						break;
				}
			}
		}

		//Prints menu
		public static void ShowMenu() 
		{
			Console.WriteLine("MENU");
			Console.WriteLine("1) Show all customers");
			Console.WriteLine("2) Choose a customer from the list");
			Console.WriteLine("3) Create a new customer");
			Console.WriteLine("4) Exit");
		}
		//Show a list of all the customers
		public static void ShowCustomers()
		{
			Console.Clear();
			using (var context = new NorthWindContext())
			{
				//Retrieve information about all customers
				var customers = context.Customers
					.Select(c => new { c.CompanyName, c.Country, c.Region, c.Phone, numberOforders = c.Orders.Count });

				while(true)
				{
					//Give menu choice for ascending or descending list
					Console.WriteLine("Order by company name:\n1) Descending\n2) Ascending");
					int.TryParse(Console.ReadLine(), out int choice);
					//Orders by descending
					if (choice == 1)
					{
						customers
							.OrderByDescending(c => c.CompanyName)
							.ToList();
						break;
					}
					//Orders by ascending
					else if (choice == 2)
					{
						customers
								.OrderBy(c => c.CompanyName)
								.ToList();
						break;
					}
					//Invalid input from user
					else
					{
						Console.Clear();
                        Console.WriteLine("Thats not a valid input!\n");
                    }
				}
				//Prints out all customers in list
				foreach (var c in customers)
				{
					Console.WriteLine(
						$"Company Name: {c.CompanyName}\n" +
						$"Country: {c.Country}\n" +
						$"Region: {c.Region}\n" +
						$"Phone: {c.Phone}\n" +
						$"Number of orders: {c.numberOforders}\n");
				}
				Console.Write("Press any button to continue");
				Console.ReadLine();
				Console.Clear();
			}
		}

		//Shows a specific customers information and orders
		static void ChooseCustomer()
		{
			Console.Clear();
            Console.Write("Type the name of the company you want information about: ");
			while (true)
			{
				//Input from user on what company to look up
				string input = Console.ReadLine();
				using (var context = new NorthWindContext())
				{
					//Retrieve information about specific company if name exists
					if (context.Customers.Any(c => c.CompanyName == input))
					{
						var customer = context.Customers
							.Select(c => new { 
								c.CustomerId,
								c.CompanyName, 
								c.ContactName, 
								c.ContactTitle,
								c.Address,
								c.City,
								c.Country,
								c.PostalCode,
								c.Region,
								c.Phone, 
								c.Fax,})
							.Where(c => c.CompanyName == input)
							.ToList();

						//Retrieve every order by the customer
						var CustomerOrders = context.Orders
							.Where(o => customer.Select(c => c.CustomerId).Contains(o.CustomerId))
							.Where(o => o.OrderId == o.OrderDetails.Single().OrderId)
							.Where (o => o.OrderDetails.Single().Quantity != 0)
							.Select(o => new {
								o.OrderDetails.Single().Quantity,
								o.OrderDetails.Single().Product.ProductName,
								o.OrderDate,
								totalPrice = (double)o.OrderDetails.Single().UnitPrice * o.OrderDetails.Single().Quantity})
								.ToList();

						//Print customer information
						foreach (var c in customer)
						{
							Console.WriteLine(
								$"Company Name: {c.CompanyName}\n" +
								$"Contact Name: {c.ContactName}\n" +
								$"Contact Title: {c.ContactTitle}\n" +
								$"Adress:: {c.Address}\n" +
								$"City:: {c.City}\n" +
								$"Country: {c.Country}\n" +
								$"Postal code:: {c.PostalCode}\n" +
								$"Region: {c.Region}\n" +
								$"Phone: {c.Phone}\n" +
								$"Fax: {c.Fax}\n");
						}

                        Console.WriteLine("Orders:\n");
						//Print order information
						foreach (var order in CustomerOrders)
						{
                            Console.WriteLine(
								$"Ordered: {order.Quantity} {order.ProductName}" +
								$", ordered date: {order.OrderDate}" +
								$", for a total price of: {order.totalPrice}$");
                        }
						Console.WriteLine("Press any button to continue");
						Console.ReadLine();
						Console.Clear();
						break;
					}

					//If the input of company name is invalid
					else
					{
                        Console.Write("There is no company with that name, try again: ");
                    }
				}
			}
			
        }

		static void CreateCustomer()
		{
			
			Console.Clear ();
			//Collect information about a customer
            Console.WriteLine("--Add a new customer--");
            Console.WriteLine("--Press [enter] if null--");

			string companyName;
			//While-loop so the company name cant be null
			while (true)
			{
				Console.Write("Company name: ");
				companyName = Console.ReadLine();
				if (companyName != "")
				{
					break;
				}
				else
				{
                    Console.WriteLine("Company name cannot be empty!");
                }
			}
			//Collect rest of information and add null if empty string
			Console.Write("Contact name: ");
			string contactName = CheckNull(Console.ReadLine());
			Console.Write("Contact title: ");
			string title = CheckNull(Console.ReadLine());
			Console.Write("Address: ");
			string address = CheckNull(Console.ReadLine());
			Console.Write("City: ");
			string city = CheckNull(Console.ReadLine());
			Console.Write("Region: ");
			string region = CheckNull(Console.ReadLine());
			Console.Write("Postal code: ");
			string postCode = CheckNull(Console.ReadLine());
			Console.Write("Country: ");
			string country = CheckNull(Console.ReadLine());
			Console.Write("Phone: ");
			string phone = CheckNull(Console.ReadLine());
			Console.Write("Fax: ");
			string fax = CheckNull(Console.ReadLine());
			
			//Create a new customer with a random ID
			Customer customer = new Customer()
			{
				CustomerId = RandomId(),
				CompanyName = companyName,
				ContactName = contactName,
				ContactTitle = title,
				Address = address,
				City = city,
				Region = region,
				PostalCode = postCode,
				Country = country,
				Phone = phone,
				Fax = fax
			};

			//Add the customer to the table and save changes to database
			using (var context = new NorthWindContext())
			{
				context.Customers.Add(customer);
				context.SaveChanges();
                Console.WriteLine("Customer added!");
                Console.WriteLine("Press any key to return to main menu");
				Console.ReadKey();
				Console.Clear();
            }
		}

		//Generate Random string ID
		static string RandomId()
		{
			string ID = "";
			int RandomNumber;
			char RandomLetter;
			Random random = new Random();
			for (int i = 0; i < 5; i++)
			{
				RandomNumber = random.Next(1, 26) + 65;
				RandomLetter = (char)RandomNumber;
				ID += RandomLetter;
			}

			return ID;
		}

		//Method to return an empty string as null
		static string CheckNull(string value)
		{
			if (value == "")
				return null;
			else return value;
		}
	}
}