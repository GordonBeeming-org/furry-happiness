using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace FurryHappiness.Pages
{
  public class IndexModel : PageModel
  {
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfiguration _config;

    [BindProperty]
    public string? FirstName { get; set; }
    [BindProperty]
    public string? LastName { get; set; }
    [BindProperty]
    public string? EmailAddress { get; set; }
    [BindProperty]
    public string? Age { get; set; }

    [ViewData]
    public string? PageMessage { get; set; }
    [ViewData]
    public string? PageError { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
    {
      _logger = logger;
      _config = config;
    }

    public void OnGet()
    {
    }

    public void OnPost()
    {
      try
      {
        using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
          connection.Open();
          var sql = $@"
INSERT INTO dbo.[tb_Contacts]
  (FirstName,LastName,EmailAddress,Age)
VALUES
  (@FirstName,@LastName,@EmailAddress,{Age})";
          using (var cmd = new SqlCommand(sql, connection))
          {
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            cmd.Parameters.AddWithValue("@LastName", LastName);
            cmd.Parameters.AddWithValue("@EmailAddress", EmailAddress);

            cmd.ExecuteNonQuery();
          }
        }
        PageMessage = $"Welcome to the club {FirstName}!";
      }
      catch (Exception ex)
      {
        PageError = ex.Message;
      }
    }
  }
}
