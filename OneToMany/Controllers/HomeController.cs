using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneToMany.Models;

namespace OneToMany.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            configuration = config;
        }

        public IActionResult Index()
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            List<Catagory> catagoryList = new List<Catagory>();
            List<SubCatagory> subcatagoryList = new List<SubCatagory>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"select 
                                                    c.Id,
                                                    sc.Id as 'subCat_id',
                                                    c.Name as 'Catagory Name',
                                                    sc.Name as 'SubCatagory Name',
                                                    sc.catagory_Id 
                                                    from Catagory c left
                                                    join SubCatagory sc
                                                    on c.Id = sc.catagory_Id", con);
                //cmd.CommandType = CommandType.TableDirect;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                int isSame = 0;
                int isSame2 = 0;
                int count = 0;
                bool isFromElseCon = false;
                Catagory catagory = new Catagory();
                SubCatagory subCatagory = new SubCatagory();
                while (rdr.Read())
                {
                    count += 1;


                    if(!(rdr["catagory_Id"] is DBNull))
                    {
                        if (Convert.ToInt32(rdr["Id"]) == Convert.ToInt32(rdr["catagory_Id"]))
                        {
                            if (isSame2 != Convert.ToInt32(rdr["catagory_Id"]) && count != 1)
                            {

                            }
                            else
                            {
                                subCatagory = new SubCatagory();
                                subCatagory.cat_id = Convert.ToInt32(rdr["catagory_Id"]);
                                subCatagory.Name = Convert.ToString(rdr["SubCatagory Name"]);
                                subCatagory.Id = Convert.ToInt32(rdr["subCat_id"]);
                                subcatagoryList.Add(subCatagory);


                            }
                            if (isSame != Convert.ToInt32(rdr["Id"]))
                            {
                                if (isSame2 != Convert.ToInt32(rdr["catagory_Id"]) && count != 1)
                                {

                                }
                                else
                                {
                                    isSame = Convert.ToInt32(rdr["Id"]);
                                    catagory.Id = Convert.ToInt32(rdr["Id"]);
                                    catagory.Name = Convert.ToString(rdr["Catagory Name"]);
                                }


                                //catagoryList.Add(catagory);
                            }
                            if (isSame2 != Convert.ToInt32(rdr["catagory_Id"]) && count != 1)
                            {
                                catagory.SubCatagories = subcatagoryList;
                                catagoryList.Add(catagory);

                                //same process
                                catagory = new Catagory();
                                subCatagory = new SubCatagory();
                                subcatagoryList = new List<SubCatagory>();

                                subCatagory.cat_id = Convert.ToInt32(rdr["catagory_Id"]);
                                subCatagory.Name = Convert.ToString(rdr["SubCatagory Name"]);
                                subCatagory.Id = Convert.ToInt32(rdr["subCat_id"]);
                                subcatagoryList.Add(subCatagory);

                                if (isSame != Convert.ToInt32(rdr["Id"]))
                                {
                                    isSame = Convert.ToInt32(rdr["Id"]);
                                    catagory.Id = Convert.ToInt32(rdr["Id"]);
                                    catagory.Name = Convert.ToString(rdr["Catagory Name"]);

                                    catagoryList.Add(catagory);
                                }
                            }

                            isSame2 = Convert.ToInt32(rdr["Id"]);

                        }
                    }
                    
                    else
                    {
                        if (isSame != Convert.ToInt32(rdr["Id"]))
                        {
                            isSame = Convert.ToInt32(rdr["Id"]);
                            catagory = new Catagory();
                            catagory.Id = Convert.ToInt32(rdr["Id"]);
                            catagory.Name = Convert.ToString(rdr["Catagory Name"]);
                            isFromElseCon = true;
                            catagoryList.Add(catagory);
                        }
                    }

                    if (!isFromElseCon && subcatagoryList.Count > 0 && subcatagoryList != null)
                    {
                        isFromElseCon = false;
                        catagory.SubCatagories = subcatagoryList;
                    }
                }
                
                    
                //catagoryList.Add(catagory);
                con.Close();
            }
            Console.WriteLine(catagoryList);
            return View(catagoryList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
