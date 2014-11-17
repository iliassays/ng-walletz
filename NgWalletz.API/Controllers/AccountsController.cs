namespace NgWalletz.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    [RoutePrefix("api/Account")]
    public class AccountsController : ApiController
    {
        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(Account.AccountSeed());
        }
    }

    public class Account
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string City { get; set; }
        public decimal Balance { get; set; }

        public static List<Account> AccountSeed()
        {
            List<Account> accountList = new List<Account> 
            {
                new Account {Id = 10248, CustomerName = "Ilias", City = "Dhaka", Balance = 10000 },
                new Account {Id = 10249, CustomerName = "Tom Hank", City = "Newyork", Balance = 20000},
                new Account {Id = 10250, CustomerName = "Garry Kastern", City = "Keptown", Balance = 30000 },
                new Account {Id = 10251, CustomerName = "Abdullah Bin Abdul Munim", City = "Abu Dhabi", Balance = 40000},
                new Account {Id = 10252, CustomerName = "James Bond", City = "California", Balance = 70000}
            };

            return accountList;
        }
    }
}
